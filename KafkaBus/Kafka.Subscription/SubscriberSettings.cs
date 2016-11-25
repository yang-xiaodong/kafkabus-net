using System;

namespace KafkaBus.Kafka.Subscription
{
    public class SubscriberSettings
    {
        private KafkaBusSubscriber _subscriber;
        private SubscriberAckBehavior _ackBehavior;
        private int _prefetchCount;
        private const int DEFAULT_PREFETCH_COUNT = 50;

        public SubscriberSettings() {
            PrefetchCount = DEFAULT_PREFETCH_COUNT;
        }

        public SubscriberAckBehavior AckBehavior {
            get {
                return _ackBehavior;
            }
            set {
                EnsureNotStarted();
                _ackBehavior = value;
            }
        }

        public int PrefetchCount {
            get { return _prefetchCount; }
            set {
                EnsureNotStarted();
                if (value < 0 || value > ushort.MaxValue) throw new ArgumentException("PrefetchCount must be between 0 and 65535.");
                _prefetchCount = value;
            }
        }

        internal KafkaBusSubscriber Subscriber {
            set {
                if (_subscriber != null) throw new InvalidOperationException("This instance of SubscriberSettings is already in use by another subscriber.");
                _subscriber = value;
            }
        }

        private void EnsureNotStarted() {
            if (_subscriber != null && _subscriber.HasStarted) {
                throw new InvalidOperationException("This instance has already started. Properties can only be modified before starting the subscriber.");
            }
        }
    }
}