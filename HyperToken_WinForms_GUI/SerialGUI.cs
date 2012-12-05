﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Anotar;
using NLog;
using Terminal_GUI_Interface;
using Terminal_Interface;
using Terminal_Interface.Enums;

namespace HyperToken_WinForms_GUI
{
	public interface ISerialSettingsMenu
	{
		ToolStripItem Menu { get; }
	}

	public abstract class SerialSettingsMenu : ISerialSettingsMenu
	{
		internal ToolStripMenuItem _menu;

		internal abstract dynamic Values { get; }

		internal abstract string MenuName { get; }

		internal readonly ISerialPort SerialPort;

		internal abstract dynamic ItemValue { get; set; }

		internal abstract string PropertyName { get; }

		protected SerialSettingsMenu(ISerialPort serialPort)
		{
			SerialPort = serialPort;
			SerialPort.PropertyChanged += (sender, args) => UpdateCheckedStates(args.PropertyName);
		}

		public ToolStripItem Menu
		{
			get
			{
				if (_menu == null)
				{
					_menu = new ToolStripMenuItem(MenuName);
					_menu.DropDownItemClicked += (sender, args) => ItemClicked(_menu.DropDownItems.IndexOf(args.ClickedItem));
					foreach (var value in Values)
					{
						_menu.DropDownItems.Add(value.ToString());
					}
					UpdateCheckedStates(PropertyName);
				}

				return _menu;
			}
		}

		private void ItemClicked(int item)
		{
			ItemValue = Values[item];
		}

		private void UpdateCheckedStates(string propertyName)
		{
			if (propertyName != PropertyName) return;

			foreach (ToolStripMenuItem menu in _menu.DropDownItems)
			{
				menu.Checked = menu.Text == ItemValue.ToString();
			}
		}
	}

	public class DataBitsMenu : SerialSettingsMenu
	{
		public DataBitsMenu(ISerialPort serialPort)
			: base(serialPort)
		{ }

		internal override dynamic Values
		{
			get
			{
				return new[] { 5, 6, 7, 8 };
			}
		}

		internal override string MenuName
		{
			get { return "Data Bits"; }
		}

		internal override dynamic ItemValue
		{
			get { return SerialPort.DataBits; }
			set { SerialPort.DataBits = value; }
		}

		internal override string PropertyName
		{
			get { return "DataBits"; }
		}
	}

	public class FlowControlMenu : SerialSettingsMenu
	{
		public FlowControlMenu(ISerialPort serialPort)
			: base(serialPort)
		{ }

		internal override dynamic Values
		{
			get
			{
				return new[]
				             {
					             FlowControl.None,
								 FlowControl.RequestToSend,
								 FlowControl.RequestToSendXOnXOff,
								 FlowControl.XOnXOff,
				             };
			}
		}

		internal override string MenuName
		{
			get { return "Flow Control"; }
		}

		internal override dynamic ItemValue
		{
			get { return SerialPort.FlowControl; }
			set { SerialPort.FlowControl = value; }
		}

		internal override string PropertyName
		{
			get { return "FlowControl"; }
		}
	}

	public class BaudRateMenu : SerialSettingsMenu
	{
		public BaudRateMenu(ISerialPort serialPort)
			: base(serialPort)
		{ }

		internal override dynamic Values
		{
			get
			{
				return new[]
					       {
						       110,
							   300,
							   1200,
							   2400,
							   4800,
							   9600,
							   19200,
							   38400,
							   57600,
							   115200,
							   230400,
							   460800,
							   921600
					       };
			}
		}

		internal override string MenuName
		{
			get { return "Baud Rate"; }
		}

		internal override dynamic ItemValue
		{
			get { return SerialPort.Baud; }
			set { SerialPort.Baud = value; }
		}

		internal override string PropertyName
		{
			get { return "Baud"; }
		}
	}

	public class PortMenu : SerialSettingsMenu
	{
		public PortMenu(ISerialPort serialPort)
			: base(serialPort)
		{ }

		internal override dynamic Values
		{
			get { return SerialPort.Devices; }
		}

		internal override string MenuName
		{
			get { return "COM Port"; }
		}

		internal override dynamic ItemValue
		{
			get { return SerialPort.DeviceName; }
			set { SerialPort.DeviceName = value; }
		}

		internal override string PropertyName
		{
			get { return "DeviceName"; }
		}
	}

	public class ParityMenu : SerialSettingsMenu
	{
		public ParityMenu(ISerialPort serialPort)
			: base(serialPort)
		{ }

		internal override dynamic Values
		{
			get
			{
				return new[]
					       {
						       Parity.Even,
						       Parity.Odd,
						       Parity.None,
						       Parity.Mark,
						       Parity.Space,
					       };
			}
		}

		internal override string MenuName
		{
			get { return "Parity"; }
		}

		internal override dynamic ItemValue
		{
			get { return SerialPort.Parity; }
			set { SerialPort.Parity = value; }
		}

		internal override string PropertyName
		{
			get { return "Parity"; }
		}
	}

	public class StopBitsMenu : SerialSettingsMenu
	{
		public StopBitsMenu(ISerialPort serialPort)
			: base(serialPort)
		{ }

		internal override dynamic Values
		{
			get
			{
				return new[]
				             {
					             StopBits.One,
								 StopBits.OnePointFive,
								 StopBits.Two,
				             };
			}
		}

		internal override string MenuName
		{
			get { return "Stop Bits"; }
		}

		internal override dynamic ItemValue
		{
			get { return SerialPort.StopBits; }
			set { SerialPort.StopBits = value; }
		}

		internal override string PropertyName
		{
			get { return "StopBits"; }
		}
	}

	public class SerialMenu : IMainMenuExtension
	{
		private ToolStripMenuItem _menu;
		private readonly IEnumerable<ISerialSettingsMenu> _serialSettingsMenus;

		public SerialMenu(IEnumerable<ISerialSettingsMenu> serialSettingsMenus)
		{
			_serialSettingsMenus = serialSettingsMenus;
		}

		public ToolStripMenuItem Menu
		{
			get
			{
				if (_menu == null)
				{
					_menu = new ToolStripMenuItem("Serial Settings");
					foreach (var serialSettingsMenu in _serialSettingsMenus)
						_menu.DropDownItems.Add(serialSettingsMenu.Menu);
				}

				return _menu;
			}
		}
	}

	public class SerialStatusbarPortMenu : IStatusbarExtension
	{
		private ToolStripDropDownButton _statusBarItem;
		private static ISerialPort _serialPort;

		public SerialStatusbarPortMenu(ISerialPort iSerialPort)
		{
			_serialPort = iSerialPort;
			_serialPort.PropertyChanged += (sender, args) => UpdateLabel();
		}

		private void UpdateLabel()
		{
			_statusBarItem.Text = _serialPort.DeviceName;
			foreach (ToolStripMenuItem portItem in _statusBarItem.DropDownItems)
			{
				if (portItem.Text == _statusBarItem.Text)
					portItem.Checked = true;
				else
					portItem.Checked = false;
			}
		}

		public ToolStripItem StatusBarItem
		{
			get
			{
				if (_statusBarItem == null)
				{
					_statusBarItem = new ToolStripDropDownButton("COM1");
					_statusBarItem.DropDownOpening += (sender, args) => UpdatePortList((ToolStripDropDownItem)sender);
					_statusBarItem.DropDownItemClicked += (sender, args) => SetSerialPort(args.ClickedItem.Text);
				}

				return _statusBarItem;
			}
		}

		private void SetSerialPort(string port)
		{
			_serialPort.DeviceName = port;
		}

		public static void UpdatePortList(ToolStripDropDownItem item)
		{
			var ports = _serialPort.Devices;

			if (ports == null)
			{
				Log.Error("No serial ports to list");
				Log.Error("TODO We should handle this more gracefully");
				Log.Error("Show a 'No serial ports found' item");
				return;
			}

			item.DropDownItems.Clear();
			foreach (var port in ports)
				item.DropDownItems.Add(port);
		}
	}

	public class SerialStatusbarStatusLabel : IStatusbarExtension
	{
		private ToolStripStatusLabel _statusBarItem;
		private readonly ISerialPort _serialPort;

		public SerialStatusbarStatusLabel(ISerialPort serialPort)
		{
			_serialPort = serialPort;
			_serialPort.PropertyChanged += (sender, args) => UpdateText();
		}

		private void UpdateText()
		{
			_statusBarItem.Text = _serialPort.DeviceStatus;
		}

		public ToolStripItem StatusBarItem
		{
			get
			{
				if (_statusBarItem == null)
				{
					_statusBarItem = new ToolStripStatusLabel(_serialPort.DeviceStatus);
				}

				return _statusBarItem;
			}
		}
	}

	public class SerialStatusbarBaudMenu : IStatusbarExtension
	{
		private ToolStripItem _statusBarItem;
		private readonly ISerialPort _serialPort;
		private readonly List<int> _baudRates;

		public SerialStatusbarBaudMenu(ISerialPort serialPort)
		{
			_serialPort = serialPort;
			_serialPort.PropertyChanged += (sender, args) => UpdateBaudSelection();
			_baudRates = new List<int>(new[] { 110, 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 });
		}

		private void UpdateBaudSelection()
		{
			_statusBarItem.Text = _serialPort.Baud.ToString();

			var tempStatusBarItem = (ToolStripDropDownButton)_statusBarItem;
			foreach (ToolStripMenuItem baudItem in tempStatusBarItem.DropDownItems)
			{
				baudItem.Checked = baudItem.Text == _statusBarItem.Text;
			}
		}

		public ToolStripItem StatusBarItem
		{
			get
			{
				if (_statusBarItem == null)
				{
					var tempStatusBarItem = new ToolStripDropDownButton();
					tempStatusBarItem.Text = _serialPort.Baud.ToString();
					foreach (var baudRate in BaudRates)
						tempStatusBarItem.DropDownItems.Add(baudRate.ToString());

					tempStatusBarItem.DropDownItemClicked += (sender, args) => SetBaudTo(args.ClickedItem.Text);
					_statusBarItem = tempStatusBarItem;
				}

				return _statusBarItem;
			}
		}

		private void SetBaudTo(string baudRate)
		{
			_serialPort.Baud = int.Parse(baudRate);
		}

		public IEnumerable<int> BaudRates
		{
			get { return _baudRates; }
		}
	}
}