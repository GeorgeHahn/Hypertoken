using System.ComponentModel;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;

namespace Terminal_Interface
{
	public delegate void SaveSessionEventHandler(object sender, SaveSessionEventArgs e);

	//TODO Add dropped file event

	public interface ITerminal : INotifyPropertyChanged
	{
		void Run();

		void AddLine(string line);

		void AddChar(char c);

		string Title { get; set; }

		void SetDevice(ISerialPort device);

		event SaveSessionEventHandler OnSaveSession;
	}
}