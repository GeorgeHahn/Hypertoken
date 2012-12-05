using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Terminal_GUI_Interface;

namespace HyperToken_WinForms_GUI
{
	public interface IHidSettingsMenu
	{
		ToolStripItem Menu { get; }
	}

	public class HidMenu : IMainMenuExtension
	{
		private ToolStripMenuItem _menu;
		private readonly IEnumerable<IHidSettingsMenu> _hidSettingsMenus;

		public HidMenu(IEnumerable<IHidSettingsMenu> hidSettingsMenus)
		{
			_hidSettingsMenus = hidSettingsMenus;
		}

		public ToolStripMenuItem Menu
		{
			get
			{
				if (_menu == null)
				{
					_menu = new ToolStripMenuItem("HID Settings");
					foreach (var serialSettingsMenu in _hidSettingsMenus)
						_menu.DropDownItems.Add(serialSettingsMenu.Menu);
				}

				return _menu;
			}
		}
	}
}