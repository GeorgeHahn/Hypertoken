using System;

namespace Terminal_Interface.Events
{
	public class DataReceivedEventArgs : EventArgs
	{
		private string _data;

		public DataReceivedEventArgs(string data)
			: base()
		{
			_data = data;
		}

		public string Data
		{
			get
			{
				return _data;
			}
		}
	}
}