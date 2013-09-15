using System.ComponentModel;

namespace Terminal.Interface
{
    public class CurrentDataDevice : INotifyPropertyChanged
    {
        public IDataDevice CurrentDevice { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}