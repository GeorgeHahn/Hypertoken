using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using Anotar;
using NLog;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;

namespace Terminal
{
	public class HIDDataHandler : IDataDevice
	{
		public HIDDataHandler()
		{
		}

		#region IDataDevice Members

		public IEnumerable<string> ListAvailableDevices()
		{
			throw new NotImplementedException();
		}

		public string DeviceName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string DeviceStatus
		{
			get { throw new NotImplementedException(); }
		}

		public deviceType DeviceType
		{
			get { throw new NotImplementedException(); }
		}

		public PortState PortState
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string[] Devices
		{
			get { throw new NotImplementedException(); }
		}

		public void KeyPressed(char c)
		{
			throw new NotImplementedException();
		}

		#endregion IDataDevice Members

		#region IDataWriter Members

		public int Write(byte[] data)
		{
			throw new NotImplementedException();
		}

		public int Write(byte data)
		{
			throw new NotImplementedException();
		}

		public int Write(char data)
		{
			throw new NotImplementedException();
		}

		public int Write(string data)
		{
			throw new NotImplementedException();
		}

		#endregion IDataWriter Members

		#region IDataReader Members

		public event DataReceivedEventHandler DataReceived;

		#endregion IDataReader Members

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion INotifyPropertyChanged Members
	}
}