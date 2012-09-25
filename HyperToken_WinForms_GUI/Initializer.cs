using System;
using System.Threading;
using System.Windows.Forms;
using Bugsense.WPF;
using NLog;
using Terminal_Interface;

namespace HyperToken_WinForms_GUI
{
	public class Initializer : IInitable
	{
		public void Init()
		{
			logger.Trace("Initializing WinForms GUI");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);

			logger.Trace("Wiring exception handlers");
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += ApplicationOnThreadException;
		}

		private void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs threadExceptionEventArgs)
		{
			logger.FatalException("Generic handler", threadExceptionEventArgs.Exception);
			BugSense.SendException(threadExceptionEventArgs.Exception);
		}

		private static Logger logger = LogManager.GetCurrentClassLogger();
	}
}