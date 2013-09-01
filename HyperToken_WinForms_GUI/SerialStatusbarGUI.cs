using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Anotar;
using Anotar.NLog;
using Terminal_GUI_Interface;
using Terminal_Interface;
using NLog;

namespace HyperToken_WinForms_GUI
{
    // TODO Refactor this to use Menu and move it into Terminal\GUI
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
                LogTo.Error("No serial ports to list");
                LogTo.Error("TODO We should handle this more gracefully");
                LogTo.Error("Show a 'No serial ports found' item");
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
            tempDropDown.DropDownItemClicked += (sender, args) =>
            {
                _serialPort.DeviceName = args.ClickedItem.Text;
                _currentDataDevice.CurrentDevice = _serialPort;
            };
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
