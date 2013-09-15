using System;
using CustomControls;
using ScintillaNET;

namespace HyperToken.WinFormsGUI
{
	partial class MainForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveEntireSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1 = new MenuStripEx();
			this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
			
			//this.IOBox = new System.Windows.Forms.RichTextBox();
			this.IOBox = new Scintilla();
			this.toolStrip1 = new ToolStripEx();
			this.toolStripButtonConnect = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonSendFile = new System.Windows.Forms.ToolStripButton();
			this.fileSendPane1 = new CustomControls.FileSendPane();
			this.statusStrip = new StatusStripEx();
			this.fileSendLoadingCircle = new MRG.Controls.UI.LoadingCircleToolStripMenuItem();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.menuStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveEntireSessionToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.newToolStripMenuItem.Text = "New Session...";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// saveEntireSessionToolStripMenuItem
			// 
			this.saveEntireSessionToolStripMenuItem.Name = "saveEntireSessionToolStripMenuItem";
			this.saveEntireSessionToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.saveEntireSessionToolStripMenuItem.Text = "Save Entire Session...";
			this.saveEntireSessionToolStripMenuItem.Visible = false;
			this.saveEntireSessionToolStripMenuItem.Click += new System.EventHandler(this.SaveEntireSessionToolStripMenuItemClick);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.Exit);
			// 
			// menuStrip1
			// 
			this.menuStrip1.ClickThrough = true;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(670, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip";
			// 
			// BottomToolStripPanel
			// 
			this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.BottomToolStripPanel.Name = "BottomToolStripPanel";
			this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// TopToolStripPanel
			// 
			this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.TopToolStripPanel.Name = "TopToolStripPanel";
			this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// RightToolStripPanel
			// 
			this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.RightToolStripPanel.Name = "RightToolStripPanel";
			this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// LeftToolStripPanel
			// 
			this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.LeftToolStripPanel.Name = "LeftToolStripPanel";
			this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// ContentPanel
			// 
			this.ContentPanel.Size = new System.Drawing.Size(125, 150);
			// 
			// IOBox
			// 



			this.IOBox.AcceptsTab = true;
			//this.IOBox.DetectUrls = false;
			this.IOBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.IOBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IOBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.IOBox.Location = new System.Drawing.Point(0, 49);
			this.IOBox.Name = "IOBox";
			//this.IOBox.ShortcutsEnabled = false;
			this.IOBox.Size = new System.Drawing.Size(491, 317);
			this.IOBox.TabIndex = 3;
			this.IOBox.Text = "";
			this.IOBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleKeyPress);
			this.IOBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SendKey);
			// 
			// toolStrip1
			// 
			this.toolStrip1.ClickThrough = true;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonConnect,
            this.toolStripButtonSendFile});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(670, 25);
			this.toolStrip1.TabIndex = 5;
			// 
			// toolStripButtonConnect
			// 
			this.toolStripButtonConnect.Image = WinFormsGUI.Properties.Resources.connected;
			this.toolStripButtonConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonConnect.Name = "toolStripButtonConnect";
			this.toolStripButtonConnect.Size = new System.Drawing.Size(72, 22);
			this.toolStripButtonConnect.Text = "Connect";
			this.toolStripButtonConnect.Click += new System.EventHandler(this.ToggleConnection);
			// 
			// toolStripButtonSendFile
			// 
			this.toolStripButtonSendFile.Image = WinFormsGUI.Properties.Resources.FileSend;
			this.toolStripButtonSendFile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSendFile.Name = "toolStripButtonSendFile";
			this.toolStripButtonSendFile.Size = new System.Drawing.Size(74, 22);
			this.toolStripButtonSendFile.Text = "Send File";
			this.toolStripButtonSendFile.ToolTipText = "Send File";
			this.toolStripButtonSendFile.Click += new System.EventHandler(this.toolStripButtonSendFile_Click);
			// 
			// fileSendPane1
			// 
			this.fileSendPane1.Dock = System.Windows.Forms.DockStyle.Right;
			this.fileSendPane1.Location = new System.Drawing.Point(491, 49);
			this.fileSendPane1.Name = "fileSendPane1";
			this.fileSendPane1.Size = new System.Drawing.Size(179, 317);
			this.fileSendPane1.TabIndex = 4;
			this.fileSendPane1.Visible = false;
			this.fileSendPane1.OnSend += new CustomControls.FileSendPane.OnFileSendEventHandler(FileSendPaneSend);
			// 
			// statusStrip
			// 
			this.statusStrip.ClickThrough = true;
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileSendLoadingCircle});
			this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.statusStrip.Location = new System.Drawing.Point(0, 366);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(670, 24);
			this.statusStrip.TabIndex = 0;
			this.statusStrip.Text = "statusStrip";
			// 
			// fileSendLoadingCircle
			// 
			this.fileSendLoadingCircle.AutoSize = false;
			this.fileSendLoadingCircle.AutoToolTip = true;
			// 
			// fileSendLoadingCircle
			// 
			this.fileSendLoadingCircle.LoadingCircleControl.AccessibleName = "toolStripStatusLabelSendingFile";
			this.fileSendLoadingCircle.LoadingCircleControl.Active = false;
			this.fileSendLoadingCircle.LoadingCircleControl.Color = System.Drawing.SystemColors.HotTrack;
			this.fileSendLoadingCircle.LoadingCircleControl.InnerCircleRadius = 5;
			this.fileSendLoadingCircle.LoadingCircleControl.Location = new System.Drawing.Point(414, 4);
			this.fileSendLoadingCircle.LoadingCircleControl.Name = "toolStripStatusLabelSendingFile";
			this.fileSendLoadingCircle.LoadingCircleControl.NumberSpoke = 12;
			this.fileSendLoadingCircle.LoadingCircleControl.OuterCircleRadius = 11;
			this.fileSendLoadingCircle.LoadingCircleControl.RotationSpeed = 100;
			this.fileSendLoadingCircle.LoadingCircleControl.Size = new System.Drawing.Size(15, 15);
			this.fileSendLoadingCircle.LoadingCircleControl.SpokeThickness = 2;
			this.fileSendLoadingCircle.LoadingCircleControl.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
			this.fileSendLoadingCircle.LoadingCircleControl.TabIndex = 1;
			this.fileSendLoadingCircle.LoadingCircleControl.Text = "Not currently sending a file";
			this.fileSendLoadingCircle.Name = "fileSendLoadingCircle";
			this.fileSendLoadingCircle.Size = new System.Drawing.Size(15, 15);
			this.fileSendLoadingCircle.Text = "Not currently sending a file";
			this.fileSendLoadingCircle.Visible = false;
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
			this.vScrollBar1.LargeChange = 1;
			this.vScrollBar1.Location = new System.Drawing.Point(474, 49);
			this.vScrollBar1.Maximum = 0;
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(17, 317);
			this.vScrollBar1.TabIndex = 6;
			this.vScrollBar1.Visible = false;
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(670, 390);
			this.Controls.Add(this.vScrollBar1);
			this.Controls.Add(this.IOBox);
			this.Controls.Add(this.fileSendPane1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.statusStrip);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(358, 300);
			this.Name = "MainForm";
			this.Text = "Hypertoken";
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.SendDroppedText);
			this.DragOver += new System.Windows.Forms.DragEventHandler(this.SendDroppedText);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}


		#endregion Windows Form Designer generated code

		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveEntireSessionToolStripMenuItem;
		private Scintilla IOBox;
		private CustomControls.FileSendPane fileSendPane1;
		private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
		private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
		private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
		private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
		private System.Windows.Forms.ToolStripContentPanel ContentPanel;
		private System.Windows.Forms.ToolStripButton toolStripButtonConnect;
		private System.Windows.Forms.ToolStripButton toolStripButtonSendFile;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private MenuStripEx menuStrip1;
		private ToolStripEx toolStrip1;
		private StatusStripEx statusStrip;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private MRG.Controls.UI.LoadingCircleToolStripMenuItem fileSendLoadingCircle;
	}
}