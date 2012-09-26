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
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
		}

		private static Logger logger = LogManager.GetCurrentClassLogger();
	}
}