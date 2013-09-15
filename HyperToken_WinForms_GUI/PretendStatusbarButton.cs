using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HyperToken.WinFormsGUI
{
	internal class PretendStatusbarButton : ToolStripStatusLabel
	{
		public PretendStatusbarButton(string text, Image image, EventHandler onClick, String name)
			: base(text, image, onClick, name)
		{
			MouseDown += PretendClick;
			MouseUp += PretendRelease;

			MouseEnter += PretendEnter;
			MouseLeave += PretendLeave;

			BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
		}

		public PretendStatusbarButton(string text, Image image, EventHandler onClick)
			: this(text, image, onClick, null)
		{ }

		public PretendStatusbarButton(string text, Image image)
			: this(text, image, null, null)
		{ }

		public PretendStatusbarButton(string text)
			: this(text, null, null, null)
		{ }

		public PretendStatusbarButton(Image image)
			: this(null, image, null, null)
		{ }

		public PretendStatusbarButton()
			: this(null, null, null, null)
		{ }

		private void PretendClick(object sender, MouseEventArgs e)
		{
			BorderStyle = Border3DStyle.SunkenInner;
		}

		private void PretendEnter(object sender, EventArgs e)
		{
			BorderStyle = Border3DStyle.RaisedInner;
		}

		private void PretendLeave(object sender, EventArgs e)
		{
			BorderStyle = Border3DStyle.Flat;
		}

		private void PretendRelease(object sender, MouseEventArgs e)
		{
			BorderStyle = Border3DStyle.Flat;
		}
	}
}