using System.ComponentModel;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;

namespace Terminal_Interface
{
	public delegate void SendFileEventHandler(object sender, SendFileEventArgs e);

	public delegate void SetLoggingPathEventHandler(object sender, SetLoggingPathEventArgs e);

	public delegate void ToggleConnectionEventHandler(object sender, ToggleConnectionEventArgs e);

	public delegate void OnKeyPressedEventHandler(object sender, OnKeyPressedEventArgs e);

	public delegate void SerialPortListEventHandler(object sender, SerialPortListEventArgs e);

	public delegate void SaveSessionEventHandler(object sender, SaveSessionEventArgs e);

	//TODO Add dropped file event

	public interface ITerminal : INotifyPropertyChanged
	{
		// Required functions

		void AddLine(string line);

		void AddChar(char c);

		void Run();

		void TrimLines(int trimTo); // TODO stopgap measure to trim lines

		void SetBackend(IBackend backend);

		// Required (?) events
		event SendFileEventHandler OnSendFile;

		event OnKeyPressedEventHandler OnKeyPressed;

		// TODO How can this be eliminated?
		event SerialPortListEventHandler OnSerialPortList;

		// New API
		string Title { get; set; }

		string LoggingFilePath { get; set; }

		LoggingState loggingState { get; set; }

		EchoState echoState { get; set; }

		PortState portState { get; set; }

		FileSendState fileSendState { get; set; }

		string COMPort { get; set; }

		int baud { get; set; }

		StopBits stopBits { get; set; }

		int dataBits { get; set; }

		FlowControl flowControl { get; set; }

		Parity parity { get; set; }

		string[] serialPorts { get; }

		// Old API

		event SetLoggingPathEventHandler OnSetLoggingPath;

		event ToggleConnectionEventHandler OnToggleConnection;

		event SaveSessionEventHandler OnSaveSession;

		string GetLoggingFilePath();

		void SetPortConnection(PortState state);

		void SetFileSendState(FileSendState fileSendState);
	}
}