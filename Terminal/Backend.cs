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
	public class Backend : IBackend
	{
		private IDataReader _reader;

		public static string GetVersion()
		{
			return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public Backend(ITerminal terminal, IDataReader reader)
		{
			if (terminal == null)
				throw new ArgumentNullException("terminal");
			if (reader == null)
				throw new ArgumentNullException("reader");

			_reader = reader;
			_terminal = terminal;

			reader.DataReceived += OnDataReceived;

			ShowVersionInformation();

			#region _terminal initialization

			terminal.SetBackend(this);

			_terminal.PropertyChanged += TerminalOnPropertyChanged;

			//TODO Remove old API calls
			_terminal.OnKeyPressed += TerminalOnKeyPressed;
			_terminal.OnSaveSession += TerminalOnSaveSession;
			_terminal.OnSendFile += TerminalOnSendFile;
			_terminal.OnSetLoggingPath += TerminalOnSetLoggingPath;

			_terminal.baud = 115200;
			_terminal.dataBits = 8;
			_terminal.stopBits = Terminal_Interface.Enums.StopBits.One;
			_terminal.parity = Parity.None;
			_terminal.flowControl = FlowControl.None;

			#endregion _terminal initialization
		}

		private void ShowVersionInformation()
		{
			logger.Warn("Version {0}", GetVersion());

			_terminal.Title = "HyperToken";

#if DEBUG
			_terminal.Title += " [Debug]";

			logger.Warn("Debug version");
#endif

			_terminal.Title += " (" + GetVersion() + ')';

			if (System.Diagnostics.Debugger.IsAttached)
			{
				logger.Warn("Debugger attached");
				_terminal.Title += " [Debugger attached]";
			}
		}

		private void TerminalOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			logger.Warn("VersionShim setting {0}", propertyChangedEventArgs.PropertyName);

			switch (propertyChangedEventArgs.PropertyName)
			{
				case "Title":
					break;

				case "LoggingFilePath":
					TerminalOnSetLoggingPath(null, new SetLoggingPathEventArgs(_terminal.LoggingFilePath));
					break;

				case "loggingState":
					SetLoggingState(_terminal.loggingState);
					break;

				case "echoState":
					SetEchoState(_terminal.echoState);
					break;

				case "portState":
					SetPortConnection(_terminal.portState);
					break;

				case "fileSendState":
					break;

				case "COMPort":
					SetCOMPort(_terminal.COMPort);
					break;

				case "baud":
					SetBaudRate(_terminal.baud);
					break;

				case "stopBits":
					SetStopBits(_terminal.stopBits);
					break;

				case "dataBits":
					SetDataBits(_terminal.dataBits);
					break;

				case "flowControl":
					SetFlowControl(_terminal.flowControl);
					break;

				case "parity":
					SetParity(_terminal.parity);
					break;

				case "serialPorts":
					break;

				case "connected":

					break;
			}
		}

		private void SetDataBits(int p)
		{
			logger.Trace("Databits set to {0}", p);
			serialPort.DataBits = p;
			_terminal.dataBits = p;
		}

		private void SetStopBits(Terminal_Interface.Enums.StopBits stopBits)
		{
			logger.Trace("Stopbits set to {0}", stopBits);
			serialPort.StopBits = (System.IO.Ports.StopBits)stopBits;
			_terminal.stopBits = stopBits;
		}

		private void SetFlowControl(Terminal_Interface.Enums.FlowControl flowControl)
		{
			logger.Trace("FlowControl set to {0}", flowControl);
			serialPort.Handshake = (System.IO.Ports.Handshake)flowControl;
			_terminal.flowControl = flowControl;
		}

		private void SetBaudRate(int baudrate)
		{
			logger.Trace("Baud set to {0}", baudrate);
			serialPort.BaudRate = baudrate;
			_terminal.baud = baudrate;
		}

		private void SetParity(Terminal_Interface.Enums.Parity parity)
		{
			logger.Trace("Parity being set to {0}", parity);
			serialPort.Parity = (System.IO.Ports.Parity)parity;
			_terminal.parity = parity;
		}

		private void TerminalOnSaveSession(object sender, SaveSessionEventArgs e)
		{
			logger.Trace("Session save clicked");
		}

		private void SendChar(char c)
		{
			if (!serialPort.IsOpen)
			{
				logger.Warn("Serial port not connected");
#if DEBUG
				logger.Warn("Opening.");
				serialPort.BaudRate = 115200;
				SetStopBits(Terminal_Interface.Enums.StopBits.One);
				serialPort.Parity = System.IO.Ports.Parity.None;
				serialPort.Handshake = Handshake.None;
				serialPort.Open();
#endif
				return;
			}

			serialPort.Write(new[] { c }, 0, 1);

			if (echoState == EchoState.Enabled)
				_terminal.AddChar(c);
		}

		private void TerminalOnKeyPressed(object sender, OnKeyPressedEventArgs e)
		{
			logger.Trace("Key pressed: {0}", e.KeyChar);
			SendChar(e.KeyChar);
		}

		internal static void Shutdown()
		{
			logger.Info("Logger shutting down");
		}

		private void SetCOMPort(string COMPort)
		{
			logger.Info("Setting COM port to {0}", COMPort);
			if (serialPort.IsOpen)
				SetPortConnection(PortState.Closed);
			serialPort.PortName = COMPort;
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

				_terminal.loggingState = loggingState;

				loggingStream.Close();
				logger.Info("Disabled logging");
				return;
			}

			if (state != LoggingState.Enabled) return;

			logger.Info("Enabling logging");

			try
			{
				logger.Trace("Setting logging file");

				if (string.IsNullOrEmpty(loggingFile))
					loggingFile = _terminal.LoggingFilePath;

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

			_terminal.loggingState = loggingState;

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

		private void SetEchoState(EchoState state)
		{
			logger.Trace("Setting echoState to {0}", state);
			echoState = state;
			_terminal.echoState = state;
		}

		private void Uninitialize()
		{
			logger.Warn("Uninitializing");
			if (serialPort.IsOpen)
				serialPort.Close();

			logger.Info("Saving settings");

			logger.Info("Closing logging stream");
			if (loggingStream != null)
				loggingStream.Close();

			logger.Info("Uninitialization complete");
		}

		#region Serial port functions

		private PortState GetPortState()
		{
			if (serialPort.IsOpen)
				return PortState.Open;
			return PortState.Closed;
		}

		private void SetPortConnection(PortState state)
		{
			if (state == GetPortState())
				return;

			try
			{
				if (state == PortState.Open)
				{
					logger.Info("Opening port");
					serialPort.Open();
				}
				else if (state == PortState.Closed)
				{
					logger.Info("Closing port");
					serialPort.Close();
				}

				_terminal.portState = state;
			}
			catch (Exception e)
			{
				logger.ErrorException("SetPortConnection failed", e);
				_terminal.portState = PortState.Error;

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

			if (!serialPort.IsOpen)
			{
				logger.Warn("Serial port is closed");
				_terminal.portState = PortState.Error;

				return;
			}

			try
			{
				_terminal.fileSendState = FileSendState.InProgress;

				//TODO Remove old API call
				_terminal.SetFileSendState(FileSendState.InProgress);

				logger.Trace("Entering serial data send loop");
				HighResolutionSleep.MM_BeginPeriod(1);

				for (int i = 0; i < argsData.Data.Length; i++)
				{
					serialPort.Write(argsData.Data, i, 1);
					if (argsData.Data[i] == 13)
						if (argsData.LineDelay > 0)
							System.Threading.Thread.Sleep(argsData.LineDelay);

					System.Threading.Thread.Sleep(argsData.CharDelay);
				}

				HighResolutionSleep.MM_EndPeriod(1);
				logger.Trace("Exiting serial data send loop");
				_terminal.fileSendState = FileSendState.Success;

				//TODO Remove old API call
				_terminal.SetFileSendState(FileSendState.Success);
			}
			catch (Exception e)
			{
				logger.ErrorException("Error in file send thread", e);
				_terminal.fileSendState = FileSendState.Error;

				//TODO Remove old API call
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

			if (serialPort == null)
			{
				logger.Info("TerminalOnSendFile hit a null serial port");
				_terminal.portState = PortState.Error;
				return;
			}

			if (!serialPort.IsOpen)
			{
				logger.Debug("TerminalOnSendFile hit a closed serial port");
				_terminal.portState = PortState.Error;
				_terminal.fileSendState = FileSendState.Error;

				//TODO Remove old API calls
				_terminal.SetFileSendState(FileSendState.Error);
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

		private ITerminal _terminal;
		private static Logger logger = LogManager.GetCurrentClassLogger();

		private LoggingState loggingState = LoggingState.Disabled;

		private EchoState echoState = EchoState.Disabled;

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

		private char lastChar;

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
	}
}