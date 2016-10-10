using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KafkaBus.Common
{
    public abstract class MessagePacket
    {

        public byte[] Content;

        public abstract byte[] Serialize();
    }
}
