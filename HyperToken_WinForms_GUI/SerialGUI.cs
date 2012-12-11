using System;
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
using Menu = Terminal_GUI_Interface.Menu;

namespace HyperToken_WinForms_GUI
{
    public interface ISerialSettingsMenu
    {
        Menu Menu { get; }
    }

    public abstract class SerialSettingsMenu : GenericSettingsMenu, ISerialSettingsMenu
    {
        protected readonly ISerialPort SerialPort;

        protected SerialSettingsMenu(ISerialPort serialPort)
        {
            SerialPort = serialPort;
            SerialPort.PropertyChanged += (sender, args) => UpdateCheckedStates(args.PropertyName);
        }
    }

    public class DataBitsMenu : SerialSettingsMenu
    {
        public DataBitsMenu(ISerialPort serialPort)
            : base(serialPort)
        { }

        protected override dynamic Values
        {
            get
            {
                return new[] { 5, 6, 7, 8 };
            }
        }

        protected override string MenuName
        {
            get { return "Data Bits"; }
        }

        protected override dynamic ItemValue
        {
            get { return SerialPort.DataBits; }
            set { SerialPort.DataBits = value; }
        }

        protected override string PropertyName
        {
            get { return "DataBits"; }
        }
    }

    public class FlowControlMenu : SerialSettingsMenu
    {
        public FlowControlMenu(ISerialPort serialPort)
            : base(serialPort)
        { }

        protected override dynamic Values
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

        protected override string MenuName
        {
            get { return "Flow Control"; }
        }

        protected override dynamic ItemValue
        {
            get { return SerialPort.FlowControl; }
            set { SerialPort.FlowControl = value; }
        }

        protected override string PropertyName
        {
            get { return "FlowControl"; }
        }
    }

    public class BaudRateMenu : SerialSettingsMenu
    {
        public BaudRateMenu(ISerialPort serialPort)
            : base(serialPort)
        { }

        protected override dynamic Values
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

        protected override string MenuName
        {
            get { return "Baud Rate"; }
        }

        protected override dynamic ItemValue
        {
            get { return SerialPort.Baud; }
            set { SerialPort.Baud = value; }
        }

        protected override string PropertyName
        {
            get { return "Baud"; }
        }
    }

    public class PortMenu : SerialSettingsMenu
    {
        private readonly CurrentDataDevice _dataDevice;

        public PortMenu(ISerialPort serialPort, CurrentDataDevice terminal)
            : base(serialPort)
        {
            _dataDevice = terminal;
        }

        protected override dynamic Values
        {
            get { return SerialPort.Devices; }
        }

        protected override string MenuName
        {
            get { return "COM Port"; }
        }

        protected override dynamic ItemValue
        {
            get { return SerialPort.DeviceName; }
            set
            {
                SerialPort.DeviceName = value;
                _dataDevice.CurrentDevice = SerialPort;
            }
        }

        protected override string PropertyName
        {
            get { return "DeviceName"; }
        }

        protected override bool UpdateOnOpen
        {
            get { return true; }
        }
    }

    public class ParityMenu : SerialSettingsMenu
    {
        public ParityMenu(ISerialPort serialPort)
            : base(serialPort)
        { }

        protected override dynamic Values
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

        protected override string MenuName
        {
            get { return "Parity"; }
        }

        protected override dynamic ItemValue
        {
            get { return SerialPort.Parity; }
            set { SerialPort.Parity = value; }
        }

        protected override string PropertyName
        {
            get { return "Parity"; }
        }
    }

    public class StopBitsMenu : SerialSettingsMenu
    {
        public StopBitsMenu(ISerialPort serialPort)
            : base(serialPort)
        { }

        protected override dynamic Values
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

        protected override string MenuName
        {
            get { return "Stop Bits"; }
        }

        protected override dynamic ItemValue
        {
            get { return SerialPort.StopBits; }
            set { SerialPort.StopBits = value; }
        }

        protected override string PropertyName
        {
            get { return "StopBits"; }
        }
    }

    public class SerialMenu : IMainMenuExtension
    {
        private Menu _menu;
        private readonly IEnumerable<ISerialSettingsMenu> _serialSettingsMenus;

        public SerialMenu(IEnumerable<ISerialSettingsMenu> serialSettingsMenus)
        {
            _serialSettingsMenus = serialSettingsMenus;
        }

        public Menu Menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu = new Menu("Serial Settings");
                    foreach (var serialSettingsMenu in _serialSettingsMenus)
                        _menu.Items.Add(serialSettingsMenu.Menu);
                }

                return _menu;
            }
        }
    }

    public abstract class SerialStatusBarExtension : IStatusbarExtension
    {
        protected static ISerialPort _serialPort;
        protected CurrentDataDevice _currentDataDevice;
        protected ToolStripItem _statusBarItem;

        protected SerialStatusBarExtension(ISerialPort iSerialPort, CurrentDataDevice currentDataDevice)
        {
            _serialPort = iSerialPort;
            _serialPort.PropertyChanged += (sender, args) => UpdateStatusBarItem();
            _currentDataDevice = currentDataDevice;
            _currentDataDevice.PropertyChanged += (sender, args) => UpdateVisibility();
        }

        protected void UpdateVisibility()
        {
            if (_currentDataDevice.CurrentDevice as ISerialPort != null)
            {
                _statusBarItem.Visible = true;
                return;
            }

            _statusBarItem.Visible = false;
        }

        public ToolStripItem StatusBarItem
        {
            get
            {
                if (_statusBarItem == null)
                    CreateStatusBarItem();
                return _statusBarItem;
            }
        }

        protected abstract void CreateStatusBarItem();

        protected abstract void UpdateStatusBarItem();
    }

    public class SerialStatusbarPortMenu : SerialStatusBarExtension
    {
        public SerialStatusbarPortMenu(ISerialPort iSerialPort, CurrentDataDevice currentDataDevice)
            : base(iSerialPort, currentDataDevice)
        { }

        protected override void UpdateStatusBarItem()
        {
            _statusBarItem.Text = _serialPort.DeviceName;
            foreach (ToolStripMenuItem portItem in ((ToolStripDropDownButton)_statusBarItem).DropDownItems)
            {
                if (portItem.Text == _statusBarItem.Text)
                    portItem.Checked = true;
                else
                    portItem.Checked = false;
            }
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

        protected override void CreateStatusBarItem()
        {
            var tempDropDown = new ToolStripDropDownButton("COM1");
            tempDropDown.DropDownOpening += (sender, args) => UpdatePortList((ToolStripDropDownItem)sender);
            tempDropDown.DropDownItemClicked += (sender, args) => _serialPort.DeviceName = args.ClickedItem.Text;
            _statusBarItem = tempDropDown;
        }
    }

    public class SerialStatusbarStatusLabel : SerialStatusBarExtension
    {
        public SerialStatusbarStatusLabel(ISerialPort serialPort, CurrentDataDevice currentDataDevice)
            : base(serialPort, currentDataDevice)
        { }

        protected override void UpdateStatusBarItem()
        {
            _statusBarItem.Text = _serialPort.DeviceStatus;
        }

        protected override void CreateStatusBarItem()
        {
            _statusBarItem = new ToolStripStatusLabel(_serialPort.DeviceStatus);
        }
    }

    public class SerialStatusbarBaudMenu : SerialStatusBarExtension
    {
        private readonly List<int> _baudRates;

        public SerialStatusbarBaudMenu(ISerialPort serialPort, CurrentDataDevice currentDataDevice)
            : base(serialPort, currentDataDevice)
        {
            _baudRates = new List<int>(new[] { 110, 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 });
        }

        protected override void UpdateStatusBarItem()
        {
            _statusBarItem.Text = _serialPort.Baud.ToString();

            var tempStatusBarItem = (ToolStripDropDownButton)_statusBarItem;
            foreach (ToolStripMenuItem baudItem in tempStatusBarItem.DropDownItems)
            {
                baudItem.Checked = baudItem.Text == _statusBarItem.Text;
            }
        }

        protected override void CreateStatusBarItem()
        {
            var tempStatusBarItem = new ToolStripDropDownButton();
            tempStatusBarItem.Text = _serialPort.Baud.ToString();
            foreach (var baudRate in BaudRates)
                tempStatusBarItem.DropDownItems.Add(baudRate.ToString());

            tempStatusBarItem.DropDownItemClicked += (sender, args) => SetBaudTo(args.ClickedItem.Text);
            _statusBarItem = tempStatusBarItem;
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