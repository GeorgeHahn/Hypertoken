using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketParser;
using Terminal;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Xunit;
using StopBits = Terminal_Interface.Enums.StopBits;

namespace TerminalTests
{
    public class HIDTests
    {
        [Fact]
        public void TestPortEnumeration()
        {
            HIDDataHandler port = new HIDDataHandler(new PacketParserHandler(new RawPacketParser()));
            Assert.NotNull(port.Devices);
        }

        [Fact]
        public void TestPortOpenClose()
        {
            HIDDataHandler port = new HIDDataHandler(new PacketParserHandler(new RawPacketParser()));

            Assert.Equal(port.PortState, PortState.Error);

            port.DeviceName = port.Devices[0];
            Assert.NotEmpty(port.DeviceName);
        }

        [Fact]
        public void TestFriendlyName()
        {
            HIDDataHandler port = new HIDDataHandler(new PacketParserHandler(new RawPacketParser()));
            port.DeviceName = port.Devices[0];
            Assert.NotNull(port.FriendlyName);
            Assert.NotEmpty(port.FriendlyName);
        }

        [Fact]
        public void TestDeviceType()
        {
            HIDDataHandler port = new HIDDataHandler(new PacketParserHandler(new RawPacketParser()));
            Assert.Equal(port.DeviceType, deviceType.HID);
        }
    }
}
