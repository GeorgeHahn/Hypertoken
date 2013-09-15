using System.ComponentModel;

namespace Terminal.Interface
{
    public class CurrentPacketInterpreter : INotifyPropertyChanged
    {
        public CurrentPacketInterpreter(IPacketInterpreter packetInterpreter)
        {
            PacketInterpreter = packetInterpreter;
        }

        public IPacketInterpreter PacketInterpreter { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}