using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Terminal_GUI_Interface
{
	public abstract class GenericSettingsMenu
	{
		protected Menu _menu;

		protected abstract dynamic Values { get; }

		protected abstract string MenuName { get; }

		protected abstract dynamic ItemValue { get; set; }

		protected abstract string PropertyName { get; }

		protected virtual bool UpdateOnOpen { get { return false; } }

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

		private void SetItems()
		{
			_menu.Items.Clear();

			foreach (var value in Values)
				_menu.Items.Add(new Menu(value.ToString()));

			UpdateCheckedStates(PropertyName);
		}

		protected void ItemClicked(int item)
		{
			ItemValue = Values[item];
		}

		protected void UpdateCheckedStates(string propertyName)
		{
			if (propertyName != PropertyName) return;

			foreach (var menuItem in _menu.Items)
			{
				menuItem.Checked = menuItem.Text == ItemValue.ToString();
			}
		}
	}
}