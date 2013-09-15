using System.Collections.Generic;
using Terminal.Interface;
using Terminal.Interface.Enums;
using Terminal.Interface.GUI;

namespace HyperToken.WinFormsGUI.Menu
{
    public interface ISerialSettingsMenu
    {
        Terminal.Interface.GUI.Menu Menu { get; }
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
        private Terminal.Interface.GUI.Menu _menu;
        private readonly IEnumerable<ISerialSettingsMenu> _serialSettingsMenus;

        public SerialMenu(IEnumerable<ISerialSettingsMenu> serialSettingsMenus)
        {
            _serialSettingsMenus = serialSettingsMenus;
        }

        public Terminal.Interface.GUI.Menu Menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu = new Terminal.Interface.GUI.Menu("Serial Settings");
                    foreach (var serialSettingsMenu in _serialSettingsMenus)
                        _menu.Items.Add(serialSettingsMenu.Menu);
                }

                return _menu;
            }
        }
    }
}