using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KafkaBus.Common
{
    public class MessageContext
    {
        public KafkaMessagePacket Request { get; set; }

        public string ReplyToQueue { get; set; }

        public string CorrelationId { get; set; }

        /// <summary>
        /// Broker specific message item.
        /// </summary>
        public object Dispatch { get; set; }

    }
}
