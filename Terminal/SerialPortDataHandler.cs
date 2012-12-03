using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using NLog;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;
using Parity = System.IO.Ports.Parity;
using StopBits = System.IO.Ports.StopBits;

namespace Terminal
{
	public class SerialPortDataHandler : ISerialPort
	{
		private readonly SerialPort _port;
		private readonly byte[] _receiveBuffer;

		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		public event DataReceivedEventHandler DataReceived;

		public SerialPortDataHandler()
		{
			_port = new SerialPort();
			_port.DataReceived += PortOnDataReceived;

			_receiveBuffer = new byte[1024];

			// TODO Can this dependency be injected?
			Settings = new SettingsDictionary();

			Settings.SettingChanged += OnSettingChanged;

			_port.PortName = "COM1";
			_port.BaudRate = 115200;
			_port.DataBits = 8;
			_port.StopBits = System.IO.Ports.StopBits.One;
			_port.Parity = System.IO.Ports.Parity.None;
			_port.Handshake = Handshake.None;
		}

		private void OnSettingChanged(object sender, SettingChangedEventArgs settingChangedEventArgs)
		{
			object value = Settings.Get(settingChangedEventArgs.ChangedSetting);
			switch (settingChangedEventArgs.ChangedSetting)
			{
				case "port":
					_port.PortName = (string)value;
					break;

				case "Baud":
					_port.BaudRate = (int)value;
					break;

				case "stopbits":
					_port.StopBits = (StopBits)value;
					break;

				case "databits":
					_port.DataBits = (int)value;
					break;

				case "handshake":
					_port.Handshake = (Handshake)value;
					break;

				case "parity":
					_port.Parity = (Parity)value;
					break;
			}
		}

		public ISettings Settings { get; private set; }

		private void PortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
		{
			logger.Trace("Received {0} bytes", _port.BytesToRead);
			if (!_port.IsOpen)
				return;

			// Print the received data to the screen
			int count = _port.BytesToRead;

			if (count == 0)
				return;

			if (count > 200)
			{
				logger.Warn("High latency detected");

				// TODO Add a feedback mechanism for reporting latency issues

				//_terminal.TrimLines(1000);
			}

			if (count > 1024)
				count = 1024;

			_port.Read(_receiveBuffer, 0, count);

			//TODO Translate unprintable characters to hex
			string inStr = System.Text.Encoding.ASCII.GetString(_receiveBuffer, 0, count);

			//int inStrLen = inStr.Length;

			inStr = inStr.Replace(new string(new char[] { (char)0xA, (char)0xD }), Environment.NewLine);

			//if (inStrLen > 1)
			//{
			//	for (int i = 1; i < inStrLen; i++)
			//		if ((inStr[i - 1] == 0x0a) && (inStr[i] == 0x0d))
			//		{
			//			inStr = inStr.Remove(i, 1);
			//			inStrLen--;
			//		}

			//	lastChar = inStr[inStrLen - 1];
			//}
			//else
			//	if (inStrLen == 1)
			//		if ((lastChar == 0x0a) && (inStr[0] == 0x0d))
			//			return;

			if (DataReceived != null)
			{
				DataReceivedEventArgs args = new DataReceivedEventArgs(inStr);
				DataReceived(this, args);
			}

			//TODO implement logging in a IDataReader
			//if (loggingEnabled)
			//    if (loggingStream.CanWrite)
			//        loggingStream.Write(_receiveBuffer, 0, count);
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

		public IEnumerable<string> ListAvailableDevices()
		{
			return SerialPort.GetPortNames().AsEnumerable();
		}

		public bool IsOpen
		{
			get
			{
				return _port.IsOpen;
			}
		}

		public string DeviceName
		{
			get { return _port.PortName; }
			set { _port.PortName = value; }
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
		}

		public deviceType DeviceType { get; private set; }

		public PortState PortState { get; set; }

		public string StatusLabel { get; set; }

		public string[] Devices { get; private set; }

		public string CurrentDevice { get; set; }

		public void KeyPressed(char c)
		{
			_port.Write(new char[] { c }, 0, 1);
		}

		public void Close()
		{
			_port.Close();
		}

		public void Open()
		{
			_port.Open();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public int Baud { get; set; }

		public Terminal_Interface.Enums.StopBits StopBits { get; set; }

		public int DataBits { get; set; }

		public FlowControl FlowControl { get; set; }

		public Terminal_Interface.Enums.Parity Parity { get; set; }
	}
}