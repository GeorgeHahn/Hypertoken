using System.ComponentModel;
using Terminal.Interface.Events;
using Terminal.Interface.Enums;

namespace Terminal.Interface
{
    public delegate void SaveSessionEventHandler(object sender, SaveSessionEventArgs e);

    //TODO Add dropped file event

    public interface ITerminal : INotifyPropertyChanged
    {
        void Run();

        void AddLine(string line);

        void AddChar(char c);

        string Title { get; set; }

        event SaveSessionEventHandler OnSaveSession;
    }
}