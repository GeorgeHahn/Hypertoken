using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomControls
{
    public partial class FileSendPane : UserControl
    {
        private byte[] _data;
        private bool isFile = false;
        private string filePath = string.Empty;

        #region Public Interface
        public FileSendPane()
        {
            InitializeComponent();
            
            
        }

        public struct SerialSendArgs
        {
            public byte[] data;
            public int LineDelay;
            public int CharDelay;
        }

        public bool SendFile(string file)
        {
            if (!System.IO.File.Exists(file))
                return false;

            isFile = true;
            filePath = file;

            LabelSendData.ForeColor = SystemColors.ControlText;

            Visible = true;
            if (LabelSendData != null)
                LabelSendData.Text = file.Substring(file.LastIndexOf('\\'));

            return true;
        }

        public bool SendData(byte[] data, string caption)
        {
            if (data.Length == 0)
                return false;

            isFile = false;
            filePath = string.Empty;

            _data = data;
            LabelSendData.Text = caption;
            LabelSendData.ForeColor = SystemColors.ControlText;
            Visible = true;
            return true;
        }

        new public void Hide()
        {
            Visible = false;
        }
        #endregion

        #region Event Definitions
        public delegate void OnFileSendEventHandler(object sender, System.EventArgs e, SerialSendArgs a);
        public event OnFileSendEventHandler OnSend;
        #endregion Event Definitions

        #region Click handlers
        private void buttonSend_Click(object sender, EventArgs e)
        {
            System.EventArgs p = new System.EventArgs();
            SerialSendArgs a;
            a.data = null;

            if (isFile)
            {
                try
                {
                    a.data = System.IO.File.ReadAllBytes(filePath);
                }
                catch (Exception exc)
                {
                    LabelSendData.ForeColor = Color.Red;
                    LabelSendData.Text = exc.Message;
                }
            }
            else
                a.data = _data;

            if ((a.data != null) && (a.data.Length == 0))
                return;

            a.LineDelay = (int)numericUpDownLineDelay.Value;
            a.CharDelay = (int)numericUpDownCharDelay.Value;
            OnSend (this, p, a);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            LabelSendData.ForeColor = SystemColors.ControlText;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if(!string.IsNullOrEmpty (openFileDialog1.FileName))
                SendFile (openFileDialog1.FileName);
        }
        #endregion
    }
}
