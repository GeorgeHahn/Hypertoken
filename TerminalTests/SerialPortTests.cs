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

            port.DeviceName = "COM1";
            port.PortState = PortState.Open;
            Assert.Equal(port.PortState, PortState.Open);
            port.PortState = PortState.Closed;
            Assert.Equal(port.PortState, PortState.Closed);
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
