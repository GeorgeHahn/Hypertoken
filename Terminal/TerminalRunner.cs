using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anotar.NLog;
using NLog;
using Terminal_Interface;
using Anotar;

namespace Terminal
{
	internal class TerminalRunner
	{
		private readonly ITerminal _terminal;
		private readonly ISerialPort _dataDevice;

		public TerminalRunner(ITerminal terminal, ISerialPort dataDevice)
		{
			LogTo.Debug("TerminalRunner created");
			_dataDevice = dataDevice;
			_terminal = terminal;
		}

		public void Run()
		{
			LogTo.Debug("Running terminal");
			_terminal.Run();

			LogTo.Info("Application shutting down");
		}
	}
}