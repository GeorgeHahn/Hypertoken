using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Terminal_Interface
{
    public class CurrentDataDevice : INotifyPropertyChanged
    {
        public IDataDevice CurrentDevice { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}