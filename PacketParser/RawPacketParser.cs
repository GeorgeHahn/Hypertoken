using System;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using Terminal.Interface;

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

        public string InterpretPacket(byte[] packet, byte[] unparsedPacket)
        {
            return InterpretPacket(packet);
        }

        public string Name { get { return "Raw string parser"; } }
    }
}

namespace PacketParser
{
    public class RawHIDParser : IPacketInterpreter
    {
        public void Create()
        { }

        public void Release()
        { }

        public string InterpretPacket(byte[] packet)
        {
            return '{' + string.Join(",", packet) + "}" + Environment.NewLine;
        }

        public string InterpretPacket(byte[] packet, byte[] unparsedPacket)
        {
            return InterpretPacket(unparsedPacket);
        }

        public string Name { get { return "Raw HID data"; }}
    }
}
