using System.Collections.Generic;
using System.Linq;
using Terminal_GUI_Interface;
using Terminal_Interface;
using Terminal_Interface.Enums;

namespace Terminal.GUI
{
    public interface IHidSettingsMenu
    {
        Menu Menu { get; }
    }

    public abstract class HidSettingsMenu : GenericSettingsMenu, IHidSettingsMenu
    {
        protected readonly IDataDevice DataDevice;

        protected HidSettingsMenu(IDataDevice dataDevice)
        {
            DataDevice = dataDevice;
            dataDevice.PropertyChanged += (sender, args) => UpdateCheckedStates(args.PropertyName);
        }
    }

    // Doesn't implement GenericSettingsMenu
    public class DeviceSelectionMenu : IHidSettingsMenu
    {
        private readonly IDataDevice _dataDevice;
        private readonly CurrentDataDevice _terminal;

        public DeviceSelectionMenu(IDataDevice dataDevice, CurrentDataDevice terminal)
        {
            _dataDevice = dataDevice;
            _terminal = terminal;
            _dataDevice.PropertyChanged += (sender, args) => UpdateCheckedStates(args.PropertyName);
        }

        private Menu _menu;

        private string MenuName
        {
            get { return "Device Selection"; }
        }

        private dynamic ItemValue
        {
            get { return _dataDevice.DeviceName; }
            set
            {
                _dataDevice.DeviceName = value;
                _terminal.CurrentDevice = _dataDevice;
            }
        }

        private string PropertyName
        {
            get { return "DeviceName"; }
        }

        private bool UpdateOnOpen
        {
            get { return true; }
        }

        public Menu Menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu = new Menu(MenuName);
                    _menu.ItemClicked += (sender, args) => ItemClicked(Menu.GetIndex(_menu.Items, args.ClickedItem));
                    if (UpdateOnOpen)
                        _menu.ItemsListOpening += (sender, args) => SetItems();
                    SetItems();
                }

                return _menu;
            }
        }

        private string[] _devicePaths;
        private string[] _deviceNames;

        private void SetItems()
        {
            _menu.Items.Clear();
            _devicePaths = _dataDevice.Devices;
            _deviceNames = _dataDevice.ListAvailableDevices().ToArray();

            var menus = new List<Menu>();
            foreach (var value in _deviceNames)
                menus.Add(new Menu(value));

            _menu.AddRange(menus);

            UpdateCheckedStates(PropertyName);
        }

        private void ItemClicked(int item)
        {
            ItemValue = _devicePaths[item];
        }

        private void UpdateCheckedStates(string propertyName)
        {
            if (propertyName != PropertyName) return;

            foreach (var menuItem in _menu.Items)
            {
                menuItem.Checked = menuItem.Text == ItemValue.ToString();
            }
        }
    }

    public class HidDeviceConnection : HidSettingsMenu
    {
        public HidDeviceConnection(IDataDevice dataDevice)
            : base(dataDevice)
        { }

        protected override dynamic Values
        {
            get
            {
                return new[]
				             {
					             PortState.Open,
								 PortState.Closed,
				             };
            }
        }

        protected override string MenuName
        {
            get { return "Connection"; }
        }

        protected override dynamic ItemValue
        {
            get { return DataDevice.PortState; }
            set { DataDevice.PortState = value; }
        }

        protected override string PropertyName
        {
            get { return "PortState"; }
        }
    }

    public class HidMenu : IMainMenuExtension
    {
        private Menu _menu;
        private readonly IEnumerable<IHidSettingsMenu> _hidSettingsMenus;

        public HidMenu(IEnumerable<IHidSettingsMenu> hidSettingsMenus)
        {
            _hidSettingsMenus = hidSettingsMenus;
        }

        public Menu Menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu = new Menu("HID Settings");
                    foreach (var serialSettingsMenu in _hidSettingsMenus)
                        _menu.Items.Add(serialSettingsMenu.Menu);
                }

                return _menu;
            }
        }
    }
}