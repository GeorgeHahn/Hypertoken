﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Anotar;
using Anotar.NLog;
using NLog;

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

                    //_menu.PropertyChanged += (sender, args) => UpdateCheckedStates(args.PropertyName);
                    if (UpdateOnOpen)
                        _menu.ItemsListOpening += (sender, args) => SetItems();
                    SetItems();
                }

                return _menu;
            }
        }

        private void SetItems()
        {
            LogTo.Debug("Setting items for menu {0}", MenuName);
            _menu.Items.Clear();

            foreach (var value in Values)
                _menu.Items.Add(new Menu(value.ToString()));

            UpdateCheckedStates(PropertyName);

            _menu.FirePropertyChanged(this, new PropertyChangedEventArgs("Items"));
        }

        protected void ItemClicked(int item)
        {
            ItemValue = Values[item];
        }

        protected void UpdateCheckedStates(string propertyName)
        {
            if (propertyName != PropertyName)
            {
                LogTo.Debug("Ignoring {0} on {1}", propertyName, PropertyName);
                return;
            }

            foreach (var menuItem in _menu.Items)
            {
                menuItem.Checked = menuItem.Text == ItemValue.ToString();
            }
        }
    }
}