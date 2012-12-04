using System;
using CustomControls;
using ScintillaNET;

namespace HyperToken_WinForms_GUI
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
			this.serialSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemCOMPort = new System.Windows.Forms.ToolStripMenuItem();
			this.noSerialPortsFoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemBaud = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemDataBits = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemParity = new System.Windows.Forms.ToolStripMenuItem();
			this.evenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.oddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.markToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.spaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemStopBits = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemFlowControl = new System.Windows.Forms.ToolStripMenuItem();
			this.noneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.requestToSendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.xonXoffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rTSXonXoffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemLineDelay = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemCharacterDelay = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
			this.ConnectionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.dropDownCOMPort = new System.Windows.Forms.ToolStripDropDownButton();
			this.dropDownBaud = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripStatusLabelLocalEcho = new PretendStatusbarButton();
			this.statusStrip = new StatusStripEx();
			this.toolStripStatusLabelPortSettings = new System.Windows.Forms.ToolStripStatusLabel();
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
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.serialSettingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(670, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip";
			// 
			// serialSettingsToolStripMenuItem
			// 
			this.serialSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCOMPort,
            this.menuItemBaud,
            this.menuItemDataBits,
            this.menuItemParity,
            this.menuItemStopBits,
            this.menuItemFlowControl,
            this.menuItemLineDelay,
            this.menuItemCharacterDelay});
			this.serialSettingsToolStripMenuItem.Name = "serialSettingsToolStripMenuItem";
			this.serialSettingsToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
			this.serialSettingsToolStripMenuItem.Text = "Serial Settings";
			// 
			// menuItemCOMPort
			// 
			this.menuItemCOMPort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noSerialPortsFoundToolStripMenuItem});
			this.menuItemCOMPort.Name = "menuItemCOMPort";
			this.menuItemCOMPort.Size = new System.Drawing.Size(184, 22);
			this.menuItemCOMPort.Text = "COM Port";
			this.menuItemCOMPort.DropDownOpening += new System.EventHandler(this.UpdateCOMPorts);
			this.menuItemCOMPort.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.SelectCOMPort);
			// 
			// noSerialPortsFoundToolStripMenuItem
			// 
			this.noSerialPortsFoundToolStripMenuItem.Name = "noSerialPortsFoundToolStripMenuItem";
			this.noSerialPortsFoundToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
			this.noSerialPortsFoundToolStripMenuItem.Text = "No serial ports found!";
			// 
			// menuItemBaud
			// 
			this.menuItemBaud.Name = "menuItemBaud";
			this.menuItemBaud.Size = new System.Drawing.Size(184, 22);
			this.menuItemBaud.Text = "Baud";
			// 
			// menuItemDataBits
			// 
			this.menuItemDataBits.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
			this.menuItemDataBits.Name = "menuItemDataBits";
			this.menuItemDataBits.Size = new System.Drawing.Size(184, 22);
			this.menuItemDataBits.Text = "Data Bits";
			this.menuItemDataBits.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.HandleCOMParamChange);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(80, 22);
			this.toolStripMenuItem2.Text = "5";
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(80, 22);
			this.toolStripMenuItem3.Text = "6";
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(80, 22);
			this.toolStripMenuItem4.Text = "7";
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(80, 22);
			this.toolStripMenuItem5.Text = "8";
			// 
			// menuItemParity
			// 
			this.menuItemParity.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.evenToolStripMenuItem,
            this.oddToolStripMenuItem,
            this.noneToolStripMenuItem,
            this.markToolStripMenuItem,
            this.spaceToolStripMenuItem});
			this.menuItemParity.Name = "menuItemParity";
			this.menuItemParity.Size = new System.Drawing.Size(184, 22);
			this.menuItemParity.Text = "Parity";
			this.menuItemParity.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.HandleCOMParamChange);
			// 
			// evenToolStripMenuItem
			// 
			this.evenToolStripMenuItem.Name = "evenToolStripMenuItem";
			this.evenToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.evenToolStripMenuItem.Text = "Even";
			// 
			// oddToolStripMenuItem
			// 
			this.oddToolStripMenuItem.Name = "oddToolStripMenuItem";
			this.oddToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.oddToolStripMenuItem.Text = "Odd";
			// 
			// noneToolStripMenuItem
			// 
			this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
			this.noneToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.noneToolStripMenuItem.Text = "None";
			// 
			// markToolStripMenuItem
			// 
			this.markToolStripMenuItem.Name = "markToolStripMenuItem";
			this.markToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.markToolStripMenuItem.Text = "Mark";
			// 
			// spaceToolStripMenuItem
			// 
			this.spaceToolStripMenuItem.Name = "spaceToolStripMenuItem";
			this.spaceToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.spaceToolStripMenuItem.Text = "Space";
			// 
			// menuItemStopBits
			// 
			this.menuItemStopBits.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8});
			this.menuItemStopBits.Name = "menuItemStopBits";
			this.menuItemStopBits.Size = new System.Drawing.Size(184, 22);
			this.menuItemStopBits.Text = "Stop Bits";
			this.menuItemStopBits.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.HandleCOMParamChange);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(89, 22);
			this.toolStripMenuItem6.Text = "1";
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(89, 22);
			this.toolStripMenuItem7.Text = "1.5";
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(89, 22);
			this.toolStripMenuItem8.Text = "2";
			// 
			// menuItemFlowControl
			// 
			this.menuItemFlowControl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem1,
            this.requestToSendToolStripMenuItem,
            this.xonXoffToolStripMenuItem,
            this.rTSXonXoffToolStripMenuItem});
			this.menuItemFlowControl.Name = "menuItemFlowControl";
			this.menuItemFlowControl.Size = new System.Drawing.Size(184, 22);
			this.menuItemFlowControl.Text = "Flow Control";
			this.menuItemFlowControl.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.HandleCOMParamChange);
			// 
			// noneToolStripMenuItem1
			// 
			this.noneToolStripMenuItem1.Name = "noneToolStripMenuItem1";
			this.noneToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
			this.noneToolStripMenuItem1.Text = "None";
			// 
			// requestToSendToolStripMenuItem
			// 
			this.requestToSendToolStripMenuItem.Name = "requestToSendToolStripMenuItem";
			this.requestToSendToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.requestToSendToolStripMenuItem.Text = "Request To Send";
			// 
			// xonXoffToolStripMenuItem
			// 
			this.xonXoffToolStripMenuItem.Name = "xonXoffToolStripMenuItem";
			this.xonXoffToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.xonXoffToolStripMenuItem.Text = "Xon/Xoff";
			// 
			// rTSXonXoffToolStripMenuItem
			// 
			this.rTSXonXoffToolStripMenuItem.Name = "rTSXonXoffToolStripMenuItem";
			this.rTSXonXoffToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.rTSXonXoffToolStripMenuItem.Text = "RTS + Xon/Xoff";
			// 
			// menuItemLineDelay
			// 
			this.menuItemLineDelay.Name = "menuItemLineDelay";
			this.menuItemLineDelay.Size = new System.Drawing.Size(184, 22);
			this.menuItemLineDelay.Text = "Line Delay (ms)";
			this.menuItemLineDelay.Visible = false;
			// 
			// menuItemCharacterDelay
			// 
			this.menuItemCharacterDelay.Name = "menuItemCharacterDelay";
			this.menuItemCharacterDelay.Size = new System.Drawing.Size(184, 22);
			this.menuItemCharacterDelay.Text = "Character Delay (ms)";
			this.menuItemCharacterDelay.Visible = false;
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.ShowAboutForm);
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
			this.toolStripButtonConnect.Image = HyperToken_WinForms_GUI.Properties.Resources.connected;
			this.toolStripButtonConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonConnect.Name = "toolStripButtonConnect";
			this.toolStripButtonConnect.Size = new System.Drawing.Size(72, 22);
			this.toolStripButtonConnect.Text = "Connect";
			this.toolStripButtonConnect.Click += new System.EventHandler(this.ToggleConnection);
			// 
			// toolStripButtonSendFile
			// 
			this.toolStripButtonSendFile.Image = HyperToken_WinForms_GUI.Properties.Resources.FileSend;
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
			// ConnectionStatusLabel
			// 
			this.ConnectionStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.ConnectionStatusLabel.Name = "ConnectionStatusLabel";
			this.ConnectionStatusLabel.Size = new System.Drawing.Size(79, 19);
			this.ConnectionStatusLabel.Text = "Disconnected";
			this.ConnectionStatusLabel.Visible = false;
			// 
			// dropDownCOMPort
			// 
			this.dropDownCOMPort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.dropDownCOMPort.Image = ((System.Drawing.Image)(resources.GetObject("dropDownCOMPort.Image")));
			this.dropDownCOMPort.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.dropDownCOMPort.Name = "dropDownCOMPort";
			this.dropDownCOMPort.Size = new System.Drawing.Size(54, 22);
			this.dropDownCOMPort.Text = "COM1";
			this.dropDownCOMPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.dropDownCOMPort.DropDownOpening += new System.EventHandler(this.UpdateCOMPorts);
			this.dropDownCOMPort.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.SelectCOMPort);
			// 
			// dropDownBaud
			// 
			this.dropDownBaud.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.dropDownBaud.Image = ((System.Drawing.Image)(resources.GetObject("dropDownBaud.Image")));
			this.dropDownBaud.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.dropDownBaud.Name = "dropDownBaud";
			this.dropDownBaud.Size = new System.Drawing.Size(86, 22);
			this.dropDownBaud.Text = "115200 Baud";
			// 
			// toolStripStatusLabelLocalEcho
			// 
			this.toolStripStatusLabelLocalEcho.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.toolStripStatusLabelLocalEcho.Name = "toolStripStatusLabelLocalEcho";
			this.toolStripStatusLabelLocalEcho.Size = new System.Drawing.Size(57, 19);
			this.toolStripStatusLabelLocalEcho.Text = "Echo Off";
			this.toolStripStatusLabelLocalEcho.ToolTipText = "Click to toggle";
			this.toolStripStatusLabelLocalEcho.Click += new System.EventHandler(this.ToggleEcho);
			// 
			// statusStrip
			// 
			this.statusStrip.ClickThrough = true;
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectionStatusLabel,
            this.dropDownCOMPort,
            this.dropDownBaud,
            this.toolStripStatusLabelPortSettings,
            this.toolStripStatusLabelLocalEcho,
            this.fileSendLoadingCircle});
			this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.statusStrip.Location = new System.Drawing.Point(0, 366);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(670, 24);
			this.statusStrip.TabIndex = 0;
			this.statusStrip.Text = "statusStrip";
			// 
			// toolStripStatusLabelPortSettings
			// 
			this.toolStripStatusLabelPortSettings.Name = "toolStripStatusLabelPortSettings";
			this.toolStripStatusLabelPortSettings.Size = new System.Drawing.Size(34, 19);
			this.toolStripStatusLabelPortSettings.Text = "8;N;1";
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
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveEntireSessionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem serialSettingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem menuItemCOMPort;
		private System.Windows.Forms.ToolStripMenuItem menuItemBaud;
		private System.Windows.Forms.ToolStripMenuItem menuItemDataBits;
		private System.Windows.Forms.ToolStripMenuItem menuItemParity;
		private System.Windows.Forms.ToolStripMenuItem menuItemStopBits;
		private System.Windows.Forms.ToolStripMenuItem menuItemFlowControl;
		private System.Windows.Forms.ToolStripMenuItem menuItemLineDelay;
		private System.Windows.Forms.ToolStripMenuItem menuItemCharacterDelay;
		//private System.Windows.Forms.RichTextBox IOBox;
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
		private System.Windows.Forms.ToolStripStatusLabel ConnectionStatusLabel;
		public System.Windows.Forms.ToolStripDropDownButton dropDownCOMPort;
		public System.Windows.Forms.ToolStripDropDownButton dropDownBaud;
		private PretendStatusbarButton toolStripStatusLabelLocalEcho;
		private MenuStripEx menuStrip1;
		private ToolStripEx toolStrip1;
		private StatusStripEx statusStrip;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private System.Windows.Forms.ToolStripMenuItem noSerialPortsFoundToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPortSettings;
		private MRG.Controls.UI.LoadingCircleToolStripMenuItem fileSendLoadingCircle;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem evenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem oddToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem markToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem spaceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
		private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem requestToSendToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem xonXoffToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rTSXonXoffToolStripMenuItem;
	}
}