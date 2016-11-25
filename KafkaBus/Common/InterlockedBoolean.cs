﻿using System;
using System.Threading;

namespace KafkaBus.Common
{
    /// <summary>
    /// Provides a boolean value that can be atomically modified using Interlocked operations.
    /// Use this struct only as a field in a class and access it directly from any callee.
    /// Do not pass this struct to methods/indexers etc. as it will be copied.
    /// </summary>
    /// <remarks>
    /// Consider using a volatile bool field instead of this type, unless you need Exchange or CompareExchange (SetIf) operations.
    /// This type is implicitly convertible to boolean.
    /// </remarks>
    public struct InterlockedBoolean : IComparable, IComparable<InterlockedBoolean>, IComparable<bool>, IEquatable<InterlockedBoolean>, IEquatable<bool>
    {
        private const int FALSE = 0;
        private const int TRUE = ~FALSE; //-1
        private volatile int _value; //Will be initialized as False

        /// <summary>
        /// Returns True, if the value is false
        /// </summary>
        public bool IsFalse {
            get {
                return _value == FALSE;
            }
        }

        /// <summary>
        /// Returns True, if the value is false
        /// </summary>
        public bool IsTrue {
            get {
                return _value != FALSE;
            }
        }

        /// <summary>
        /// Sets the value to a specified value
        /// </summary>
        /// <param name="value">The new value</param>
        public void Set(bool value) {
            int i_value = value == true ? TRUE : FALSE;
            _value = i_value;
        }

        /// <summary>
        /// Sets to a specified new value if the existing value matches a specified value
        /// </summary>
        /// <param name="valueEquals">The compared value</param>
        /// <param name="newValue">The new value</param>
        /// <returns>True if the set operation succeeded, false otherwise</returns>
        public bool SetIf(bool valueEquals, bool newValue) {
            return Interlocked.CompareExchange(ref _value, newValue ? TRUE : FALSE, valueEquals ? TRUE : FALSE) == (valueEquals ? TRUE : FALSE);
        }

        /// <summary>
        /// Sets to true if the existing value matches a specified value
        /// </summary>
        /// <param name="valueEquals"></param>
        /// <returns>True if the set operation succeeded, false otherwise</returns>
        public bool SetTrueIf(bool valueEquals) {
            return SetIf(valueEquals, true);
        }

        /// <summary>
        /// Sets to false if the existing value matches a specified value
        /// </summary>
        /// <param name="valueEquals"></param>
        /// <returns>True if the set operation succeeded, false otherwise</returns>
        public bool SetFalseIf(bool valueEquals) {
            return SetIf(valueEquals, false);
        }

        /// <summary>
        /// Atomically exchanges the value to a specified value
        /// </summary>
        /// <returns>
        /// The original value
        /// </returns>
        /// <param name="value">The new value</param>
        public bool Exchange(bool value) {
            int i_value = value == true ? TRUE : FALSE;
            return Interlocked.Exchange(ref _value, i_value) == FALSE ? false : true;
        }

        public int CompareTo(object obj) {
            if (obj == null) {
                return 1;
            }

            if (obj is InterlockedBoolean) {
                return CompareTo((InterlockedBoolean)obj);
            }

            if (obj is bool) {
                return CompareTo((bool)obj);
            }

            throw new Exception("Object must be of type InterlockedBoolean or Boolean");
        }

        public int CompareTo(InterlockedBoolean other) {
            return _value.CompareTo(other._value);
        }

        public int CompareTo(bool other) {
            return IsTrue.CompareTo(other);
        }

        public bool Equals(InterlockedBoolean other) {
            return _value.Equals(other._value);
        }

        public bool Equals(bool other) {
            return IsTrue.Equals(other);
        }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override string ToString() {
            return IsTrue.ToString();
        }

        public override int GetHashCode() {
            return IsTrue.GetHashCode();
        }

        public static bool operator ==(InterlockedBoolean a, InterlockedBoolean b) {
            return a._value == b._value;
        }

        public static bool operator !=(InterlockedBoolean a, InterlockedBoolean b) {
            return a._value != b._value;
        }

        public static bool operator ==(InterlockedBoolean a, bool b) {
            return a.IsTrue == b;
        }

        public static bool operator !=(InterlockedBoolean a, bool b) {
            return a.IsTrue != b;
        }

        public static bool operator ==(bool a, InterlockedBoolean b) {
            return a == b.IsTrue;
        }

        public static bool operator !=(bool a, InterlockedBoolean b) {
            return a != b.IsTrue;
        }

        public static implicit operator bool(InterlockedBoolean val) {
            return val.IsTrue;
        }
    }
}