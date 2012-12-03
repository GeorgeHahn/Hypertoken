using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using HyperToken_WinForms_GUI.Helpers;
using HyperToken_WinForms_GUI.Properties;
using NLog;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;
using Terminal_Interface.Exceptions;

// TODO tweak garbage collection
// TODO Custom Baud setting
// TODO right click menu
// - Copy, Save selection, Send file
// TODO Parse incoming data for unprintable characters (display as hex?)

namespace HyperToken_WinForms_GUI
{
	public partial class MainForm : Form, ITerminal, INotifyPropertyChanged
	{
		#region Initialization

		private void SetMonospacedFont()
		{
			// TODO Install Inconsolata (license?),  Deja Vu Sans Mono (public domain!), or Droid Sans Mono (Apache)
			// Deja Vu Sans Mono: http://dejavu-fonts.org/wiki/index.php?title=Main_Page

			// The lower indices should be preferred fonts. Higher indices should be generic fonts.
			string[] fontList = new string[] { "Consolas", "Inconsolata", "Deja Vu Sans Mono", "Lucida Console", "Courier New", "Courier" };

			foreach (string font in fontList)
			{
				Font test = new System.Drawing.Font(font, 9.5F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)(0));

				if (test.Name == font)
				{
					// Got a font on our list
					this.IOBox.Font = test;
					logger.Info("Picked font: {0}", test.Name);
					return;
				}
			}

			// No fonts in the list were available, pick the most generic font from the list and allow the system to pick a fallback
			this.IOBox.Font = new Font(fontList[fontList.Length - 1], 10.0f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
		}

		#region COM port settings handlers

		public string GetLoggingFilePath()
		{
			if (selectLoggingFileDialog.ShowDialog() == DialogResult.OK)
				return selectLoggingFileDialog.FileName;

			throw new FileSelectionCanceledException();
		}

		public void TrimLines(int trimTo)
		{
			Invoke(new MethodInvoker(
					   () =>
					   {
						   logger.Info("Trimming lines; current count: {0}", IOBox.Lines.Length);
						   if (IOBox.Lines.Length <= trimTo)
							   return;

						   string[] newLines = new string[trimTo];

						   Array.Copy(IOBox.Lines, IOBox.Lines.Length - trimTo, newLines, 0, newLines.Length);
						   IOBox.Lines = newLines;

						   IOBox.Text = string.Join(Environment.NewLine, IOBox.Lines); // TODO is this necessary?

						   logger.Info("After trim, lines: {0}", IOBox.Lines.Length);
					   }));
		}

		public void AddLine(string line)
		{
			Invoke(new MethodInvoker(
					   () =>
					   {
						   if (!Focused)
						   {
							   IOBox.Select(IOBox.Text.Length, 0);
							   IOBox.ScrollToCaret();
						   }
						   IOBox.AppendText(line);
					   }));
		}

		public void AddChar(char c)
		{
			Invoke(new MethodInvoker(() => IOBox.AppendText(c.ToString(CultureInfo.InvariantCulture))));
		}

		public void SetFileSendState(FileSendState state)
		{
			Color circleColor = Color.Empty;
			bool circleVisible = false;
			bool circleActive = false;

			switch (state)
			{
				case FileSendState.Error:
					circleColor = Color.Red;
					circleVisible = true;
					break;

				case FileSendState.Hidden:
					break;

				case FileSendState.InProgress:
					circleColor = SystemColors.HotTrack;
					circleVisible = true;
					circleActive = true;
					break;

				case FileSendState.Success:
					circleColor = Color.Lime;
					circleVisible = true;
					break;
			}

			Invoke(new MethodInvoker(delegate
			{
				fileSendLoadingCircle.LoadingCircleControl.Color = circleColor;
				fileSendLoadingCircle.Visible = circleVisible;
				fileSendLoadingCircle.LoadingCircleControl.Active = circleActive;
				fileSendLoadingCircle.LoadingCircleControl.Refresh();
			}));
		}

		private ISerialBackend _backend;

		public void SetBackend(ISerialBackend backend)
		{
			logger.Trace("Set backend");
			_backend = backend;
			_backend.PropertyChanged += BackendOnPropertyChanged;
		}

		public event SendFileEventHandler OnSendFile;

		public event SetLoggingPathEventHandler OnSetLoggingPath;

		public event OnKeyPressedEventHandler OnKeyPressed;

		public event SaveSessionEventHandler OnSaveSession;

		#endregion COM port settings handlers

		// MenuItem handler
		private void SetLoggingFile(object sender, EventArgs e)
		{
			try
			{
				if (OnSetLoggingPath == null) return;

				var loggingPath = new SetLoggingPathEventArgs(GetLoggingFilePath());
				OnSetLoggingPath(this, loggingPath);
			}
			catch (FileSelectionCanceledException)
			{
			}
		}

		private void fileSendPane1_OnSend(object sender, EventArgs e, CustomControls.FileSendPane.SerialSendArgs a)
		{
			SerialSendBytes(a);
		}

		private void SerialSendBytes(CustomControls.FileSendPane.SerialSendArgs a)
		{
			if (a.data == null)
				return;

			SendFileEventArgs e = new SendFileEventArgs(a.data, a.LineDelay, a.CharDelay);
			if (OnSendFile != null)
				OnSendFile(this, e);
		}

		public void Run()
		{
			Initialize();
			Application.Run(this);
		}

		private IAboutBox _aboutBox;

		private List<int> baudRateVals;

		public MainForm(IAboutBox aboutBox)
		{
			logger.Trace("Mainform object created");
			_aboutBox = aboutBox;

			baudRateVals = new List<int>(new[] { 110, 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 });
		}

		private void Initialize()
		{
			logger.Trace("Initializing MainForm");
			InitializeComponent();

			SetMonospacedFont();

			if (System.Diagnostics.Debugger.IsAttached)
				saveEntireSessionToolStripMenuItem.Visible = true;

			CreateMenuFrom(dropDownBaud, baudRateVals, "BaudRate", ChangeCOMParam);
			CreateMenuFrom(menuItemBaud, baudRateVals, "BaudRate", ChangeCOMParam);
			fileSendLoadingCircle.Alignment = ToolStripItemAlignment.Right;

			SetupFileSendSpinnerSpokes();

			logger.Warn("MainForm initialization complete");
		}

		//Toggle connected
		private void ToggleConnection(object sender, EventArgs e)
		{
			logger.Trace("ToggleConnection");
			_backend.PortState = _backend.PortState == PortState.Open ? PortState.Closed : PortState.Open;
		}

		//Toggle echo
		private void ToggleEcho(object sender, EventArgs e)
		{
			logger.Trace("Toggle Echo");
			_backend.EchoState = _backend.EchoState == EchoState.Enabled ? EchoState.Disabled : EchoState.Enabled;
		}

		// Why both of these? - they catch from different controls
		private void SendKey(object sender, KeyPressEventArgs e) //Key press handler
		{
			e.Handled = true;

			if (OnKeyPressed != null)
			{
				OnKeyPressedEventArgs ev = new OnKeyPressedEventArgs();
				ev.KeyChar = e.KeyChar;
				OnKeyPressed(this, ev);
			}
		}

		// Why both of these?
		private void HandleKeyPress(object sender, KeyEventArgs e)
		{
			e.Handled = true;

			if (e.KeyValue == 0x0A) // Newline
			{
				if (OnKeyPressed != null)
				{
					OnKeyPressedEventArgs ev = new OnKeyPressedEventArgs();
					ev.KeyChar = (char)10;
					OnKeyPressed(this, ev);
				}
			}
		}

		//Show file send pane
		private void toolStripButtonSendFile_Click(object sender, EventArgs e)
		{
			fileSendPane1.Visible = !fileSendPane1.Visible;
		}

		//Initialize application shutdown
		private void Exit(object sender, EventArgs e)
		{
			logger.Info("Menu click: Exit");
			Application.Exit();
		}

		//Show about form
		private void ShowAboutForm(object sender, EventArgs e)
		{
			_aboutBox.Open();
		}

		private void SelectCOMPort(object sender, ToolStripItemClickedEventArgs e)
		{
			_backend.CurrentDevice = e.ClickedItem.Text;
		}

		private void SaveEntireSessionToolStripMenuItemClick(object sender, EventArgs e)
		{
			SaveSession();
		}

		private bool SaveSession()
		{
			logger.Info("Saving session");
			var save = new SaveFileDialog
									  {
										  AddExtension = true,
										  AutoUpgradeEnabled = true,
										  Filter =
											  Resources.MainForm_SaveSession_Text_files + "|*.txt|" +
											  Resources.MainForm_SaveSession_All_files + "|*.*",
										  FilterIndex = 0,
										  OverwritePrompt = true,
										  Title = Resources.MainForm_SaveSession_Save_Session,
										  ValidateNames = true
									  };

			if (save.ShowDialog() == DialogResult.OK)
			{
				if (!string.IsNullOrEmpty(save.FileName))
				{
					if (OnSaveSession != null)
					{
						SaveSessionEventArgs ev = new SaveSessionEventArgs();
						ev.FileName = save.FileName;
						ev.SessionData = IOBox.Text;
						OnSaveSession(this, ev);
						logger.Debug("Session saved");
						return true;
					}
					logger.Warn("OnSaveSession event has no handlers");
				}
				else
					logger.Error("Filename null or empty");
			}
			else
				logger.Debug("User canceled");

			logger.Warn("Session not saved");
			return false;
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			logger.Trace("User clicked menu item: New");
			DialogResult result = MessageBox.Show("Would you like to save current session?", Resources.MainForm_SaveSession_Save_Session, MessageBoxButtons.YesNoCancel);

			if (result == DialogResult.Cancel)
			{
				logger.Trace("User canceled");
				return;
			}

			if (result == DialogResult.No)
			{
				logger.Debug("User cleared session");
				IOBox.Clear();
				return;
			}

			if (result == DialogResult.Yes)
			{
				logger.Debug("User saved session");
				if (SaveSession())
				{
					IOBox.Clear();
					logger.Trace("Session cleared");
				}
				else
					logger.Debug("Session not cleared");
			}
		}

		/// <summary>
		/// Populates a menu with items
		/// </summary>
		/// <param name="items">Reference to the finished items</param>
		/// <param name="values">One item will be generated for each value</param>
		/// <param name="parentMenu">Parent menu for the items</param>
		/// <param name="name">DeviceName property for each item</param>
		/// <param name="clickHandler">OnClick event for each item</param>
		/// <returns>Success</returns>
		private void CreateMenuFrom(ToolStripDropDownItem parentMenu, IEnumerable values, string name, EventHandler clickHandler)
		{
			if (values == null)
				return;

			logger.Trace("Creating menu \"" + name + '"');

			var items = new List<ToolStripItem>();

			foreach (var value in values)
			{
				ToolStripItem temp = new ToolStripMenuItem();
				temp.Name = name;
				temp.Size = new Size(152, 22);
				temp.Click += clickHandler;
				temp.Text = value.ToString();
				items.Add(temp);
			}

			parentMenu.DropDownItems.AddRange(items.ToArray());
		}

		private void SetupFileSendSpinnerSpokes()
		{
			fileSendLoadingCircle.LoadingCircleControl.InnerCircleRadius = 3;
			fileSendLoadingCircle.LoadingCircleControl.OuterCircleRadius = 6;
			fileSendLoadingCircle.LoadingCircleControl.NumberSpoke = 8;
			fileSendLoadingCircle.LoadingCircleControl.SpokeThickness = 2;
			fileSendLoadingCircle.LoadingCircleControl.RotationSpeed = 100;
			fileSendLoadingCircle.LoadingCircleControl.Color = SystemColors.HotTrack;
		}

		#endregion Initialization

		#region Event Handlers

		// TODO FIXME AAH THIS IS YUCK. REWRITE.
		private void HandleCOMParamChange(object sender, ToolStripItemClickedEventArgs e)
		{
			ChangeCOMParam(new string[2] { ((ToolStripItem)sender).Text, e.ClickedItem.Text }, null);
		}

		// TODO This is terrifying
		public void ChangeCOMParam(object sender, EventArgs e)
		{
			logger.Warn("Hit hideous code.");
			string param = null;
			string value = null;

			if (sender is ToolStripMenuItem)
			{
				param = ((ToolStripMenuItem)sender).Name;
				value = ((ToolStripMenuItem)sender).Text;
			}
			else
				if (sender is string[])
				{
					param = ((string[])sender)[0];
					value = ((string[])sender)[1];
				}
				else //Handle anything else that uses this function in the future
					return;

			switch (param)
			{
				case "Parity": //String
					switch (value.ToLower())
					{
						case "even":
							_backend.Parity = Parity.Even;
							break;

						case "odd":
							_backend.Parity = Parity.Odd;
							break;

						case "none":
							_backend.Parity = Parity.None;
							break;

						case "mark":
							_backend.Parity = Parity.Mark;
							break;

						case "space":
							_backend.Parity = Parity.Space;
							break;
					}
					break;

				case "Stop Bits": //Double
					switch ((int)(double.Parse(value) * 10))
					{
						case 10:
							_backend.StopBits = StopBits.One;
							break;

						case 15:
							_backend.StopBits = StopBits.OnePointFive;
							break;

						case 20:
							_backend.StopBits = StopBits.Two;
							break;
					}
					break;

				case "Data Bits": //Int
					_backend.DataBits = int.Parse(value);
					break;

				case "Flow Control": //String
					switch (value)
					{
						case "None":
							_backend.FlowControl = FlowControl.None;
							break;

						case "Request To Send":
							_backend.FlowControl = FlowControl.RequestToSend;
							break;

						case "Xon/Xoff":
							_backend.FlowControl = FlowControl.XOnXOff;
							break;

						case "RTS + Xon/Xoff":
							_backend.FlowControl = FlowControl.RequestToSendXOnXOff;
							break;

						default:
							_backend.FlowControl = FlowControl.None;
							break;
					}
					break;

				case "BaudRate": //Int
					_backend.Baud = int.Parse(value);
					break;
			}
		}

		//List all COM ports
		private void UpdateCOMPorts(object sender, EventArgs e)
		{
			string[] ports = _backend.Devices;

			if (ports == null)
			{
				logger.Error("No serial ports to list");
				logger.Fatal("TODO We should handle this more gracefully");
				logger.Fatal("Show a 'No serial ports found' item");
				return;
			}

			if (sender is ToolStripDropDownItem)
			{
				ToolStripDropDownItem menu = sender as ToolStripDropDownItem;
				menu.DropDownItems.Clear();
				foreach (var port in ports)
				{
					menu.DropDownItems.Add(port);
				}
			}
		}

		//TODO Fix SendDroppedText
		private void SendDroppedText(object sender, DragEventArgs e)//Drop handler
		{
			//Serial send the dropped text

			if (e.Effect != DragDropEffects.None)
				return;

			//Handle file drops
			//if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
			//{
			//    //LogLine("File dropped onto application");
			//    String[] files = (String[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);
			//    fileSendPane1.SendFile(files[0]);
			//    e.Effect = DragDropEffects.Copy;
			//}
			//else
			//    //Handle raw text drops
			//    if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.Text))
			//    {
			//        //LogLine("Text dropped onto application");
			//        String data = (String)e.Data.GetData(System.Windows.Forms.DataFormats.StringFormat);
			//        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

			//        fileSendPane1.SendData(encoding.GetBytes(data), "Dropped text");
			//        e.Effect = DragDropEffects.Copy;
			//    }
		}

		#region Pretend buttons for status bar

		private void PretendClick(object sender, MouseEventArgs e)
		{
			if (sender is ToolStripStatusLabel)
				((ToolStripStatusLabel)sender).BorderStyle = Border3DStyle.SunkenInner;
		}

		private void PretendRelease(object sender, MouseEventArgs e)
		{
			if (sender is ToolStripStatusLabel)
				((ToolStripStatusLabel)sender).BorderStyle = Border3DStyle.Flat;
		}

		private void PretendLeave(object sender, EventArgs e)
		{
			if (sender is ToolStripStatusLabel)
				((ToolStripStatusLabel)sender).BorderStyle = Border3DStyle.Flat;
		}

		private void PretendEnter(object sender, EventArgs e)
		{
			if (sender is ToolStripStatusLabel)
				((ToolStripStatusLabel)sender).BorderStyle = Border3DStyle.RaisedInner;
		}

		#endregion Pretend buttons for status bar

		#endregion Event Handlers

		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		public event PropertyChangedEventHandler PropertyChanged;

		private void BackendOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			logger.Warn("VersionShim setting {0}", propertyChangedEventArgs.PropertyName);

			try
			{
				switch (propertyChangedEventArgs.PropertyName)
				{
					case "Title":
						this.Text = _backend.Title;
						break;

					case "currentDevice":
						UpdateCOMPort();
						break;

					case "portState":
						UpdatePortState();
						break;

					case "Baud":
						UpdateBaudRate();
						break;

					case "loggingState":
						UpdateLoggingState();
						break;

					case "echoState":
						UpdateEchoState();
						break;

					case "stopBits":
						logger.Trace("Changing stopBits to {0}", _backend.StopBits);

						if (menuItemStopBits == null)
						{
							logger.Error("menuItemStopBits == null");
							return;
						}

						foreach (ToolStripMenuItem item in menuItemStopBits.DropDownItems)
							item.Checked = item.Text == _backend.StopBits.GetDescription<StopBits>();

						break;

					case "dataBits":
						UpdateDataBits();
						break;

					case "flowControl":
						UpdateFlowControl();
						break;

					case "parity":
						UpdateParity();
						break;

					case "DeviceStatus":
						toolStripStatusLabelPortSettings.Text = _backend.StatusLabel;
						break;

					default:
						logger.Error("Unhandled - {0}", propertyChangedEventArgs.PropertyName);
						break;
				}
			}
			catch (NullReferenceException e)
			{
				logger.Error("Error: {0}", e.Message);
			}
		}

		public void UpdatePortState()
		{
			switch (_backend.PortState)
			{
				case PortState.Open:
					toolStripButtonConnect.Text = Resources.Text_Disconnect;
					toolStripButtonConnect.Image = Resources.disconnected;
					toolStripButtonConnect.ForeColor = SystemColors.ControlText;
					break;

				case PortState.Closed:
					toolStripButtonConnect.Text = Resources.Text_Connect;
					toolStripButtonConnect.Image = Resources.connected;
					break;

				case PortState.Error:
					_backend.PortState = PortState.Closed;
					toolStripButtonConnect.ForeColor = Color.Red;
					break;
			}
		}

		private void UpdateLoggingState()
		{
			logger.Trace("Logging set to {0}", _backend.LoggingState);

			switch (_backend.LoggingState)
			{
				case LoggingState.Disabled:

					toolStripLoggingEnabled.Text = Resources.Text_Logging_Disabled;
					MenuItemToggleLogging.Checked = false;
					break;

				case LoggingState.Enabled:

					toolStripLoggingEnabled.Text = Resources.Text_Logging_Enabled;
					MenuItemToggleLogging.Checked = true;
					break;
			}
		}

		private void ToggleLogging(object sender, System.EventArgs e)
		{
			logger.Trace("Toggle logging");
			_backend.LoggingState = _backend.LoggingState == LoggingState.Disabled ? LoggingState.Enabled : LoggingState.Disabled;
		}

		private void UpdateEchoState()
		{
			logger.Trace("Echo set to {0}", _backend.EchoState);

			switch (_backend.EchoState)
			{
				case EchoState.Disabled:
					toolStripStatusLabelLocalEcho.Text = Resources.Text_Echo_Off;
					break;

				case EchoState.Enabled:
					toolStripStatusLabelLocalEcho.Text = Resources.Text_Echo_On;
					break;
			}
		}

		public void UpdateCOMPort()
		{
			dropDownCOMPort.Text = _backend.CurrentDevice;

			foreach (ToolStripMenuItem item in menuItemCOMPort.DropDownItems)
				item.Checked = item.Text == _backend.CurrentDevice;

			foreach (ToolStripMenuItem item in dropDownCOMPort.DropDownItems)
				item.Checked = item.Text == _backend.CurrentDevice;
		}

		public void UpdateBaudRate()
		{
			if (dropDownBaud == null)
				return;

			// TODO Throw an error. Also, rejigger this shit.

			dropDownBaud.Text = _backend.Baud.ToString(CultureInfo.InvariantCulture) + Resources.Text_Baud;

			foreach (ToolStripMenuItem item in menuItemBaud.DropDownItems)
				item.Checked = item.Text == _backend.Baud.ToString(CultureInfo.InvariantCulture);

			foreach (ToolStripMenuItem item in dropDownBaud.DropDownItems)
				item.Checked = item.Text == _backend.Baud.ToString(CultureInfo.InvariantCulture);
		}

		// TODO this is gross, fix it.
		public void UpdateDataBits()
		{
			if (menuItemDataBits == null)
				return;

			logger.Trace("Setting dataBits to {0}", _backend.DataBits);

			foreach (ToolStripMenuItem item in menuItemDataBits.DropDownItems)
				item.Checked = item.Text == _backend.DataBits.ToString();
		}

		public void UpdateFlowControl()
		{
			logger.Trace("Setting flow control to {0}", _backend.FlowControl);

			foreach (ToolStripMenuItem item in menuItemFlowControl.DropDownItems)
				item.Checked = item.Text == _backend.FlowControl.GetDescription<FlowControl>();
		}

		public void UpdateParity()
		{
			logger.Trace("Setting parity to {0}", _backend.Parity);

			foreach (ToolStripMenuItem item in menuItemParity.DropDownItems)
				item.Checked = item.Text == _backend.Parity.ToString();
		}
	}
}