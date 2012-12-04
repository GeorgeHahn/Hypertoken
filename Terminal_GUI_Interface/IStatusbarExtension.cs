using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Terminal_GUI_Interface
{
	public interface IStatusbarExtension
	{
		ToolStripStatusLabel StatusBarItem { get; }
	}
}