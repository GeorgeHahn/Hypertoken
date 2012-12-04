using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HyperToken_WinForms_GUI
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

		private static void PretendClick(object sender, MouseEventArgs e)
		{
			if (sender is ToolStripStatusLabel)
				((ToolStripStatusLabel)sender).BorderStyle = Border3DStyle.SunkenInner;
		}

		private static void PretendEnter(object sender, EventArgs e)
		{
			if (sender is ToolStripStatusLabel)
				((ToolStripStatusLabel)sender).BorderStyle = Border3DStyle.RaisedInner;
		}

		private static void PretendLeave(object sender, EventArgs e)
		{
			if (sender is ToolStripStatusLabel)
				((ToolStripStatusLabel)sender).BorderStyle = Border3DStyle.Flat;
		}

		private static void PretendRelease(object sender, MouseEventArgs e)
		{
			if (sender is ToolStripStatusLabel)
				((ToolStripStatusLabel)sender).BorderStyle = Border3DStyle.Flat;
		}
	}
}