using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using NLog;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;

namespace Terminal
{
    public class SerialPortDataHandler : ISerialPort
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly SerialPort _port;
        private readonly byte[] _receiveBuffer;

        public SerialPortDataHandler()
        {
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
            set { _port.PortName = value; }
        }

        public string[] Devices
        {
            get { return ListAvailableDevices().ToArray(); }
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
            return SerialPort.GetPortNames().AsEnumerable();
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

            //if (count > 200)
            //{
            //	logger.Warn("High latency detected");

            //	// TODO Add a feedback mechanism for reporting latency issues

            //	//_terminal.TrimLines(1000);
            //}

            if (count > 1024)
                count = 1024;

            _port.Read(_receiveBuffer, 0, count);

            //TODO Translate unprintable characters to hex
            string inStr = System.Text.Encoding.ASCII.GetString(_receiveBuffer, 0, count);

            inStr = inStr.Replace(new string(new char[] { (char)0xA, (char)0xD }), Environment.NewLine);

            if (DataReceived != null)
            {
                var args = new DataReceivedEventArgs(inStr);
                DataReceived(this, args);
            }
        }
    }
}