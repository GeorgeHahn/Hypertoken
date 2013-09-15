using System.Text;
using Terminal.Interface;

namespace PacketParser
{
    public class StringPacketInterpreter : IPacketInterpreter
    {
        public void Create()
        { }

        public void Release()
        { }

        public string InterpretPacket(byte[] packet)
        {
            var hex = new StringBuilder(packet.Length * 4);
            int endIndex = packet.Length - 1;
            while ((endIndex >= 0) && (packet[endIndex] == 0))
                endIndex--;

            if (endIndex < 0)
                return string.Empty;

            for (int index = 0; index <= endIndex; index++)
            {
                var b = packet[index];
                if ((b >= 0x20) && (b <= 0x7E))
                    hex.Append(Encoding.UTF8.GetChars(new []{ b }));
                else
                    hex.AppendFormat("[{0:X2}]", b);
            }
            return hex.ToString();
        }

        public string Name { get { return "Unprintable character parser"; }}
    }
}