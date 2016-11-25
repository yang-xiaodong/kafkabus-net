﻿namespace KafkaBus.Kafka.Subscription
{
    public enum SubscriberAckBehavior
    {
        /// <summary>
        /// Requests are explicitly acknowledged after they have been fully processed.
        /// Requests in an unexpected format are rejected.
        /// This is the default behavior.
        /// </summary>
        ProcessedRequests,

        /// <summary>
        /// Requests are automatically acknowledged by the broker once they are received by the subscriber.
        /// </summary>
        Automatic
    }
}