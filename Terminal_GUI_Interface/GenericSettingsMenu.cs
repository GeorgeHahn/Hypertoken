using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Terminal_GUI_Interface
{
	public abstract class GenericSettingsMenu
	{
		protected ToolStripMenuItem _menu;

		protected abstract dynamic Values { get; }

		protected abstract string MenuName { get; }

		protected abstract dynamic ItemValue { get; set; }

		protected abstract string PropertyName { get; }

		protected virtual bool UpdateOnOpen { get { return false; } }

		public ToolStripItem Menu
		{
			get
			{
				if (_menu == null)
				{
					_menu = new ToolStripMenuItem(MenuName);
					_menu.DropDownItemClicked += (sender, args) => ItemClicked(_menu.DropDownItems.IndexOf(args.ClickedItem));
					if (UpdateOnOpen)
						_menu.DropDownOpening += (sender, args) => SetItems();
					SetItems();
				}

				return _menu;
			}
		}

		private void SetItems()
		{
			_menu.DropDownItems.Clear();

			foreach (var value in Values)
				_menu.DropDownItems.Add(value.ToString());

			UpdateCheckedStates(PropertyName);
		}

		protected void ItemClicked(int item)
		{
			ItemValue = Values[item];
		}

		protected void UpdateCheckedStates(string propertyName)
		{
			if (propertyName != PropertyName) return;

			foreach (ToolStripMenuItem menu in _menu.DropDownItems)
			{
				menu.Checked = menu.Text == ItemValue.ToString();
			}
		}
	}
}