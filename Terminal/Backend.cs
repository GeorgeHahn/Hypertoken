using System;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Threading;
using Bugsense.WPF;
using HyperToken_WinForms_GUI.Helpers;
using NLog;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;
using Terminal_Interface.Exceptions;
using Parity = Terminal_Interface.Enums.Parity;
using StopBits = Terminal_Interface.Enums.StopBits;

namespace Terminal
{
	public class Backend : ISerialBackend
	{
		private IDataDevice _comms;

		public static string GetVersion()
		{
			return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public Backend(ITerminal terminal, IDataDevice serialPort)
		{
			if (terminal == null)
				throw new ArgumentNullException("terminal");
			if (serialPort == null)
				throw new ArgumentNullException("reader");

			_comms = serialPort;
			_terminal = terminal;

			_comms.DataReceived += OnDataReceived;

			ShowVersionInformation();

			#region _terminal initialization

			_terminal.SetBackend(this);

			//TODO Remove old API calls
			_terminal.OnKeyPressed += TerminalOnKeyPressed;
			_terminal.OnSaveSession += TerminalOnSaveSession;
			_terminal.OnSendFile += TerminalOnSendFile;
			_terminal.OnSetLoggingPath += TerminalOnSetLoggingPath;

			baud = 115200;
			dataBits = 8;
			stopBits = Terminal_Interface.Enums.StopBits.One;
			parity = Parity.None;
			flowControl = FlowControl.None;

			#endregion _terminal initialization
		}

		private void ShowVersionInformation()
		{
			logger.Warn("Version {0}", GetVersion());

			Title = "HyperToken";

#if DEBUG
			Title += " [Debug]";

			logger.Warn("Debug version");
#endif

			Title += " (" + GetVersion() + ')';

			if (System.Diagnostics.Debugger.IsAttached)
			{
				logger.Warn("Debugger attached");
				Title += " [Debugger attached]";
			}
		}

		private void TerminalOnSaveSession(object sender, SaveSessionEventArgs e)
		{
			logger.Trace("Session save clicked");
		}

		private void SendChar(char c)
		{
			_comms.Write(c);

			// TODO implement write echoing as a IDataWriter
			if (echoState == EchoState.Enabled)
				_terminal.AddChar(c);
		}

		private void TerminalOnKeyPressed(object sender, OnKeyPressedEventArgs e)
		{
			logger.Trace("Key pressed: {0}", e.KeyChar);
			SendChar(e.KeyChar);
		}

		public void Shutdown()
		{
			logger.Info("Logger shutting down");
		}

		public string Title { get; set; }

		public string LoggingFilePath { get; set; }

		public LoggingState loggingState { get; set; }

		public EchoState echoState { get; set; }

		public FileSendState fileSendState { get; set; }

		public string COMPort
		{
			get
			{
				return "PORT 5";

				// return comms portname?
			}
			set
			{
				//comms portName = value;
			}
		}

		#region Logging

		private void ToggleLogging()
		{
			if (loggingState == LoggingState.Enabled)
				SetLoggingState(LoggingState.Disabled);
			else
				SetLoggingState(LoggingState.Enabled);
		}

		/// <summary>
		/// Toggles logging on or off
		/// </summary>
		private void SetLoggingState(LoggingState state)
		{
			//Toggle debug logging
			if (state == LoggingState.Disabled)
			{
				if (loggingStream == null)
				{
					logger.Debug("Logging stream is null"); // Probably has not been initialized
					return;
				}
				logger.Info("Disabling logging");
				loggingState = LoggingState.Disabled;

				loggingStream.Close();
				logger.Info("Disabled logging");
				return;
			}

			if (state != LoggingState.Enabled) return;

			logger.Info("Enabling logging");

			try
			{
				logger.Trace("Setting logging file");

				//if (string.IsNullOrEmpty(loggingFile))
				//	loggingFile = _terminal.LoggingFilePath;

				//TODO Remove old API call
				if (loggingFile == null)
					loggingFile = _terminal.GetLoggingFilePath();

				if (loggingFile == null)
					throw new Exception("Logging file not selected");

				if (loggingFile.Equals(""))
					throw new Exception("Logging file not selected");

				loggingStream = File.Open(loggingFile, FileMode.Append, FileAccess.Write, FileShare.Read);

				if (loggingStream == null)
					throw new IOException("Logging file could not be opened");

				if (!loggingStream.CanWrite) // Will this ever happen?
					throw new IOException("Logging file cannot be written to");

				loggingState = LoggingState.Enabled;
			}
			catch (FileSelectionCanceledException e)
			{
				logger.ErrorException("FileSelectionCanceledException encountered in ToggleLogging", e);
			}
			catch (IOException e)
			{
				logger.ErrorException("IOException encountered in ToggleLogging", e);
				_terminal.AddLine(e.Message);
			}
			catch (Exception e)
			{
				logger.ErrorException("Generic exception encountered in ToggleLogging", e);
				_terminal.AddLine(e.Message);
			}

			logger.Info("Logging to file {0}", loggingFile);
		}

		private void TerminalOnSetLoggingPath(object sender, SetLoggingPathEventArgs e)
		{
			if (e.Path != null)
				NewLoggingFile(e.Path);
		}

		internal void NewLoggingFile(string path)
		{
			SetLoggingState(LoggingState.Disabled);
			loggingFile = path;
			SetLoggingState(LoggingState.Enabled);
		}

		#endregion Logging

		#region Serial port functions

		public PortState portState
		{
			get
			{
				if (_comms.IsOpen)
					return PortState.Open;
				return PortState.Closed;
			}
			set
			{
				if (value == PortState.Open)
					_comms.Open();
				else
					_comms.Close();
			}
		}

		private void SetPortConnection(PortState state)
		{
			if (portState == state)
				return;

			try
			{
				if (state == PortState.Open)
				{
					logger.Info("Opening port");
					_comms.Open();
				}
				else if (state == PortState.Closed)
				{
					logger.Info("Closing port");
					_comms.Close();
				}

				portState = state;
			}
			catch (Exception e)
			{
				logger.ErrorException("SetPortConnection failed", e);
				portState = PortState.Error;

				_terminal.AddLine(e.Message);

				BugSense.SendException(e);
			}
		}

		private void SerialFileSendThreadProc(object data)
		{
			logger.Debug("Inside file send thread");
			var argsData = (SendFileEventArgs)data;

			if (argsData.Data == null)
			{
				logger.Debug("SerialSendArgs.data is null");
				return;
			}

			if (!_comms.IsOpen)
			{
				logger.Warn("Serial port is closed");
				portState = PortState.Error;

				return;
			}

			try
			{
				fileSendState = FileSendState.InProgress;

				//TODO Remove old API call
				_terminal.SetFileSendState(FileSendState.InProgress);

				logger.Trace("Entering serial data send loop");
				HighResolutionSleep.MM_BeginPeriod(1);

				foreach (byte b in argsData.Data)
				{
					_comms.Write(b);
					if (b == 13)
						if (argsData.LineDelay > 0)
							Thread.Sleep(argsData.LineDelay);

					Thread.Sleep(argsData.CharDelay);
				}

				HighResolutionSleep.MM_EndPeriod(1);
				logger.Trace("Exiting serial data send loop");
				fileSendState = FileSendState.Success;
			}
			catch (Exception e)
			{
				logger.ErrorException("Error in file send thread", e);
				fileSendState = FileSendState.Error;

				_terminal.SetFileSendState(FileSendState.Error);
				_terminal.AddLine(e.Message);
			}
		}

		private void TerminalOnSendFile(object sender, SendFileEventArgs e)
		{
			if (e.Data == null)
			{
				logger.Info("TerminalOnSendFile got a null SendFileEventArgs");
				return;
			}

			if (!_comms.IsOpen)
			{
				logger.Debug("TerminalOnSendFile hit a closed serial port");
				portState = PortState.Error;
				fileSendState = FileSendState.Error;
			}

			if ((serialFileSendThread == null) || (!serialFileSendThread.IsAlive))
			{
				logger.Trace("Starting serial file send thread");
				serialFileSendThread = new Thread(new ParameterizedThreadStart(SerialFileSendThreadProc));
				serialFileSendThread.Start(e);
			}
			else
			{
				logger.Warn("Already sending a file; cancelling user request for another file send");
				_terminal.AddLine("Already sending a file, please wait until send is complete");
				return;
			}
		}

		private void OnDataReceived(object sender, DataReceivedEventArgs e)
		{
			_terminal.AddLine(e.Data);
		}

		#endregion Serial port functions

		private readonly ITerminal _terminal;
		private static Logger logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Path to file where incoming data is to be written
		/// </summary>
		private String loggingFile;

		/// <summary>
		/// File stream used to write incoming data to logging file
		/// </summary>
		private FileStream loggingStream;

		/// <summary>
		/// Thread that handles file sending
		/// </summary>
		private Thread serialFileSendThread;

		#region Implementation of IBackend

		public string[] GetSerialPorts()
		{
			logger.Trace("Serial ports listed");
			string[] ports = SerialPort.GetPortNames();
			logger.Debug("{0} ports available", ports.Length);
			return ports;
		}

		public void KeyPressed(char c)
		{
			SendChar(c);
		}

		#endregion Implementation of IBackend

		public event PropertyChangedEventHandler PropertyChanged;

		public int baud { get; set; }

		public StopBits stopBits { get; set; }

		public int dataBits { get; set; }

		public FlowControl flowControl { get; set; }

		public Parity parity { get; set; }

		public string[] serialPorts { get; private set; }
	}
}