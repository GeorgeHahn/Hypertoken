using System.Windows.Forms;
using Terminal.Interface.GUI;

namespace HyperToken.WinForms
{
    public class FileSendGUI : IToolbarExtension
    {
        private ToolStripItem _toolBarItem;

        public ToolStripItem ToolBarItem
        {
            get
            {
                return new ToolStripButton("Send File");
            }
        }
    }
}
