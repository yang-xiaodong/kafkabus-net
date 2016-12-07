using KafkaBus.Common;
using RdKafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaBus.Kafka.Subscription
{
    public class KafkaBusSubscriber : IKafkaBusSubscriber
    {
        private readonly InterlockedBoolean hasStarted;

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

        public MessageContext Dequeue() {
            throw new NotImplementedException();
        }

        public void Dispose() {
            
        }

        public void SendResponse(MessageContext context, KafkaMessagePacket packet) {
            throw new NotImplementedException();
        }

        public void Start() {

            var config = new Config() { GroupId = "simple-csharp-consumer" };
            using (var consumer = new EventConsumer(config, "")) {
                consumer.OnMessage += (obj, msg) => {
                    string text = Encoding.UTF8.GetString(msg.Payload, 0, msg.Payload.Length);
                    Console.WriteLine($"Topic: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {text}");
                };

                consumer.Assign(new List<TopicPartitionOffset> { new TopicPartitionOffset(topics.First(), 0, 5) });

                consumer.Start();

                Console.WriteLine("Started consumer, press enter to stop consuming");
                Console.ReadLine();
            }


            Console.WriteLine("Starting...");
        }
    }
}