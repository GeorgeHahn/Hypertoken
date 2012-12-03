using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminal_Interface.Enums;

namespace Terminal_Interface
{
	public interface IBackend
	{
		string[] GetSerialPorts();

		void KeyPressed(char c);

		void Shutdown();
	}

	public interface ISerialBackend
	{
		string COMPort { get; set; }

		int baud { get; set; }

		StopBits stopBits { get; set; }

		int dataBits { get; set; }

		FlowControl flowControl { get; set; }

		Parity parity { get; set; }

		string[] serialPorts { get; }
	}
}