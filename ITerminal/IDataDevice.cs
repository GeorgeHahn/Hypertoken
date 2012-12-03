﻿using System.Collections.Generic;
using System.ComponentModel;
using Terminal_Interface.Enums;

namespace Terminal_Interface
{
	public enum deviceType
	{
		SerialPort,
		HID,
	}

	public interface IDataDevice : IDataWriter, IDataReader, INotifyPropertyChanged
	{
		new void Close();

		new void Open();

		new bool IsOpen { get; }

		IEnumerable<string> ListAvailableDevices();

		string DeviceName { get; set; }

		string DeviceStatus { get; }

		deviceType DeviceType { get; }

		PortState PortState { get; set; }

		string StatusLabel { get; set; }

		string[] Devices { get; }

		string CurrentDevice { get; set; }

		void KeyPressed(char c);
	}

	public interface ISerialPort : IDataDevice
	{
		int Baud { get; set; }

		StopBits StopBits { get; set; }

		int DataBits { get; set; }

		FlowControl FlowControl { get; set; }

		Parity Parity { get; set; }
	}

	public interface IHIDDevice : IDataDevice
	{
		int ReportLength { get; set; }
	}

	public interface ILogger
	{
		string LoggingFilePath { get; set; }

		LoggingState LoggingState { get; set; }
	}

	public interface IEchoer
	{
		EchoState EchoState { get; set; }
	}

	public interface IFileSender
	{
		FileSendState FileSendState { get; set; }
	}
}