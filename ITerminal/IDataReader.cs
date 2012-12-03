using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminal_Interface.Events;

namespace Terminal_Interface
{
	public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);

	public interface IDataReader
	{
		event DataReceivedEventHandler DataReceived;

		void Close();

		void Open();

		bool IsOpen { get; }
	}
}