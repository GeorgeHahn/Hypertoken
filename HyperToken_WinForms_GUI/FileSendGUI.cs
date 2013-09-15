using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Terminal_GUI_Interface;
using Terminal_Interface;
using Terminal_Interface.Enums;

namespace HyperToken.WinFormsGUI
{
    public class FileSendGUI : IToolbarExtension
    {
        private ToolStripItem _toolBarItem;

        public ToolStripItem ToolBarItem
        {
            get
            {
                if(_toolBarItem != null)
                    return _toolBarItem;
                else
                {
                    return new ToolStripButton("Send File");
                }
            }
        }
    }
}
