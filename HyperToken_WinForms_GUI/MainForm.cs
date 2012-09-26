//Author: George Hahn
//$Rev:: 761                                             $:  Revision of last commit
//$Author:: ghahn                                        $:  Author of last commit
//$Date:: 2011-07-13 13:47:52 -0400 (Wed, 13 Jul 2011)   $:  Date of last commit

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Bugsense.WPF;
using HyperToken_WinForms_GUI.Helpers;
using HyperToken_WinForms_GUI.Properties;
using NLog;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;
using Terminal_Interface.Exceptions;

// TODO tweak garbage collection
// TODO Custom baud setting
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

		#region ITerminal

		public string Title { get; set; }

		private void OnTitleChanged()
		{
			this.Text = Title;
		}

		public string LoggingFilePath { get; set; }

		public LoggingState loggingState { get; set; }

		private void OnloggingStateChanged()
		{
			logger.Trace("Logging set to {0}", loggingState);

			switch (loggingState)
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
			if (loggingState == LoggingState.Disabled)
				loggingState = LoggingState.Enabled;
			else
				loggingState = LoggingState.Disabled;
		}

		public EchoState echoState { get; set; }

		private void OnechoStateChanged()
		{
			logger.Trace("Echo set to {0}", echoState);

			switch (echoState)
			{
				case EchoState.Disabled:
					toolStripStatusLabelLocalEcho.Text = Resources.Text_Echo_Off;
					break;

				case EchoState.Enabled:
					toolStripStatusLabelLocalEcho.Text = Resources.Text_Echo_On;
					break;
			}
		}

		public PortState portState { get; set; }

		public FileSendState fileSendState { get; set; }

		#region COM port settings handlers

		public string COMPort { get; set; }

		public void OnCOMPortChanged()
		{
			dropDownCOMPort.Text = COMPort;

			foreach (ToolStripMenuItem item in menuItemCOMPort.DropDownItems)
				item.Checked = item.Text == COMPort;

			foreach (ToolStripMenuItem item in dropDownCOMPort.DropDownItems)
				item.Checked = item.Text == COMPort;
		}

		public int baud { get; set; }

		public void OnbaudChanged()
		{
			dropDownBaud.Text = baud.ToString(CultureInfo.InvariantCulture) + Resources.Text_Baud;

			foreach (ToolStripMenuItem item in menuItemBaud.DropDownItems)
				item.Checked = item.Text == baud.ToString();

			foreach (ToolStripMenuItem item in dropDownBaud.DropDownItems)
				item.Checked = item.Text == baud.ToString();
		}

		public StopBits stopBits { get; set; }

		public void OnstopBitsChanged()
		{
			logger.Trace("Changing stopBits to {0}", stopBits);

			foreach (ToolStripMenuItem item in menuItemStopBits.DropDownItems)
			{
				if (item.Text == stopBits.GetDescription<StopBits>())
					item.Checked = true;
				else
					item.Checked = false;
			}

			UpdateSerialStatusLabel();
		}

		public int dataBits { get; set; }

		public void OndataBitsChanged()
		{
			logger.Trace("Setting dataBits to {0}", dataBits);

			foreach (ToolStripMenuItem item in menuItemDataBits.DropDownItems)
				item.Checked = item.Text == dataBits.ToString();

			UpdateSerialStatusLabel();
		}

		public FlowControl flowControl { get; set; }

		public void OnflowControlChanged()
		{
			logger.Trace("Setting flow control to {0}", flowControl);

			foreach (ToolStripMenuItem item in menuItemFlowControl.DropDownItems)
				item.Checked = item.Text == flowControl.GetDescription<FlowControl>();

			//Change the port flow control
		}

		public Parity parity { get; set; }

		public void OnparityChanged()
		{
			logger.Trace("Setting parity to {0}", parity);

			foreach (ToolStripMenuItem item in menuItemParity.DropDownItems)
				item.Checked = item.Text == parity.ToString();

			UpdateSerialStatusLabel();
		}

		#endregion COM port settings handlers

		public string[] serialPorts { get; private set; }

		public bool connected { get; set; }

		public string GetLoggingFilePath()
		{
			if (selectLoggingFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				return selectLoggingFileDialog.FileName;

			throw new FileSelectionCanceledException();
		}

		public void TrimLines(int trimTo)
		{
			Invoke(new MethodInvoker(

				delegate
				{
					if (IOBox.Lines.Length <= trimTo)
						return;

					string[] newLines = new string[trimTo];

					Array.Copy(IOBox.Lines, IOBox.Lines.Length - trimTo, newLines, 0, newLines.Length);
					IOBox.Lines = newLines;

					IOBox.Text = string.Join(Environment.NewLine, IOBox.Lines); // TODO is this necessary?
				}));
		}

		public void AddLine(string line)
		{
			Invoke(new MethodInvoker(

				delegate
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
			Invoke(new MethodInvoker(
				delegate
				{
					IOBox.AppendText(c.ToString(CultureInfo.InvariantCulture));

					// Apparently this doesn't work:
					// IOBox.Lines[IOBox.Lines.Length - 1] += c;
				}));
		}

		public void SetPortConnection(PortState state)
		{
			switch (state)
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
					SetPortConnection(PortState.Closed);
					toolStripButtonConnect.ForeColor = Color.Red;
					break;
			}
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
					circleActive = false;
					break;

				case FileSendState.Hidden:
					circleVisible = false;
					circleActive = false;
					break;

				case FileSendState.InProgress:
					circleColor = SystemColors.HotTrack;
					circleVisible = true;
					circleActive = true;
					break;

				case FileSendState.Success:
					circleColor = Color.Lime;
					circleVisible = true;
					circleActive = false;
					break;
				default:
					SetFileSendState(FileSendState.Hidden);
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

		private IBackend _backend;

		public void SetBackend(IBackend backend)
		{
			_backend = backend;
		}

		public event SendFileEventHandler OnSendFile;

		public event SetLoggingPathEventHandler OnSetLoggingPath;

		public event ToggleConnectionEventHandler OnToggleConnection;

		public event OnKeyPressedEventHandler OnKeyPressed;

		public event SerialPortListEventHandler OnSerialPortList;

		public event SaveSessionEventHandler OnSaveSession;

		#endregion ITerminal

		// MenuItem handler
		private void SetLoggingFile(object sender, EventArgs e)
		{
			try
			{
				if (OnSetLoggingPath != null)
				{
					SetLoggingPathEventArgs loggingPath = new SetLoggingPathEventArgs(GetLoggingFilePath());
					OnSetLoggingPath(this, loggingPath);
				}
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
			BugSense.SendException(new Exception("Test - 3"));
			Application.Run(this);
		}

		public MainForm()
		{
			logger.Trace("Initializing MainForm");
			InitializeComponent();

			SetMonospacedFont();

			if (System.Diagnostics.Debugger.IsAttached)
				saveEntireSessionToolStripMenuItem.Visible = true;

			// TODO Rewrite this steaming pile of %$&#
			object[] baudRateValues = new object[13] { 110, 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 };

			baudRates = new ToolStripItem[2][];
			CreateMenuFrom(ref baudRates[0], baudRateValues, dropDownBaud, "BaudRate", MenuType.dropdown, ChangeCOMParam);
			CreateMenuFrom(ref baudRates[1], baudRateValues, menuItemBaud, "BaudRate", MenuType.menu, ChangeCOMParam);

			fileSendLoadingCircle.Alignment = ToolStripItemAlignment.Right;

			SetupFileSendSpinnerSpokes();

			logger.Warn("MainForm initialization complete");
		}

		//Toggle connected
		private void ToggleConnection(object sender, EventArgs e)
		{
			logger.Trace("ToggleConnection");
			if (OnToggleConnection != null)
			{
				ToggleConnectionEventArgs ev = new ToggleConnectionEventArgs();
				OnToggleConnection(this, ev);
			}
			else
				logger.Trace("No OnToggleConnection handler available");
		}

		// Why both of these?
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

		//Toggle echo
		private void ToggleEcho(object sender, EventArgs e)
		{
			if (echoState == EchoState.Enabled)
				echoState = EchoState.Disabled;
			else
				echoState = EchoState.Enabled;
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
			//Display a brief 'about' form
			if ((formAbout == null) || (!formAbout.Visible))
			{
				logger.Trace("Showing about form");
				formAbout = new AboutBox();
				formAbout.Show();
			}
			else
			{
				logger.Trace("About form already open");
				formAbout.BringToFront();
			}
		}

		private void SelectCOMPort(object sender, ToolStripItemClickedEventArgs e)
		{
			COMPort = e.ClickedItem.Text;
		}

		private void SaveEntireSessionToolStripMenuItemClick(object sender, EventArgs e)
		{
			SaveSession();
		}

		private bool SaveSession()
		{
			logger.Info("Saving session");
			SaveFileDialog save = new SaveFileDialog();
			save.AddExtension = true;
			save.AutoUpgradeEnabled = true;
			save.Filter = "Text files|*.txt|All files|*.*";
			save.FilterIndex = 0;
			save.OverwritePrompt = true;
			save.Title = "Save Session";
			save.ValidateNames = true;
			if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
					else
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
			DialogResult result = MessageBox.Show("Would you like to save current session?", "Save Session", MessageBoxButtons.YesNoCancel);

			if (result == System.Windows.Forms.DialogResult.Cancel)
			{
				logger.Trace("User canceled");
				return;
			}

			if (result == System.Windows.Forms.DialogResult.No)
			{
				logger.Debug("User cleared session");
				IOBox.Clear();
				return;
			}

			if (result == System.Windows.Forms.DialogResult.Yes)
			{
				logger.Debug("User saved session");
				if (SaveSession())
				{
					IOBox.Clear();
					logger.Trace("Session cleared");
				}
				else
					logger.Debug("Session not cleared");
				return;
			}
		}

		/// <summary>
		/// Populates a menu with items
		/// </summary>
		/// <param name="items">Reference to the finished items</param>
		/// <param name="values">One item will be generated for each value</param>
		/// <param name="parentMenu">Parent menu for the items</param>
		/// <param name="name">Name property for each item</param>
		/// <param name="type">What type of menu the items are for</param>
		/// <param name="clickHandler">OnClick event for each item</param>
		/// <returns>Success</returns>
		private bool CreateMenuFrom(ref ToolStripItem[] items, object[] values, object parentMenu, string name, MenuType type, System.EventHandler clickHandler)
		{
			if (values == null)
				return false;

			if (values.Length <= 0)
				return false;

			//LogLine("Creating menu \"" + name + '"');

			items = new ToolStripItem[values.Length];

			for (int i = 0; i < values.Length; i++)
			{
				switch (type)
				{
					case MenuType.menu:
						items[i] = new ToolStripMenuItem();

						//items[i].Name = "menu";
						break;

					case MenuType.dropdown:
						items[i] = new ToolStripMenuItem();

						//items[i].Name = "dropDown";
						break;
				}

				items[i].Name = name;
				items[i].Size = new System.Drawing.Size(152, 22);
				items[i].Click += new System.EventHandler(clickHandler);
				items[i].Text = values[i].ToString();
			}

			((ToolStripDropDownItem)parentMenu).DropDownItems.AddRange(items);

			return true;
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

		private void Uninitialize(object sender, FormClosingEventArgs e)
		{
		}

		#region Event Handlers

		// TODO FIXME AAH THIS IS YUCK. REWRITE.
		private void HandleCOMParamChange(object sender, ToolStripItemClickedEventArgs e)
		{
			ChangeCOMParam(new string[2] { ((ToolStripItem)sender).Text, e.ClickedItem.Text }, null);
		}

		// TODO This is terrifying
		public void ChangeCOMParam(object sender, EventArgs e)
		{
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
							parity = Parity.Even;
							break;

						case "odd":
							parity = Parity.Odd;
							break;

						case "none":
							parity = Parity.None;
							break;

						case "mark":
							parity = Parity.Mark;
							break;

						case "space":
							parity = Parity.Space;
							break;
					}
					break;

				case "Stop Bits": //Double
					switch ((int)(double.Parse(value) * 10))
					{
						case 10:
							stopBits = StopBits.One;
							break;

						case 15:
							stopBits = StopBits.OnePointFive;
							break;

						case 20:
							stopBits = StopBits.Two;
							break;
					}
					break;

				case "Data Bits": //Int
					dataBits = int.Parse(value);
					break;

				case "Flow Control": //String
					switch (value)
					{
						case "None":
							flowControl = FlowControl.None;
							break;

						case "Request To Send":
							flowControl = FlowControl.RequestToSend;
							break;

						case "Xon/Xoff":
							flowControl = FlowControl.XOnXOff;
							break;

						case "RTS + Xon/Xoff":
							flowControl = FlowControl.RequestToSendXOnXOff;
							break;

						default:
							flowControl = FlowControl.None;
							break;
					}
					break;

				case "BaudRate": //Int
					baud = int.Parse(value);
					break;
			}
		}

		//List all COM ports
		private void UpdateCOMPorts(object sender, EventArgs e)
		{
			string[] ports = null;
			if (OnSerialPortList != null)
			{
				SerialPortListEventArgs ev = new SerialPortListEventArgs();
				OnSerialPortList(this, ev);
				if (ev.ports != null)
					ports = ev.ports;
			}

			if (ports == null)
			{
				logger.Warn("No serial ports to list");
				return;
			}

			if (sender is ToolStripDropDownItem)
			{
				ToolStripDropDownItem menu = sender as ToolStripDropDownItem;
				menu.DropDownItems.Clear();
				foreach (string port in ports)
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

		private void UpdateSerialStatusLabel()
		{
			toolStripStatusLabelPortSettings.Text =
				dataBits.ToString() + ';' +
				parity.ToString()[0] + ';' +
				stopBits.GetDescription<StopBits>();
		}

		#region Private variables

		private static Logger logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// About box instance
		/// </summary>
		private AboutBox formAbout;

		/// <summary>
		/// Contains all lines received so far
		/// </summary>
		//private List<string> allLines = new List<string>(100000);

		/// <summary>
		/// Various menu types for the CreateMenuFrom function
		/// </summary>
		public enum MenuType { menu, dropdown, combobox };

		public ToolStripItem[][] baudRates;

		#endregion Private variables

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Implementation of INotifyPropertyChanged
	}
}