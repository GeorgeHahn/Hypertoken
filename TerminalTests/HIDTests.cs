﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketParser;
using Terminal;
using Terminal.Interface;
using Terminal.Interface.Enums;
using Xunit;
using StopBits = Terminal.Interface.Enums.StopBits;

namespace TerminalTests
{
    public class HIDTests
    {
        [Fact]
        public void TestPortEnumeration()
        {
            HIDDataHandler port = new HIDDataHandler(new CurrentPacketParser(new RawPacketParser()), null);
            Assert.NotNull(port.Devices);
        }

        [Fact]
        public void TestPortOpenClose()
        {
            HIDDataHandler port = new HIDDataHandler(new CurrentPacketParser(new RawPacketParser()), null);

            Assert.Equal(port.PortState, PortState.Error);

            port.DeviceName = port.Devices[0];
            Assert.NotEmpty(port.DeviceName);
        }

        [Fact]
        public void TestFriendlyName()
        {
            HIDDataHandler port = new HIDDataHandler(new CurrentPacketParser(new RawPacketParser()), null);
            port.DeviceName = port.Devices[0];
            Assert.NotNull(port.FriendlyName);
            Assert.NotEmpty(port.FriendlyName);
        }

        [Fact]
        public void TestDeviceType()
        {
            HIDDataHandler port = new HIDDataHandler(new CurrentPacketParser(new RawPacketParser()), null);
            Assert.Equal(port.DeviceType, DeviceType.HID);
        }
    }
}
