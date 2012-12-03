using System.ComponentModel;
using Terminal_Interface.Enums;

namespace Terminal_Interface
{
	public interface IBackend : INotifyPropertyChanged
	{
		void KeyPressed(char c);

		void Shutdown();

		string Title { get; set; }

		string LoggingFilePath { get; set; }

		LoggingState LoggingState { get; set; }

		EchoState EchoState { get; set; }

		PortState PortState { get; set; }

		FileSendState FileSendState { get; set; }

		string StatusLabel { get; set; }

		string[] Devices { get; }

		string CurrentDevice { get; set; }
	}

	public interface IHIDBackend : IBackend
	{
		int ReportLength { get; set; }
	}

	public interface ISerialBackend : IBackend
	{
		int Baud { get; set; }

		StopBits StopBits { get; set; }

		int DataBits { get; set; }

		FlowControl FlowControl { get; set; }

		Parity Parity { get; set; }
	}
}