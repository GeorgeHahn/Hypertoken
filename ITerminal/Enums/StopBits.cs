using System.ComponentModel;

namespace Terminal.Interface.Enums
{
	public enum StopBits
	{
		[Description("0")]
		None,

		[Description("1")]
		One,

		[Description("1.5")]
		OnePointFive,

		[Description("2")]
		Two
	}
}