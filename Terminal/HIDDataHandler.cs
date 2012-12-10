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
	public class HIDDataHandler : IDataDevice
	{
		private HidDevice _device;

		public HIDDataHandler()
		{
		}

		public IEnumerable<string> ListAvailableDevices()
		{
			var devices = HidDevices.Enumerate();
			List<string> names = new List<string>();
			foreach (HidDevice device in devices)
			{
				names.Add(string.Format("{0}, {1}: {2}", device.Attributes.VendorHexId, device.Attributes.ProductHexId, device.Description));
			}
			return names.AsEnumerable();
		}

		public string[] Devices
		{
			get
			{
				return ListAvailableDevices().ToArray();
			}
		}

		public string DeviceName
		{
			get
			{
				if (_device == null)
					return string.Empty;

				return _device.DevicePath;
			}
			set { _device = HidDevices.GetDevice(value); }
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
	}
}