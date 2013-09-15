using System.Runtime.InteropServices;

namespace HyperToken.WinFormsGUI.Helpers
{
	public class HighResolutionSleep
	{
		/// <summary>
		/// Set system timer resolution
		/// </summary>
		/// <param name="uMilliseconds">Desired resolution</param>
		/// <returns></returns>
		[DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
		public static extern uint MM_BeginPeriod(uint uMilliseconds);

		/// <summary>
		/// Unset system timer resolution
		/// </summary>
		/// <param name="uMilliseconds">Previously desired resolution</param>
		/// <returns></returns>
		[DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
		public static extern uint MM_EndPeriod(uint uMilliseconds);
	}
}