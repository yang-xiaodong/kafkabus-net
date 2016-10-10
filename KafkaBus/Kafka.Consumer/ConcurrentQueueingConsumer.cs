using RdKafka;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;


namespace KafkaBus.Kafka.Consumer
{
    /// <summary>
    /// Provides an implementation of a queuing consumer that uses a ConcurrentQueue internally.
    /// </summary>
    internal sealed class ConcurrentQueueingConsumer : EventConsumer
    {
        readonly ConcurrentQueue<Message> _queue = new ConcurrentQueue<Message>();
        readonly ManualResetEventSlim _itemQueuedEvent;
        volatile bool _isRunning = false;
        volatile bool _isClosed = false;

        public ConcurrentQueueingConsumer(Config config, string brokerList = null) : base(config, brokerList) {

        }

        public bool IsRunning {
            get {
                return _isRunning;
            }
            private set {
                _isRunning = value;
            }
        }


    }
}
