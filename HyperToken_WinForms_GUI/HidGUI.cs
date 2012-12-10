using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Terminal_GUI_Interface;
using Terminal_Interface;
using Terminal_Interface.Enums;

namespace HyperToken_WinForms_GUI
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
			DataDevice.PropertyChanged += (sender, args) => UpdateCheckedStates(args.PropertyName);
		}
	}

	public class DeviceSelectionMenu : HidSettingsMenu
	{
		public DeviceSelectionMenu(IDataDevice dataDevice)
			: base(dataDevice)

		{ }

		protected override dynamic Values
		{
			get
			{
				return DataDevice.Devices;
			}
		}

		protected override string MenuName
		{
			get { return "Device Selection"; }
		}

		protected override dynamic ItemValue
		{
			get { return DataDevice.DeviceName; }
			set { DataDevice.DeviceName = value; }
		}

		protected override string PropertyName
		{
			get { return "DeviceName"; }
		}

		protected override bool UpdateOnOpen
		{
			get { return false; }
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