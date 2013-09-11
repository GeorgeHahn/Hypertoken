using System.Windows.Forms;

namespace Terminal_GUI_Interface
{
    public interface IToolbarExtension
    {
        ToolStripItem ToolBarItem { get; }
    }
}