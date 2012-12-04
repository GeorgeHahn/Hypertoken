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
		private readonly ISerialPort _dataDevice;

		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		public TerminalRunner(ITerminal terminal, ISerialPort dataDevice)
		{
			logger.Trace("TerminalRunner created");
			_dataDevice = dataDevice;
			_terminal = terminal;
		}

		public void Run()
		{
			logger.Trace("Running terminal");
			_terminal.Run();

			logger.Info("Application shutting down");
		}
	}
}