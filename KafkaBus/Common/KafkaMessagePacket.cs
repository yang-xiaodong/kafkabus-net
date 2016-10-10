using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KafkaBus.Common
{
    public class KafkaMessagePacket : MessagePacket
    {
        public override byte[] Serialize() {
            throw new NotImplementedException();
        }

        public static KafkaMessagePacket Deserialize(byte[] data) {
            return new KafkaMessagePacket();
        }
    }
}
