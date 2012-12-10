using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminal_GUI_Interface;
using Menu = Terminal_GUI_Interface.Menu;

namespace HyperToken_WinForms_GUI
{
    public class WinformsMainMenuExtender
    {
        private readonly IEnumerable<IMainMenuExtension> _menuExtensions;

        public WinformsMainMenuExtender(IEnumerable<IMainMenuExtension> menuExtensions)
        {
            _menuExtensions = menuExtensions;
        }

        private ToolStripMenuItem MenuToToolStripMenuItem(Menu m)
        {
            var temp = new ToolStripMenuItem(m.Text);

            if(m.Items != null)
                foreach (var item in m.Items)
                    temp.DropDownItems.Add(MenuToToolStripMenuItem(item));

            temp.Checked = m.Checked;
            m.CheckStateChanged += (sender, args) => { if (m.Checked != temp.Checked) temp.Checked = m.Checked; };
            //temp.CheckedChanged += (sender, args) => { if (temp.Checked != m.Checked) m.Checked = temp.Checked; };
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
