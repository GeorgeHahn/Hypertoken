using System.Windows.Forms;
using Anotar.NLog;
using Terminal.Interface;

namespace HyperToken.WinForms
{
	public class Initializer : IInitable
	{
		public void Init()
		{
			LogTo.Trace("Initializing WinForms GUI");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);

			LogTo.Trace("Wiring exception handlers");
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
		}
	}
}