using System;
using System.Windows.Forms;

namespace Sniffer
{
	/// <summary>
	/// Datapane that displays data in ASCII format
	/// </summary>
	public partial class ASCIIPane : UserControl, DataPane
	{
		public ASCIIPane()
		{
			InitializeComponent();
		}

		[System.Runtime.InteropServices.DllImport("User32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, EntryPoint = "SendMessage")]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		const int WM_VSCROLL = 277;
		const int SB_BOTTOM = 7;
		DateTime timestamp;

		/// <summary>
		/// Scrolls to the end of a richtextbox
		/// </summary>
		/// <param name="rtb">RTB to scroll</param>
		private void ScrollToEnd(RichTextBox rtb)
		{
			IntPtr ptrWparam = new IntPtr(SB_BOTTOM);
			IntPtr ptrLparam = new IntPtr(0);
			SendMessage(rtb.Handle, WM_VSCROLL, ptrWparam, ptrLparam);
		}

		/// <summary>
		/// Add multiple lines to the pane
		/// Note: Threadsafe
		/// </summary>
		/// <param name="timestamps">Timestamp for each line</param>
		/// <param name="addr">Address for each line</param>
		/// <param name="data">Data array for each line</param>
		public void AddData(long[] timestamps, byte[] addr, byte[][] data)
		{
			for (int i = 0; i < timestamps.Length; i++)
			{
				timestamp = new DateTime(timestamps[i]);
				String s = "[" + timestamp.Hour.ToString() + ":" + timestamp.Minute.ToString() + ":" + timestamp.Second.ToString() + "." + timestamp.Millisecond.ToString("d3") + "]";
				s += ": ";
				s += addr[i].ToString();
				s += ": ";

				s += System.Text.UnicodeEncoding.ASCII.GetString(data[i], 0, data[i].Length);

				s += Environment.NewLine;

				IOBox.BeginInvoke(new MethodInvoker(
					delegate
					{
						IOBox.AppendText(s);
					}));
			}

			ScrollToEnd(IOBox);
		}

		/// <summary>
		/// Add text to the pane
		/// Note: Threadsafe
		/// </summary>
		/// <param name="s">String to add</param>
		public void AddRawText(string s)
		{
			IOBox.BeginInvoke(new MethodInvoker(
					delegate
					{
						IOBox.AppendText(s);
					}));
		}

		/// <summary>
		/// Add text to the pane, preceeded by a timestamp
		/// Note: Threadsafe
		/// </summary>
		/// <param name="s">String to add</param>
		public void AddText(string s)
		{
			timestamp = DateTime.Now;
			String stamp = "[" + timestamp.Hour.ToString() + ":" + timestamp.Minute.ToString() + ":" + timestamp.Second.ToString() + "." + timestamp.Millisecond.ToString("d3") + "]";
			s = stamp + s;
			AddRawText(s);
		}
	}
}