using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			Log.Debug("TerminalRunner created");
			_dataDevice = dataDevice;
			_terminal = terminal;
		}

		public void Run()
		{
			Log.Debug("Running terminal");
			_terminal.Run();

			Log.Info("Application shutting down");
		}
	}
}