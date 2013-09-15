using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminal.Interface.Events;

namespace Terminal.Interface
{
	public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);

	public interface IDataReader
	{
		event DataReceivedEventHandler DataReceived;
	}
}