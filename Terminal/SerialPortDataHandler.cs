﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using NLog;
using Terminal_Interface;
using Terminal_Interface.Events;

namespace Terminal
{
	public class SerialPortDataHandler : IDataReader, IDataWriter
	{
		private SerialPort _port;
		private byte[] _receiveBuffer;

		private static Logger logger = LogManager.GetCurrentClassLogger();

		public event DataReceivedEventHandler DataReceived;

		private SerialPortDataHandler()
		{
			_port = new SerialPort();
			_port.DataReceived += PortOnDataReceived;

			_receiveBuffer = new byte[1024];
		}

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

		public int Write(string data)
		{
			// TODO handle long string sending

			_port.WriteLine(data);
		}
	}
}