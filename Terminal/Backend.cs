using System;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Threading;
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
		public static string GetVersion()
		{
			return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public Backend(ITerminal _terminal)
		{
			#region Serial port initialization

			recieveBuffer = new byte[1024];

			serialPort = new SerialPort();
			serialPort.DataReceived += SerialPortOnDataReceived;

			#endregion Serial port initialization

			#region terminal initialization

			this.terminal = _terminal;

			logger.Warn("Version {0}", GetVersion());

			this.terminal.Title = "HyperToken";

#if DEBUG
			this.terminal.Title += " [Debug]";

			logger.Warn("Debug version");
#endif

			this.terminal.Title += " (" + GetVersion() + ')';

			if (System.Diagnostics.Debugger.IsAttached)
			{
				logger.Warn("Debugger attached");
				this.terminal.Title += " [Debugger attached]";
			}

			this.terminal.PropertyChanged += TerminalOnPropertyChanged;

			//TODO Remove old API calls
			this.terminal.OnKeyPressed += TerminalOnKeyPressed;
			this.terminal.OnSaveSession += TerminalOnSaveSession;
			this.terminal.OnSendFile += TerminalOnSendFile;
			this.terminal.OnSerialPortList += TerminalOnSerialPortList;
			this.terminal.OnSetLoggingPath += TerminalOnSetLoggingPath;
			this.terminal.OnToggleConnection += TerminalOnToggleConnection;

			this.terminal.baud = 115200;
			this.terminal.dataBits = 8;
			this.terminal.stopBits = Terminal_Interface.Enums.StopBits.One;
			this.terminal.parity = Parity.None;
			this.terminal.flowControl = FlowControl.None;

			#endregion terminal initialization
		}

		private void TerminalOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			logger.Warn("VersionShim setting {0}", propertyChangedEventArgs.PropertyName);

			switch (propertyChangedEventArgs.PropertyName)
			{
				case "Title":
					break;

				case "LoggingFilePath":
					TerminalOnSetLoggingPath(null, new SetLoggingPathEventArgs(terminal.LoggingFilePath));
					break;

				case "loggingState":
					SetLoggingState(terminal.loggingState);
					break;

				case "echoState":
					SetEchoState(terminal.echoState);
					break;

				case "portState":
					SetPortConnection(terminal.portState);
					break;

				case "fileSendState":
					break;

				case "COMPort":
					SetCOMPort(terminal.COMPort);
					break;

				case "baud":
					SetBaudRate(terminal.baud);
					break;

				case "stopBits":
					SetStopBits(terminal.stopBits);
					break;

				case "dataBits":
					SetDataBits(terminal.dataBits);
					break;

				case "flowControl":
					SetFlowControl(terminal.flowControl);
					break;

				case "parity":
					SetParity(terminal.parity);
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
			terminal.dataBits = p;
		}

		private void SetStopBits(Terminal_Interface.Enums.StopBits stopBits)
		{
			logger.Trace("Stopbits set to {0}", stopBits);
			serialPort.StopBits = (System.IO.Ports.StopBits)stopBits;
			terminal.stopBits = stopBits;
		}

		private void SetFlowControl(Terminal_Interface.Enums.FlowControl flowControl)
		{
			logger.Trace("FlowControl set to {0}", flowControl);
			serialPort.Handshake = (System.IO.Ports.Handshake)flowControl;
			terminal.flowControl = flowControl;
		}

		private void SetBaudRate(int baudrate)
		{
			logger.Trace("Baud set to {0}", baudrate);
			serialPort.BaudRate = baudrate;
			terminal.baud = baudrate;
		}

		private void SetParity(Terminal_Interface.Enums.Parity parity)
		{
			logger.Trace("Parity being set to {0}", parity);
			serialPort.Parity = (System.IO.Ports.Parity)parity;
			terminal.parity = parity;
		}

		private void TerminalOnToggleConnection(object sender, ToggleConnectionEventArgs e)
		{
			logger.Trace("Connection toggled");
			ToggleConnected();
		}

		private void TerminalOnSerialPortList(object sender, SerialPortListEventArgs e)
		{
			logger.Trace("Serial ports listed");
			e.ports = SerialPort.GetPortNames();
			logger.Debug("{0} ports available", e.ports.Length);
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
				terminal.AddChar(c);
		}

		private void TerminalOnKeyPressed(object sender, OnKeyPressedEventArgs e)
		{
			logger.Trace("Key pressed: {0}", e.KeyChar);
			SendChar(e.KeyChar);
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

				terminal.loggingState = loggingState;

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
					loggingFile = terminal.LoggingFilePath;

				//TODO Remove old API call
				if (loggingFile == null)
					loggingFile = terminal.GetLoggingFilePath();

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
				terminal.AddLine(e.Message);
			}
			catch (Exception e)
			{
				logger.ErrorException("Generic exception encountered in ToggleLogging", e);
				terminal.AddLine(e.Message);
			}

			terminal.loggingState = loggingState;

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
			terminal.echoState = state;
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

				terminal.portState = state;

				//TODO Remove old API call
				terminal.SetPortConnection(state);
			}
			catch (Exception e)
			{
				logger.ErrorException("SetPortConnection failed", e);
				terminal.portState = PortState.Error;

				//TODO Remove old API call
				terminal.SetPortConnection(PortState.Error);
				terminal.AddLine(e.Message);
			}
		}

		/// <summary>
		/// Toggle COM port connection state
		/// </summary>
		private void ToggleConnected()
		{
			if (serialPort.IsOpen)
				SetPortConnection(PortState.Closed);
			else
				SetPortConnection(PortState.Open);
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
				terminal.portState = PortState.Error;

				//TODO Remove old API call
				terminal.SetPortConnection(PortState.Error);
				return;
			}

			try
			{
				terminal.fileSendState = FileSendState.InProgress;

				//TODO Remove old API call
				terminal.SetFileSendState(FileSendState.InProgress);

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
				terminal.fileSendState = FileSendState.Success;

				//TODO Remove old API call
				terminal.SetFileSendState(FileSendState.Success);
			}
			catch (Exception e)
			{
				logger.ErrorException("Error in file send thread", e);
				terminal.fileSendState = FileSendState.Error;

				//TODO Remove old API call
				terminal.SetFileSendState(FileSendState.Error);
				terminal.AddLine(e.Message);
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
				terminal.portState = PortState.Error;

				//TODO Remove old API call
				terminal.SetPortConnection(PortState.Error);
				return;
			}

			if (!serialPort.IsOpen)
			{
				logger.Debug("TerminalOnSendFile hit a closed serial port");
				terminal.portState = PortState.Error;
				terminal.fileSendState = FileSendState.Error;

				//TODO Remove old API calls
				terminal.SetFileSendState(FileSendState.Error);
				terminal.SetPortConnection(PortState.Error);
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
				terminal.AddLine("Already sending a file, please wait until send is complete");
				return;
			}
		}

		private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			logger.Trace("Received {0} bytes", serialPort.BytesToRead);
			if (!serialPort.IsOpen)
				return;

			// Print the received data to the screen
			int count = serialPort.BytesToRead;

			if (count == 0)
				return;

			if (count > 200)
			{
				logger.Warn("High memory usage detected, trimming lines!");
				terminal.TrimLines(1000);
			}

			//if (count > 1024)
			//	count = 1024;

			serialPort.Read(recieveBuffer, 0, count);

			//TODO Translate unprintable characters to hex
			string inStr = System.Text.Encoding.ASCII.GetString(recieveBuffer, 0, count);

			//int inStrLen = inStr.Length;

			inStr = inStr.Replace(new string(new char[] { (char)0xA, (char)0xD }), Environment.NewLine);

			//if (inStrLen > 1)
			//{
			//	for (int i = 1; i < inStrLen; i++)
			//		if ((inStr[i - 1] == 0x0a) && (inStr[i] == 0x0d))
			//		{
			//			inStr = inStr.Remove(i, 1);
			//			inStrLen--;
			//		}

			//	lastChar = inStr[inStrLen - 1];
			//}
			//else
			//	if (inStrLen == 1)
			//		if ((lastChar == 0x0a) && (inStr[0] == 0x0d))
			//			return;

			terminal.AddLine(inStr);

			//TODO implement this in the backend code
			//if (loggingEnabled)
			//    if (loggingStream.CanWrite)
			//        loggingStream.Write(recieveBuffer, 0, count);
		}

		#endregion Serial port functions

		private ITerminal terminal;
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

		private SerialPort serialPort;

		/// <summary>
		/// Buffer for serial port incoming data
		/// </summary>
		private byte[] recieveBuffer;

		private char lastChar;

		#region Implementation of IBackend

		public string[] GetSerialPorts()
		{
			return SerialPort.GetPortNames();
		}

		public void KeyPressed(char c)
		{
			SendChar(c);
		}

		#endregion Implementation of IBackend
	}
}