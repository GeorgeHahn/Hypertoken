using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Anotar;
using HidLibrary;
using NLog;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;

namespace Terminal
{
	public class HIDDataHandler : ISerialPort
	{
		public HIDDataHandler()
		{
		}

		public IEnumerable<string> ListAvailableDevices()
		{
			var devices = HidDevices.Enumerate();
			List<string> names = new List<string>();
			foreach (HidDevice device in devices)
				names.Add(device.Attributes.VendorHexId + "," + device.Attributes.ProductHexId);
			return names.AsEnumerable();
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
			get
			{
				return ListAvailableDevices().ToArray();
			}
		}

		public void KeyPressed(char c)
		{
			throw new NotImplementedException();
		}

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

		public event DataReceivedEventHandler DataReceived;

		public event PropertyChangedEventHandler PropertyChanged;

		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below
		// TODO remove below

		#region ISerialPort Members

		public int Baud
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

		public StopBits StopBits
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

		public int DataBits
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

		public FlowControl FlowControl
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

		public Parity Parity
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

		#endregion ISerialPort Members
	}
}