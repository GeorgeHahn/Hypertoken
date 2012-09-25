using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sniffer
{
    public partial class HexPane : UserControl, DataPane
    {
        public HexPane()
        {
            InitializeComponent();
        }

        [System.Runtime.InteropServices.DllImport("User32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, EntryPoint = "SendMessage")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        const int WM_VSCROLL = 277;
        const int SB_BOTTOM = 7;

        private void ScrollToEnd(RichTextBox rtb)
        {
            IntPtr ptrWparam = new IntPtr(SB_BOTTOM);
            IntPtr ptrLparam = new IntPtr(0);
            SendMessage(rtb.Handle, WM_VSCROLL, ptrWparam, ptrLparam);
        } 

        public void AddData(long[] timestamps, byte[] addr, byte[][] data)
        {
            for(int i = 0; i < timestamps.Length; i++)
            {
                DateTime timestamp = new DateTime(timestamps[i]);
                String s = "[" + timestamp.Hour.ToString() + ":" + timestamp.Minute.ToString() + ":" + timestamp.Second.ToString() + "." + timestamp.Millisecond.ToString("d3") + "]";
                s += ": 0x";
                s += addr[i].ToString("X2");
                s += ": ";

                for (int j = 0; j < data[i].Length; j++)
                    s += " " + data[i][j].ToString("X2");

                s += Environment.NewLine;
                IOBox.BeginInvoke(new MethodInvoker(
                    delegate
                    {
                        IOBox.AppendText(s);
                    }));
            }

            ScrollToEnd(IOBox);
            /*IOBox.BeginInvoke(new MethodInvoker(
                delegate
                {
                    IOBox.SelectionStart = IOBox.Text.Length - 1;
                    IOBox.ScrollToCaret();
                }));*/
        }
    }
}
