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
		private ITerminal _terminal;

		private static Logger logger = LogManager.GetCurrentClassLogger();

		public TerminalRunner(ITerminal terminal)
		{
			logger.Trace("TerminalRunner created");
			_terminal = terminal;
		}

		public void Run()
		{
			logger.Trace("Running terminal");
			_terminal.Run();
		}
	}
}