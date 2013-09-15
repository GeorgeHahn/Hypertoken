using System;
using Terminal.Interface;

namespace PacketParser
{
    public class HIDPreparser: IHIDPreparser
    {
        public byte[] InterpretPacket(byte[] packet)
        {
            if (packet.Length < 2)
                return new byte[0];

            int packetLength = packet[3];
            var data = new byte[packetLength];
            Array.Copy(packet, 4, data, 0, packetLength);
            return data;
        }

        public string Name { get { return "HID Preparser"; }}
    }
}