using System;
using System.Windows.Forms;

//using circuitworks.utility.UnhandledException;
using Terminal_Interface;

namespace HyperToken_WinForms_GUI
{
	public class Initializer : IInitable
	{
		public void Init()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);

			//Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			//Application.ThreadException += Handlers.ThreadExceptionHandler;
			//AppDomain.CurrentDomain.UnhandledException += Handlers.UnhandledExceptionHandler;
		}
	}
}