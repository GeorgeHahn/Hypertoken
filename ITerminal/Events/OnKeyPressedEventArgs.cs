using System;

namespace Terminal_Interface.Events
{
	public class OnKeyPressedEventArgs : EventArgs
	{
		public OnKeyPressedEventArgs()
			: base()
		{ }

		public char KeyChar { get; set; }
	}
}