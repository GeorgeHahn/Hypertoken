using System.Collections.Generic;
using System.ComponentModel;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;

namespace Terminal_Interface
{
    public enum deviceType
    {
        SerialPort,
        HID,
    }

    public interface IDataDevice : IDataWriter, IDataReader, INotifyPropertyChanged
    {
        IEnumerable<string> ListAvailableDevices();

        string DeviceName { get; set; }

        string FriendlyName { get; }

        string DeviceStatus { get; }

        deviceType DeviceType { get; }

        PortState PortState { get; set; }

        string[] Devices { get; }

        void KeyPressed(char c);
    }

    public interface ISerialPort : IDataDevice
    {
        int Baud { get; set; }

        StopBits StopBits { get; set; }

        int DataBits { get; set; }

        FlowControl FlowControl { get; set; }

        Parity Parity { get; set; }
    }

    public interface IHIDDevice : IDataDevice
    {
        int ReportLength { get; set; }
    }

    public interface ILogger : IDataWriter, INotifyPropertyChanged
    {
        string LoggingFilePath { get; set; }

        LoggingState LoggingState { get; set; }
    }

    public interface IEchoer
    {
        EchoState EchoState { get; set; }
    }

    public interface IFileSender
    {
        FileSendState FileSendState { get; set; }
    }
}