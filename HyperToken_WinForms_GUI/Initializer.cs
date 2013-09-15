using System;
using System.Threading;
using System.Windows.Forms;
using Anotar.NLog;
using Bugsense.WPF;
using NLog;
using Terminal.Interface;

namespace HyperToken.WinFormsGUI
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