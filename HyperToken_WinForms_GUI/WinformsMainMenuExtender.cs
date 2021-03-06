﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Anotar.NLog;
using Terminal.Interface.GUI;

namespace HyperToken.WinForms
{
    public class WinformsMainMenuExtender
    {
        private readonly IEnumerable<IMainMenuExtension> _menuExtensions;

        public WinformsMainMenuExtender(IEnumerable<IMainMenuExtension> menuExtensions)
        {
            _menuExtensions = menuExtensions;
        }

        private ToolStripMenuItem MenuToToolStripMenuItem(LightMenu m)
        {
            var temp = new ToolStripMenuItem(m.Text);

            if (m.Items != null)
            {
                m.PropertyChanged += (sender, args) =>
                                        {
                                            LogTo.Debug("Property changed: {0}", args.PropertyName);
                                            if (args.PropertyName != "Items")
                                                return;

                                            LogTo.Debug("Regenerating ToolStripMenuItems for menu {0}", m.Text);

                                            temp.DropDownItems.Clear();
                                            foreach (var item in m.Items)
                                                temp.DropDownItems.Add(MenuToToolStripMenuItem(item));
                                        };

                if(m.PullRight)
                    temp.Alignment = ToolStripItemAlignment.Right;

                foreach (var item in m.Items)
                    temp.DropDownItems.Add(MenuToToolStripMenuItem(item));
            }

            temp.Click += m.FireClicked;
            temp.Checked = m.Checked;
            if (m.Highlight)
                temp.BackColor = Color.Aqua;

            m.PropertyChanged += (sender, args) =>
                                     {
                                         LogTo.Debug("Property changed: {0}", args.PropertyName);
                                         if (args.PropertyName != "Checked")
                                             return;
                                         if (m.Checked != temp.Checked)
                                             temp.Checked = m.Checked;
                                     };
            temp.DropDownItemClicked += (sender, args) => m.FireItemClicked(sender, args.ClickedItem.Text);
            temp.DropDownOpening += m.FireItemListOpening;

            return temp;
        }

        public IEnumerable<ToolStripMenuItem> GetMenus()
        {
            return _menuExtensions.Select(mainMenuExtension => MenuToToolStripMenuItem(mainMenuExtension.Menu));
        }
    }
}