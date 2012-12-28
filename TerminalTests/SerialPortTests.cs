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
    public class SerialPortTests
    {
        [Fact]
        public void TestPortEnumeration()
        {
            var availablePorts = SerialPort.GetPortNames();

            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));
            foreach (var device in port.Devices)
            {
                Assert.Contains(device, availablePorts);
            }
            Assert.Equal(availablePorts.Length, port.Devices.Length);
        }

        [Fact]
        public void TestPortOpenClose()
        {
            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));

            Assert.Equal(port.PortState, PortState.Closed);

            port.DeviceName = port.Devices[0];
            port.PortState = PortState.Open;
            Assert.Equal(port.PortState, PortState.Open);
            port.PortState = PortState.Closed;
            Assert.Equal(port.PortState, PortState.Closed);
        }

        [Fact]
        public void TestEnumeration()
        {
            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));
            Assert.Equal(port.Devices, port.ListAvailableDevices().ToArray());
        }

        [Fact]
        public void TestNonExistentNames()
        {
            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));

            port.DeviceName = "COMx";
            bool threw = false;
            try
            {
                port.PortState = PortState.Open;
            }
            catch (System.IO.IOException)
            {
                threw = true;
            }
            Assert.True(threw);
        }

        [Fact]
        public void TestInvalidNames()
        {
            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));

            port.DeviceName = "NOGOOD";
            bool threw = false;
            try
            {
                port.PortState = PortState.Open;
            }
            catch (ArgumentException)
            {
                threw = true;
            }
            Assert.True(threw);
        }

        [Fact]
        public void TestDataBits()
        {
            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));
            port.DataBits = 8;
            Assert.Equal(port.DataBits, 8);
            port.DataBits = 7;
            Assert.Equal(port.DataBits, 7);
        }

        [Fact]
        public void TestStopBits()
        {
            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));
            port.StopBits = StopBits.One;
            Assert.Equal(port.StopBits, StopBits.One);
        }

        [Fact]
        public void TestParity()
        {
            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));
            port.Parity = Terminal_Interface.Enums.Parity.None;
            Assert.Equal(port.Parity, Terminal_Interface.Enums.Parity.None);
            port.Parity = Terminal_Interface.Enums.Parity.Even;
            Assert.Equal(port.Parity, Terminal_Interface.Enums.Parity.Even);
            port.Parity = Terminal_Interface.Enums.Parity.Odd;
            Assert.Equal(port.Parity, Terminal_Interface.Enums.Parity.Odd);
            port.Parity = Terminal_Interface.Enums.Parity.Mark;
            Assert.Equal(port.Parity, Terminal_Interface.Enums.Parity.Mark);
            port.Parity = Terminal_Interface.Enums.Parity.Space;
            Assert.Equal(port.Parity, Terminal_Interface.Enums.Parity.Space);
        }

        [Fact]
        public void TestFlowControl()
        {
            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));
            port.FlowControl = FlowControl.None;
            Assert.Equal(port.FlowControl, FlowControl.None);
            port.FlowControl = FlowControl.RequestToSend;
            Assert.Equal(port.FlowControl, FlowControl.RequestToSend);
        }

        [Fact]
        public void TestDeviceStatus()
        {
            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));
            port.DataBits = 8;
            port.Parity = Terminal_Interface.Enums.Parity.None;
            port.StopBits = StopBits.One;
            Assert.Equal(port.DeviceStatus, "8;N;1");
        }

        [Fact]
        public void TestFriendlyName()
        {
            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));
            port.DeviceName = "COM1";
            Assert.NotNull(port.FriendlyName);
            Assert.NotEmpty(port.FriendlyName);
        }

        [Fact]
        public void TestDeviceType()
        {
            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));
            Assert.Equal(port.DeviceType, deviceType.SerialPort);
        }

        [Fact]
        public void TestPortWriteData()
        {
            SerialPortDataHandler port = new SerialPortDataHandler(new PacketParserHandler(new RawPacketParser()));
            port.DeviceName = "COM1";
            port.PortState = PortState.Open;
            Assert.Equal(port.PortState, PortState.Open);

            Assert.Equal(port.Write((byte) 0), 1);
            Assert.Equal(port.Write('T'), 1);
            Assert.Equal(port.Write("est"), 3);
            Assert.Equal(port.Write(new byte[] {0, 1, 2, 3}), 4);
            port.PortState = PortState.Closed;
        }
    }
}
