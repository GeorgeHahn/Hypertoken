using System;

namespace Terminal.Interface.Events
{
	public class SetLoggingPathEventArgs : EventArgs
	{
		public SetLoggingPathEventArgs()
			: base()
		{
			Path = null;
		}

		public SetLoggingPathEventArgs(string path)
			: base()
		{
			Path = path;
		}

		public string Path { get; private set; }
	}
}