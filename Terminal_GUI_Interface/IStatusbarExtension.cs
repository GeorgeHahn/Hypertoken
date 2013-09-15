using System.Windows.Forms;

namespace Terminal.Interface.GUI
{
	public interface IStatusbarExtension
	{
		ToolStripItem StatusBarItem { get; }
	}
}