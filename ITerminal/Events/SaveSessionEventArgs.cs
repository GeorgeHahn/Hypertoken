using System;

namespace Terminal_Interface.Events
{
	public class SaveSessionEventArgs : EventArgs
	{
		public SaveSessionEventArgs()
			: base()
		{ }

		public string FileName { get; set; }

		public string SessionData { get; set; }
	}
}