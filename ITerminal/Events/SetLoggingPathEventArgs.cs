using System;

namespace Terminal_Interface.Events
{
	public class SetLoggingPathEventArgs : EventArgs
	{
		public SetLoggingPathEventArgs()
			: base()
		{
			_path = null;
		}

		public SetLoggingPathEventArgs(string Path)
		{
			_path = Path;
		}

		private string _path;

		public string Path
		{
			get
			{
				return _path;
			}
		}
	}
}