using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

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