namespace KafkaBus.Common
{
    public abstract class MessagePacket
    {
        public byte[] Content;

        public abstract byte[] Serialize();
    }
}