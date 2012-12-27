using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using HyperToken_WinForms_GUI;
using NLog;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;

namespace Terminal
{
    public class SerialPortDataHandler : ISerialPort
    {
        private readonly PacketParserHandler _handler;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly SerialPort _port;
        private readonly byte[] _receiveBuffer;

        public SerialPortDataHandler(PacketParserHandler handler)
        {
            _handler = handler;
            _port = new SerialPort();
            _port.DataReceived += PortOnDataReceived;

            _receiveBuffer = new byte[1024];

            DeviceName = "COM1";
            Baud = 115200;
            DataBits = 8;
            StopBits = Terminal_Interface.Enums.StopBits.One;
            Parity = Terminal_Interface.Enums.Parity.None;
            FlowControl = FlowControl.None;
        }

        public event DataReceivedEventHandler DataReceived;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Baud
        {
            get { return _port.BaudRate; }
            set { _port.BaudRate = value; }
        }

        public string CurrentDevice { get; set; }

        public int DataBits
        {
            get { return _port.DataBits; }
            set
            {
                _port.DataBits = value;
                DeviceStatus = "";
            }
        }

        public string DeviceName
        {
            get { return _port.PortName; }
            set
            {
                if(_port.IsOpen)
                    PortState = PortState.Closed;

                _port.PortName = value;
            }
        }

        public string[] Devices
        {
            get { return SerialPort.GetPortNames(); }
        }

        public string FriendlyName
        {
            get { return string.Format("{0}", _port.PortName); }
        }

        public string DeviceStatus
        {
            get
            {
                return string.Format("{0};{1};{2}",
                    _port.DataBits,
                    _port.Parity.ToString()[0],
                    (float)((int)_port.StopBits + 1) / 2);
            }
            set
            {
                logger.Trace("Invalidated DeviceStatus; {0}", value);
            }
        }

        public deviceType DeviceType
        {
            get
            {
                return deviceType.SerialPort;
            }
        }

        public FlowControl FlowControl
        {
            get { return (Terminal_Interface.Enums.FlowControl)_port.Handshake; }
            set { _port.Handshake = (System.IO.Ports.Handshake)value; }
        }

        public Terminal_Interface.Enums.Parity Parity
        {
            get { return (Terminal_Interface.Enums.Parity)_port.Parity; }
            set
            {
                _port.Parity = (System.IO.Ports.Parity)value;
                DeviceStatus = "";
            }
        }

        public PortState PortState
        {
            get { return _port.IsOpen ? PortState.Open : PortState.Closed; }
            set
            {
                logger.Trace("Port being set to {0}", value);
                if (value == PortState.Open)
                    try
                    {
                        _port.Open();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        logger.Error("{0} is in use", DeviceName);
                    }
                else
                    _port.Close();
            }
        }

        public Terminal_Interface.Enums.StopBits StopBits
        {
            get { return (Terminal_Interface.Enums.StopBits)_port.StopBits; }
            set
            {
                _port.StopBits = (System.IO.Ports.StopBits)value;
                DeviceStatus = "";
            }
        }

        public void KeyPressed(char c)
        {
            _port.Write(new char[] { c }, 0, 1);
        }

        public IEnumerable<string> ListAvailableDevices()
        {
            return Devices.AsEnumerable();
        }

        public int Write(byte[] data)
        {
            int length = data.Length;
            if (length > _port.WriteBufferSize)
                length = _port.WriteBufferSize;

            _port.Write(data, 0, length);
            return length;
        }

        public int Write(byte data)
        {
            _port.Write(new byte[] { data }, 0, 1);
            return 1;
        }

        public int Write(string data)
        {
            // TODO handle long string sending

            _port.WriteLine(data);
            return data.Length;
        }

        public int Write(char data)
        {
            _port.Write(new[] { data }, 0, 1);
            return 1;
        }

        private void PortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            logger.Trace("Received {0} bytes", _port.BytesToRead);
            if (!_port.IsOpen)
                return;

            var count = _port.BytesToRead;

            if (count == 0)
                return;
            
            if (count > 1024)
                count = 1024;

            _port.Read(_receiveBuffer, 0, count);

            if (DataReceived != null)
            {
                var actualData = new byte[count];
                Array.Copy(_receiveBuffer, actualData, count);
                string packet = _handler.CurrentParser.InterpretPacket(actualData);

                var args = new DataReceivedEventArgs(packet);
                DataReceived(this, args);
            }
        }
    }
}