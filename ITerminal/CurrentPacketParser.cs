using System.ComponentModel;

namespace Terminal.Interface
{
    public class CurrentPacketParser : INotifyPropertyChanged
    {
        public IPacketInterpreter CurrentParser { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public CurrentPacketParser(IPacketInterpreter defaultParser)
        {
            CurrentParser = defaultParser;
        }
    }
}
