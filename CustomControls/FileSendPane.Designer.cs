namespace CustomControls
{
    partial class FileSendPane
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelData = new System.Windows.Forms.Panel();
            this.LabelSendData = new System.Windows.Forms.Label();
            this.panelPreviewData = new System.Windows.Forms.Panel();
            this.buttonPreviewData = new System.Windows.Forms.Button();
            this.panelBrowse = new System.Windows.Forms.Panel();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.panelCharDelay = new System.Windows.Forms.Panel();
            this.numericUpDownCharDelay = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.panelLineDelay = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownLineDelay = new System.Windows.Forms.NumericUpDown();
            this.panelProgressBar = new System.Windows.Forms.Panel();
            this.progressBarSend = new System.Windows.Forms.ProgressBar();
            this.panelSend = new System.Windows.Forms.Panel();
            this.buttonSend = new System.Windows.Forms.Button();
            this.panelCancel = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelData.SuspendLayout();
            this.panelPreviewData.SuspendLayout();
            this.panelBrowse.SuspendLayout();
            this.panelCharDelay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCharDelay)).BeginInit();
            this.panelLineDelay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLineDelay)).BeginInit();
            this.panelProgressBar.SuspendLayout();
            this.panelSend.SuspendLayout();
            this.panelCancel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panelData);
            this.flowLayoutPanel1.Controls.Add(this.panelPreviewData);
            this.flowLayoutPanel1.Controls.Add(this.panelBrowse);
            this.flowLayoutPanel1.Controls.Add(this.panelCharDelay);
            this.flowLayoutPanel1.Controls.Add(this.panelLineDelay);
            this.flowLayoutPanel1.Controls.Add(this.panelProgressBar);
            this.flowLayoutPanel1.Controls.Add(this.panelSend);
            this.flowLayoutPanel1.Controls.Add(this.panelCancel);
            this.flowLayoutPanel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 18);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(179, 239);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // panelData
            // 
            this.panelData.Controls.Add(this.LabelSendData);
            this.panelData.Location = new System.Drawing.Point(3, 3);
            this.panelData.Name = "panelData";
            this.panelData.Size = new System.Drawing.Size(173, 29);
            this.panelData.TabIndex = 2;
            // 
            // LabelSendData
            // 
            this.LabelSendData.AutoSize = true;
            this.LabelSendData.Location = new System.Drawing.Point(8, 8);
            this.LabelSendData.Name = "LabelSendData";
            this.LabelSendData.Size = new System.Drawing.Size(89, 13);
            this.LabelSendData.TabIndex = 1;
            this.LabelSendData.Text = "No file selected...";
            // 
            // panelPreviewData
            // 
            this.panelPreviewData.Controls.Add(this.buttonPreviewData);
            this.panelPreviewData.Location = new System.Drawing.Point(3, 38);
            this.panelPreviewData.Name = "panelPreviewData";
            this.panelPreviewData.Size = new System.Drawing.Size(90, 29);
            this.panelPreviewData.TabIndex = 6;
            this.panelPreviewData.Visible = false;
            // 
            // buttonPreviewData
            // 
            this.buttonPreviewData.Location = new System.Drawing.Point(3, 3);
            this.buttonPreviewData.Name = "buttonPreviewData";
            this.buttonPreviewData.Size = new System.Drawing.Size(84, 23);
            this.buttonPreviewData.TabIndex = 3;
            this.buttonPreviewData.Text = "Preview Data";
            this.buttonPreviewData.UseVisualStyleBackColor = true;
            // 
            // panelBrowse
            // 
            this.panelBrowse.Controls.Add(this.buttonBrowse);
            this.panelBrowse.Location = new System.Drawing.Point(99, 38);
            this.panelBrowse.Name = "panelBrowse";
            this.panelBrowse.Size = new System.Drawing.Size(57, 29);
            this.panelBrowse.TabIndex = 7;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(3, 3);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(51, 23);
            this.buttonBrowse.TabIndex = 1;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // panelCharDelay
            // 
            this.panelCharDelay.Controls.Add(this.numericUpDownCharDelay);
            this.panelCharDelay.Controls.Add(this.label2);
            this.panelCharDelay.Location = new System.Drawing.Point(3, 73);
            this.panelCharDelay.Name = "panelCharDelay";
            this.panelCharDelay.Size = new System.Drawing.Size(170, 28);
            this.panelCharDelay.TabIndex = 0;
            // 
            // numericUpDownCharDelay
            // 
            this.numericUpDownCharDelay.Location = new System.Drawing.Point(114, 3);
            this.numericUpDownCharDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownCharDelay.Name = "numericUpDownCharDelay";
            this.numericUpDownCharDelay.Size = new System.Drawing.Size(51, 20);
            this.numericUpDownCharDelay.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Character Delay (ms)";
            // 
            // panelLineDelay
            // 
            this.panelLineDelay.Controls.Add(this.label3);
            this.panelLineDelay.Controls.Add(this.numericUpDownLineDelay);
            this.panelLineDelay.Location = new System.Drawing.Point(3, 107);
            this.panelLineDelay.Name = "panelLineDelay";
            this.panelLineDelay.Size = new System.Drawing.Size(149, 28);
            this.panelLineDelay.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Line Delay (ms)";
            // 
            // numericUpDownLineDelay
            // 
            this.numericUpDownLineDelay.Location = new System.Drawing.Point(93, 3);
            this.numericUpDownLineDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownLineDelay.Name = "numericUpDownLineDelay";
            this.numericUpDownLineDelay.Size = new System.Drawing.Size(51, 20);
            this.numericUpDownLineDelay.TabIndex = 0;
            // 
            // panelProgressBar
            // 
            this.panelProgressBar.Controls.Add(this.progressBarSend);
            this.panelProgressBar.Location = new System.Drawing.Point(3, 141);
            this.panelProgressBar.Name = "panelProgressBar";
            this.panelProgressBar.Size = new System.Drawing.Size(173, 25);
            this.panelProgressBar.TabIndex = 5;
            this.panelProgressBar.Visible = false;
            // 
            // progressBarSend
            // 
            this.progressBarSend.Location = new System.Drawing.Point(3, 3);
            this.progressBarSend.Name = "progressBarSend";
            this.progressBarSend.Size = new System.Drawing.Size(167, 19);
            this.progressBarSend.TabIndex = 0;
            // 
            // panelSend
            // 
            this.panelSend.Controls.Add(this.buttonSend);
            this.panelSend.Location = new System.Drawing.Point(3, 172);
            this.panelSend.Name = "panelSend";
            this.panelSend.Size = new System.Drawing.Size(63, 29);
            this.panelSend.TabIndex = 3;
            // 
            // buttonSend
            // 
            this.buttonSend.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSend.Location = new System.Drawing.Point(3, 3);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(57, 23);
            this.buttonSend.TabIndex = 0;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // panelCancel
            // 
            this.panelCancel.Controls.Add(this.buttonCancel);
            this.panelCancel.Location = new System.Drawing.Point(72, 172);
            this.panelCancel.Name = "panelCancel";
            this.panelCancel.Size = new System.Drawing.Size(60, 29);
            this.panelCancel.TabIndex = 4;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(3, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(54, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Close";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.buttonClose);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(179, 18);
            this.panel1.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.SystemColors.ControlDark;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonClose.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.buttonClose.FlatAppearance.BorderSize = 3;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Location = new System.Drawing.Point(161, 0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(14, 14);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File Send Options";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Text files|*.txt|Hex files|*.hex|All files|*.*";
            this.openFileDialog1.FilterIndex = 3;
            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.SupportMultiDottedExtensions = true;
            this.openFileDialog1.Title = "Select file to send";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // FileSendPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Name = "FileSendPane";
            this.Size = new System.Drawing.Size(179, 257);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panelData.ResumeLayout(false);
            this.panelData.PerformLayout();
            this.panelPreviewData.ResumeLayout(false);
            this.panelBrowse.ResumeLayout(false);
            this.panelCharDelay.ResumeLayout(false);
            this.panelCharDelay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCharDelay)).EndInit();
            this.panelLineDelay.ResumeLayout(false);
            this.panelLineDelay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLineDelay)).EndInit();
            this.panelProgressBar.ResumeLayout(false);
            this.panelSend.ResumeLayout(false);
            this.panelCancel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panelCharDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownCharDelay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelLineDelay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownLineDelay;
        private System.Windows.Forms.Panel panelData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelSendData;
        private System.Windows.Forms.Panel panelSend;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Panel panelCancel;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panelProgressBar;
        private System.Windows.Forms.ProgressBar progressBarSend;
        private System.Windows.Forms.Panel panelPreviewData;
        private System.Windows.Forms.Button buttonPreviewData;
        private System.Windows.Forms.Panel panelBrowse;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonClose;
    }
}
