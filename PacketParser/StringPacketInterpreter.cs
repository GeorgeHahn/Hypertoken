using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal_Interface;

namespace PacketParser
{
    public class StringPacketInterpreter : IPacketInterpreter
    {
        public string InterpretPacket(byte[] packet)
        {
            var hex = new StringBuilder(packet.Length * 4);
            int endIndex = packet.Length - 1;
            while ((packet[endIndex] == 0) && (endIndex >= 0))
                endIndex--;

            if (endIndex < 0)
                return "Empty string\r\n";

            for (int index = 0; index <= endIndex; index++)
            {
                var b = packet[index];
                if ((b >= 0x21) && (b <= 0x7E))
                    hex.Append(Encoding.UTF8.GetChars(new []{ b }));
                else
                    hex.AppendFormat("[{0:X2}]", b);
            }
            return hex.ToString();
        }

        public string Name { get { return "Unprintable character parser"; }}
    }
}