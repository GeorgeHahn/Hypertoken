using System.Windows.Forms;

namespace Terminal.Interface.GUI
{
	public interface IStatusbarExtension
	{
		ToolStripItem StatusBarItem { get; } // TODO: This should be generic and the winforms gui should wrap it
	}
}