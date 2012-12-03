using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Terminal_Interface.Enums;

namespace Terminal_Interface
{
	public interface IBackend : INotifyPropertyChanged
	{
		void KeyPressed(char c);

		void Shutdown();

		// New API
		string Title { get; set; }

		string LoggingFilePath { get; set; }

		LoggingState loggingState { get; set; }

		EchoState echoState { get; set; }

		PortState portState { get; set; }

		FileSendState fileSendState { get; set; }
	}

	public interface ISerialBackend : IBackend
	{
		string COMPort { get; set; }

		int baud { get; set; }

		StopBits stopBits { get; set; }

		int dataBits { get; set; }

		FlowControl flowControl { get; set; }

		Parity parity { get; set; }

		string[] serialPorts { get; }

		string StatusLabel { get; set; }
	}
}