using System.ComponentModel;

namespace Terminal_Interface
{
    public class PacketParserHandler : INotifyPropertyChanged
    {
        public IPacketInterpreter CurrentParser { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public PacketParserHandler(IPacketInterpreter defaultParser)
        {
            CurrentParser = defaultParser;
        }
    }
}
