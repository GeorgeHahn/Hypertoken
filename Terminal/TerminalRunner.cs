using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Terminal_Interface;

namespace Terminal
{
	internal class TerminalRunner
	{
		private readonly ITerminal _terminal;
		private readonly IBackend _backend;

		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		public TerminalRunner(ITerminal terminal, IBackend backend)
		{
			logger.Trace("TerminalRunner created");
			_backend = backend;
			_terminal = terminal;
		}

		public void Run()
		{
			logger.Trace("Running terminal");
			_terminal.SetBackend(_backend);
			_terminal.Run();

			logger.Info("Application shutting down");

			_backend.Shutdown();
		}
	}
}