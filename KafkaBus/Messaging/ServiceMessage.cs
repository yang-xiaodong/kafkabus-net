﻿using KafkaBus.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace KafkaBus.Messaging
{
    internal class ServiceMessage
        : IFeatureCollection, IHttpRequestFeature, IHttpResponseFeature, IHttpConnectionFeature
    {
        private int featureRevision;
        private object _currentIHttpRequestFeature;
        private object _currentIHttpResponseFeature;
        private object _currentIHttpConnectionFeature;
        private List<KeyValuePair<Type, object>> otherFeatures;
        private volatile bool _disposed;
        private string _scheme;
        private string _pathBase;
        private string _path;
        private string _queryString;

        private readonly object _onStartingSync = new object();
        private readonly object _onCompletedSync = new object();

        internal List<KeyValuePair<Func<object, Task>, object>> _onStarting;
        internal List<KeyValuePair<Func<object, Task>, object>> _onCompleted;
        internal Exception _applicationException;

        private static SequenceGenerator connectionIdSeqGen = new SequenceGenerator(DateTime.UtcNow.Ticks, true);

        internal bool HasResponseStarted {
            get {
                if (OriginalResponseBody == null) return false;
                return OriginalResponseBody.Length > 0;
            }
        }

        internal bool HasApplicationException {
            get {
                return _applicationException != null;
            }
        }

        internal MemoryStream OriginalResponseBody { get; private set; }

        internal MemoryStream OriginalRequestBody { get; private set; }

        internal void CreateResponseBody() {
            //TODO: Implement a pooled MemoryStream and replace MemoryStream throughout solution.
            OriginalResponseBody = new MemoryStream();
            ((IHttpResponseFeature)this).Body = OriginalResponseBody;
        }

        internal void CreateRequestBody(byte[] content) {
            //TODO: Implement a pooled MemoryStream and replace MemoryStream throughout solution.
            OriginalRequestBody = new MemoryStream(content);
            ((IHttpRequestFeature)this).Body = OriginalRequestBody;
        }

        public void OnStarting(Func<object, Task> callback, object state) {
            lock (_onStartingSync) {
                if (_onStarting == null) {
                    _onStarting = new List<KeyValuePair<Func<object, Task>, object>>();
                }
                _onStarting.Add(new KeyValuePair<Func<object, Task>, object>(callback, state));
            }
        }

        public void OnCompleted(Func<object, Task> callback, object state) {
            lock (_onCompletedSync) {
                if (_onCompleted == null) {
                    _onCompleted = new List<KeyValuePair<Func<object, Task>, object>>();
                }
                _onCompleted.Add(new KeyValuePair<Func<object, Task>, object>(callback, state));
            }
        }

        internal ServiceMessage() {
            _currentIHttpRequestFeature = this;
            _currentIHttpResponseFeature = this;
            _currentIHttpConnectionFeature = this;

            //Set connection feature properties
            ((IHttpConnectionFeature)this).RemoteIpAddress = IPAddress.IPv6Any;
            ((IHttpConnectionFeature)this).LocalIpAddress = null;
            ((IHttpConnectionFeature)this).ConnectionId = connectionIdSeqGen.GetNext().ToString();
        }

        #region IFeatureCollection Implementation

        object IFeatureCollection.this[Type key] {
            get {
                if (key == typeof(IHttpRequestFeature)) { return _currentIHttpRequestFeature; }
                if (key == typeof(IHttpResponseFeature)) { return _currentIHttpResponseFeature; }
                if (key == typeof(IHttpConnectionFeature)) { return _currentIHttpConnectionFeature; }

                if (otherFeatures == null) return null;
                foreach (var kv in otherFeatures) {
                    if (kv.Key == key) return kv.Value;
                }

                return null;
            }

            set {
                featureRevision++;
                if (key == typeof(IHttpRequestFeature)) { _currentIHttpRequestFeature = value; }
                if (key == typeof(IHttpResponseFeature)) { _currentIHttpResponseFeature = value; }
                if (key == typeof(IHttpConnectionFeature)) { _currentIHttpConnectionFeature = value; }

                if (otherFeatures == null) {
                    otherFeatures = new List<KeyValuePair<Type, object>>();
                }

                for (int i = 0; i < otherFeatures.Count; i++) {
                    if (otherFeatures[i].Key == key) {
                        otherFeatures[i] = new KeyValuePair<Type, object>(key, value);
                        return;
                    }
                }

                otherFeatures.Add(new KeyValuePair<Type, object>(key, value));
            }
        }

        bool IFeatureCollection.IsReadOnly {
            get {
                return false;
            }
        }

        int IFeatureCollection.Revision {
            get {
                return featureRevision;
            }
        }

        public void Dispose() {
            if (_disposed) return;
            _disposed = true;

            if (OriginalRequestBody != null) {
                try {
                    OriginalRequestBody.Dispose();
                }
                catch { }
            }

            if (OriginalResponseBody != null) {
                try {
                    OriginalResponseBody.Dispose();
                }
                catch { }
            }

            IDisposable disposable;
            if (otherFeatures != null) {
                foreach (var feature in otherFeatures) {
                    if (feature.Value != null) {
                        disposable = feature.Value as IDisposable;
                        if (disposable != null) {
                            try {
                                disposable.Dispose();
                            }
                            catch {
                            }
                        }
                    }
                }
            }
        }

        IEnumerator<KeyValuePair<Type, object>> IEnumerable<KeyValuePair<Type, object>>.GetEnumerator() {
            return GetFeatureCollectionEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetFeatureCollectionEnumerable().GetEnumerator();
        }

        private IEnumerable<KeyValuePair<Type, object>> GetFeatureCollectionEnumerable() {
            if (_currentIHttpRequestFeature != null) {
                yield return new KeyValuePair<Type, object>(typeof(IHttpRequestFeature), _currentIHttpRequestFeature as IHttpRequestFeature);
            }
            if (_currentIHttpResponseFeature != null) {
                yield return new KeyValuePair<Type, object>(typeof(IHttpResponseFeature), _currentIHttpResponseFeature as IHttpResponseFeature);
            }

            if (otherFeatures != null) {
                foreach (var feature in otherFeatures) {
                    yield return feature;
                }
            }
        }

        #endregion IFeatureCollection Implementation

        #region IHttpRequestFeature Implementation

        string IHttpRequestFeature.Protocol {
            get; set;
        }

        string IHttpRequestFeature.Scheme {
            get {
                return _scheme ?? "http";
            }

            set {
                _scheme = value;
            }
        }

        string IHttpRequestFeature.Method {
            get; set;
        }

        string IHttpRequestFeature.PathBase {
            get {
                return _pathBase ?? String.Empty;
            }

            set {
                _pathBase = value;
            }
        }

        string IHttpRequestFeature.Path {
            get { return _path ?? String.Empty; }
            set { _path = value; }
        }

        string IHttpRequestFeature.QueryString {
            get { return _queryString ?? String.Empty; }
            set { _queryString = value; }
        }

        IHeaderDictionary IHttpRequestFeature.Headers {
            get; set;
        }

        Stream IHttpRequestFeature.Body {
            get; set;
        }

        #endregion IHttpRequestFeature Implementation

        #region IHttpResponseFeature Implementation

        int IHttpResponseFeature.StatusCode {
            get; set;
        }

        string IHttpResponseFeature.ReasonPhrase {
            get; set;
        }

        IHeaderDictionary IHttpResponseFeature.Headers {
            get; set;
        }

        Stream IHttpResponseFeature.Body {
            get; set;
        }

        bool IHttpResponseFeature.HasStarted {
            get {
                return HasResponseStarted;
            }
        }

        void IHttpResponseFeature.OnStarting(Func<object, Task> callback, object state) {
            OnStarting(callback, state);
        }

        void IHttpResponseFeature.OnCompleted(Func<object, Task> callback, object state) {
            OnCompleted(callback, state);
        }

        public TFeature Get<TFeature>() {
            var v = ((IFeatureCollection)this)[typeof(TFeature)];
            if (v is TFeature) {
                return (TFeature)v;
            }
            return default(TFeature);
        }

        public void Set<TFeature>(TFeature instance) {
            ((IFeatureCollection)this)[typeof(TFeature)] = instance;
        }

        #endregion IHttpResponseFeature Implementation

        #region IHttpConnectionFeature Implementation

        IPAddress IHttpConnectionFeature.RemoteIpAddress { get; set; }

        IPAddress IHttpConnectionFeature.LocalIpAddress { get; set; }

        int IHttpConnectionFeature.RemotePort { get; set; }

        int IHttpConnectionFeature.LocalPort { get; set; }

        public string RawTarget { get; set; }

        public string ConnectionId { get; set; }

        #endregion IHttpConnectionFeature Implementation
    }
}