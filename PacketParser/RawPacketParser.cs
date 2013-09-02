using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal_Interface;

namespace PacketParser
{
    public class RawPacketParser : IPacketInterpreter
    {
        public void Create()
        { }

        public void Release()
        { }

        public string InterpretPacket(byte[] packet)
        {
            return Encoding.UTF8.GetString(packet);
        }

        public string Name { get { return "Raw string parser"; }}
    }
}
