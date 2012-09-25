using System;

namespace Terminal_Interface.Events
{
	public class SerialPortListEventArgs : EventArgs
	{
		public SerialPortListEventArgs()
			: base()
		{
		}

		public string[] ports { get; set; }
	}
}