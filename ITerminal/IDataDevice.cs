using System.Collections.Generic;

namespace Terminal_Interface
{
	public interface IDataDevice : IDataWriter, IDataReader
	{
		new void Close();

		new void Open();

		new bool IsOpen { get; }

		IEnumerable<string> ListAvailableDevices();

		string DeviceName { get; set; }

		string DeviceStatus { get; }
	}

	public interface ISerialPort : IDataDevice
	{
	}

	public interface IHIDDevice : IDataDevice
	{
	}
}