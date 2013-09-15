using System;

namespace Terminal.Interface.Events
{
	public class SendFileEventArgs : EventArgs
	{
		public SendFileEventArgs()
			: base()
		{ }

		public SendFileEventArgs(byte[] data, int lineDelay, int charDelay)
		{
			_charDelay = charDelay;
			_lineDelay = lineDelay;
			_data = data;
		}

		public SendFileEventArgs(byte[] data, int lineDelay)
			: this(data, lineDelay, 0)
		{ }

		public SendFileEventArgs(byte[] data)
			: this(data, 0, 0)
		{ }

		private int _charDelay;
		private byte[] _data;
		private int _lineDelay;

		public int CharDelay
		{
			get
			{
				return _charDelay;
			}
		}

		public int LineDelay
		{
			get
			{
				return _lineDelay;
			}
		}

		public byte[] Data
		{
			get
			{
				return _data;
			}
		}
	}
}