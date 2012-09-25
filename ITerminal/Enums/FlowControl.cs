using System.ComponentModel;

namespace Terminal_Interface.Enums
{
	public enum FlowControl
	{
		[Description("None")]
		None,

		[Description("Request To Send")]
		RequestToSend,

		[Description("Xon/Xoff")]
		XOnXOff,

		[Description("RTS + Xon/Xoff")]
		RequestToSendXOnXOff
	}
}