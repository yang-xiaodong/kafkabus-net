using KafkaBus.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KafkaBus.Kafka.Subscription
{
    public class KafkaBusSubscriber : IKafkaBusSubscriber
    {

        readonly InterlockedBoolean hasStarted;

        public KafkaBusSubscriber() {

        }

     

        public IList<string> ConnectionNames {
            get {
                throw new NotImplementedException();
            }
        }

        public string Id {
            get {
                throw new NotImplementedException();
            }
        }

        internal bool HasStarted {
            get {
                return hasStarted;
            }
        }
        //public void Restart() {
        //    hasStarted.Set(true);

        //    //CLose connections and channels
        //    if (subscriberChannel != null) {
        //        if (subscriberConsumer != null) {
        //            try {
        //                subscriberChannel.Channel.BasicCancel(subscriberConsumer.ConsumerTag);
        //            }
        //            catch { }
        //        }

        //        try {
        //            subscriberChannel.Close();
        //        }
        //        catch {
        //        }
        //    }

        //    if (workChannel != null) {
        //        if (workConsumer != null) {
        //            try {
        //                workChannel.Channel.BasicCancel(workConsumer.ConsumerTag);
        //            }
        //            catch { }
        //        }

        //        try {
        //            workChannel.Close();
        //        }
        //        catch {
        //        }
        //    }

        //    if (_subscriberPool != null) {
        //        _subscriberPool.Dispose();
        //    }

        //    //NOTE: CreateConnection() can throw BrokerUnreachableException
        //    //That's okay because the exception needs to propagate to Reconnect() or Start()
        //    var conn = connectionFactory.CreateConnection();

        //    if (connectionBroken != null) connectionBroken.Dispose();
        //    connectionBroken = new CancellationTokenSource();

        //    if (stopWaitingOnQueue != null) stopWaitingOnQueue.Dispose();
        //    stopWaitingOnQueue = CancellationTokenSource.CreateLinkedTokenSource(disposedCancellationSource.Token, connectionBroken.Token);

        //    var pool = new AmqpChannelPooler(conn);
        //    _subscriberPool = pool;

        //    //Use pool reference henceforth.

        //    //Create work channel and declare exchanges and queues
        //    workChannel = pool.GetModel(ChannelFlags.Consumer);

        //    //Redeclare exchanges and queues
        //    AmqpUtils.DeclareExchangeAndQueues(workChannel.Channel, messageMapper, messagingConfig, serviceName, exchangeDeclareSync, Id);

        //    //Listen on work queue
        //    workConsumer = new ConcurrentQueueingConsumer(workChannel.Channel, requestQueued);
        //    string workQueueName = AmqpUtils.GetWorkQueueName(messagingConfig, serviceName);

        //    workChannel.Channel.BasicQos(0, (ushort)Settings.PrefetchCount, false);
        //    workChannel.Channel.BasicConsume(workQueueName, Settings.AckBehavior == SubscriberAckBehavior.Automatic, workConsumer);

        //    //Listen on subscriber queue
        //    subscriberChannel = pool.GetModel(ChannelFlags.Consumer);
        //    subscriberConsumer = new ConcurrentQueueingConsumer(subscriberChannel.Channel, requestQueued);
        //    string subscriberWorkQueueName = AmqpUtils.GetSubscriberQueueName(serviceName, Id);

        //    subscriberChannel.Channel.BasicQos(0, (ushort)Settings.PrefetchCount, false);
        //    subscriberChannel.Channel.BasicConsume(subscriberWorkQueueName, Settings.AckBehavior == SubscriberAckBehavior.Automatic, subscriberConsumer);

        //    //Cancel connectionBroken on connection/consumer problems 
        //    pool.Connection.ConnectionShutdown += (s, e) => { connectionBroken.Cancel(); };
        //    workConsumer.ConsumerCancelled += (s, e) => { connectionBroken.Cancel(); };
        //    subscriberConsumer.ConsumerCancelled += (s, e) => { connectionBroken.Cancel(); };
        //}

        public MessageContext Dequeue() {
            throw new NotImplementedException();
        }

        public void Dispose() {
            throw new NotImplementedException();
        }

        public void SendResponse(MessageContext context, KafkaMessagePacket packet) {
            throw new NotImplementedException();
        }

        public void Start() {
            throw new NotImplementedException();
        }
    }
}
