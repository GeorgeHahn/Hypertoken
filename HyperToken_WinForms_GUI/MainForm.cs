using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using CustomControls;
using HyperToken_WinForms_GUI.Helpers;
using HyperToken_WinForms_GUI.Properties;
using NLog;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;

// TODO tweak garbage collection
// TODO Custom Baud setting
// TODO right click menu
// - Copy, Save selection, Send file
// TODO Parse incoming data for unprintable characters (display as hex?)

namespace HyperToken_WinForms_GUI
{
	public partial class MainForm : Form, ITerminal, INotifyPropertyChanged
	{
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		private readonly IAboutBox _aboutBox;

		private readonly IEchoer _echoer;

		private readonly ILogger _logger;

		private readonly IFileSender _fileSender;

		private ISerialPort _dataDevice;

		private List<int> _baudRateVals;

		public MainForm(IAboutBox aboutBox, ILogger logger, IEchoer echoer, IFileSender fileSender, ISerialPort dataDevice)
		{
			_aboutBox = aboutBox;
			_logger = logger;
			_echoer = echoer;
			_fileSender = fileSender;
			_dataDevice = dataDevice;

			_dataDevice.PropertyChanged += DataDeviceOnPropertyChanged;
			_dataDevice.DataReceived += DataDeviceOnDataReceived;

			MainForm.logger.Trace("Mainform object created");
		}

		public MainForm(IAboutBox aboutBox, ISerialPort dataDevice)
			: this(aboutBox, null, null, null, dataDevice)
		{ }

		public event SaveSessionEventHandler OnSaveSession;

		public event PropertyChangedEventHandler PropertyChanged;

		public string Title
		{
			get { return Text; }
			set { Text = value; }
		}

		public static string GetVersion()
		{
			return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public void AddChar(char c)
		{
			Invoke(new MethodInvoker(() => IOBox.AppendText(c.ToString(CultureInfo.InvariantCulture))));
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

			var menuItem = sender as ToolStripMenuItem;
			if (menuItem is ToolStripMenuItem)
			{
				param = menuItem.Name;
				value = menuItem.Text;
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
							_dataDevice.Parity = Parity.Even;
							break;

						case "odd":
							_dataDevice.Parity = Parity.Odd;
							break;

						case "none":
							_dataDevice.Parity = Parity.None;
							break;

						case "mark":
							_dataDevice.Parity = Parity.Mark;
							break;

						case "space":
							_dataDevice.Parity = Parity.Space;
							break;
					}
					break;

				case "Stop Bits": //Double
					switch ((int)(double.Parse(value) * 10))
					{
						case 10:
							_dataDevice.StopBits = StopBits.One;
							break;

						case 15:
							_dataDevice.StopBits = StopBits.OnePointFive;
							break;

						case 20:
							_dataDevice.StopBits = StopBits.Two;
							break;
					}
					break;

				case "Data Bits": //Int
					_dataDevice.DataBits = int.Parse(value);
					break;

				case "Flow Control": //String
					switch (value)
					{
						case "None":
							_dataDevice.FlowControl = FlowControl.None;
							break;

						case "Request To Send":
							_dataDevice.FlowControl = FlowControl.RequestToSend;
							break;

						case "Xon/Xoff":
							_dataDevice.FlowControl = FlowControl.XOnXOff;
							break;

						case "RTS + Xon/Xoff":
							_dataDevice.FlowControl = FlowControl.RequestToSendXOnXOff;
							break;

						default:
							_dataDevice.FlowControl = FlowControl.None;
							break;
					}
					break;

				case "BaudRate": //Int
					_dataDevice.Baud = int.Parse(value);
					break;
			}
		}

		public void Run()
		{
			Initialize();
			Application.Run(this);
		}

		public void UpdateBaudRate()
		{
			if (dropDownBaud == null)
				return;

			// TODO Throw an error. Also, rejigger this shit.

			dropDownBaud.Text = _dataDevice.Baud.ToString(CultureInfo.InvariantCulture) + Resources.Text_Baud;

			foreach (ToolStripMenuItem item in menuItemBaud.DropDownItems)
				item.Checked = item.Text == _dataDevice.Baud.ToString(CultureInfo.InvariantCulture);

			foreach (ToolStripMenuItem item in dropDownBaud.DropDownItems)
				item.Checked = item.Text == _dataDevice.Baud.ToString(CultureInfo.InvariantCulture);
		}

		public void UpdateCOMPort()
		{
			dropDownCOMPort.Text = _dataDevice.DeviceName;

			foreach (ToolStripMenuItem item in menuItemCOMPort.DropDownItems)
				item.Checked = item.Text == _dataDevice.DeviceName;

			foreach (ToolStripMenuItem item in dropDownCOMPort.DropDownItems)
				item.Checked = item.Text == _dataDevice.DeviceName;
		}

		// TODO this is gross, fix it.
		public void UpdateDataBits()
		{
			if (menuItemDataBits == null)
				return;

			logger.Trace("Setting dataBits to {0}", _dataDevice.DataBits);

			foreach (ToolStripMenuItem item in menuItemDataBits.DropDownItems)
				item.Checked = item.Text == _dataDevice.DataBits.ToString();
		}

		public void UpdateFlowControl()
		{
			logger.Trace("Setting flow control to {0}", _dataDevice.FlowControl);

			foreach (ToolStripMenuItem item in menuItemFlowControl.DropDownItems)
				item.Checked = item.Text == _dataDevice.FlowControl.GetDescription<FlowControl>();
		}

		public void UpdateParity()
		{
			logger.Trace("Setting parity to {0}", _dataDevice.Parity);

			foreach (ToolStripMenuItem item in menuItemParity.DropDownItems)
				item.Checked = item.Text == _dataDevice.Parity.ToString();
		}

		public void UpdatePortState()
		{
			switch (_dataDevice.PortState)
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
					_dataDevice.PortState = PortState.Closed;
					toolStripButtonConnect.ForeColor = Color.Red;
					break;
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

			logger.Trace("Creating menu {0}", name);

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

		//Initialize application shutdown
		private void Exit(object sender, EventArgs e)
		{
			logger.Info("Menu click: Exit");
			Application.Exit();
		}

		// Why both of these?
		private void HandleKeyPress(object sender, KeyEventArgs e)
		{
			e.Handled = true;

			if (e.KeyValue == 0x0A) // Newline
			{
				_dataDevice.Write((byte)0x0A);
			}
		}

		private void Initialize()
		{
			logger.Trace("Initializing MainForm");
			InitializeComponent();

			_baudRateVals = new List<int>(new[] { 110, 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 });

			SetMonospacedFont();

			if (System.Diagnostics.Debugger.IsAttached)
				saveEntireSessionToolStripMenuItem.Visible = true;

			CreateMenuFrom(dropDownBaud, _baudRateVals, "BaudRate", ChangeCOMParam);
			CreateMenuFrom(menuItemBaud, _baudRateVals, "BaudRate", ChangeCOMParam);
			fileSendLoadingCircle.Alignment = ToolStripItemAlignment.Right;

			SetupFileSendSpinnerSpokes();

			ShowVersionInformation();

			logger.Warn("MainForm initialization complete");
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

		private void SelectCOMPort(object sender, ToolStripItemClickedEventArgs e)
		{
			_dataDevice.DeviceName = e.ClickedItem.Text;
		}

		//TODO Fix SendDroppedText
		private void SendDroppedText(object sender, DragEventArgs e)//Drop handler
		{
			//Serial send the dropped text

			if (e.Effect != DragDropEffects.None)
				return;
		}

		// Why both of these? - they catch from different controls
		private void SendKey(object sender, KeyPressEventArgs e) //Key press handler
		{
			e.Handled = true;

			_dataDevice.Write(e.KeyChar);
		}

		private void DataDeviceOnDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
		{
			AddLine(dataReceivedEventArgs.Data);
		}

		private void DataDeviceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			logger.Warn("BindingShim setting {0}", propertyChangedEventArgs.PropertyName);

			try
			{
				switch (propertyChangedEventArgs.PropertyName)
				{
					case "DeviceName":
						UpdateCOMPort();
						break;

					case "PortState":
						UpdatePortState();
						break;

					case "Baud":
						UpdateBaudRate();
						break;

					case "LoggingState":
						UpdateLoggingState();
						break;

					case "EchoState":
						UpdateEchoState();
						break;

					case "StopBits":
						logger.Trace("Changing stopBits to {0}", _dataDevice.StopBits);

						if (menuItemStopBits == null)
						{
							logger.Error("menuItemStopBits == null");
							return;
						}

						foreach (ToolStripMenuItem item in menuItemStopBits.DropDownItems)
							item.Checked = item.Text == _dataDevice.StopBits.GetDescription<StopBits>();

						break;

					case "DataBits":
						UpdateDataBits();
						break;

					case "FlowControl":
						UpdateFlowControl();
						break;

					case "Parity":
						UpdateParity();
						break;

					case "DeviceStatus":
						toolStripStatusLabelPortSettings.Text = _dataDevice.DeviceStatus;
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

		private void SetMonospacedFont()
		{
			// TODO Install Inconsolata (license?),  Deja Vu Sans Mono (public domain!), or Droid Sans Mono (Apache)
			// Deja Vu Sans Mono: http://dejavu-fonts.org/wiki/index.php?title=Main_Page

			// The lower indices should be preferred fonts. Higher indices should be generic fonts.
			string[] fontList = new string[] { "Consolas", "Inconsolata", "Deja Vu Sans Mono", "Lucida Console", "Courier New", "Courier" };

			foreach (string font in fontList)
			{
				Font test = new System.Drawing.Font(font, 9.5F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);

				if (test.Name == font)
				{
					// Got a font on our list
					IOBox.Font = test;
					logger.Info("Picked font: {0}", test.Name);
					return;
				}
			}

			// No fonts in the list were available, pick the most generic font from the list and allow the system to pick a fallback
			this.IOBox.Font = new Font(fontList[fontList.Length - 1], 10.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
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

		//Show about form
		private void ShowAboutForm(object sender, EventArgs e)
		{
			_aboutBox.Open();
		}

		private void ShowVersionInformation()
		{
			logger.Warn("Version {0}", GetVersion());

			Title = "HyperToken";

#if DEBUG
			Title += " [Debug]";

			logger.Warn("Debug version");
#endif

			Title += " (" + GetVersion() + ')';

			if (System.Diagnostics.Debugger.IsAttached)
			{
				logger.Warn("Debugger attached");
				Title += " [Debugger attached]";
			}
		}

		private void ToggleConnection(object sender, EventArgs e)
		{
			logger.Trace("ToggleConnection");
			_dataDevice.PortState = _dataDevice.PortState == PortState.Open ? PortState.Closed : PortState.Open;
		}

		private void ToggleEcho(object sender, EventArgs e)
		{
			logger.Trace("Toggle Echo");
			_echoer.EchoState = _echoer.EchoState == EchoState.Enabled ? EchoState.Disabled : EchoState.Enabled;
		}

		private void ToggleLogging(object sender, System.EventArgs e)
		{
			logger.Trace("Toggle logging");
			_logger.LoggingState = _logger.LoggingState == LoggingState.Disabled ? LoggingState.Enabled : LoggingState.Disabled;
		}

		//Show file send pane
		private void toolStripButtonSendFile_Click(object sender, EventArgs e)
		{
			fileSendPane1.Visible = !fileSendPane1.Visible;
		}

		//List all COM ports
		private void UpdateCOMPorts(object sender, EventArgs e)
		{
			string[] ports = _dataDevice.Devices;

			if (ports == null)
			{
				logger.Error("No serial ports to list");
				logger.Fatal("TODO We should handle this more gracefully");
				logger.Fatal("Show a 'No serial ports found' item");
				return;
			}

			if (sender is ToolStripDropDownItem)
			{
				var menu = sender as ToolStripDropDownItem;
				menu.DropDownItems.Clear();
				foreach (var port in ports)
					menu.DropDownItems.Add(port);
			}
		}

		#region Pretend buttons for status bar

		private void PretendClick(object sender, MouseEventArgs e)
		{
			if (sender is ToolStripStatusLabel)
				((ToolStripStatusLabel)sender).BorderStyle = Border3DStyle.SunkenInner;
		}

		private void PretendEnter(object sender, EventArgs e)
		{
			if (sender is ToolStripStatusLabel)
				((ToolStripStatusLabel)sender).BorderStyle = Border3DStyle.RaisedInner;
		}

		private void PretendLeave(object sender, EventArgs e)
		{
			if (sender is ToolStripStatusLabel)
				((ToolStripStatusLabel)sender).BorderStyle = Border3DStyle.Flat;
		}

		private void PretendRelease(object sender, MouseEventArgs e)
		{
			if (sender is ToolStripStatusLabel)
				((ToolStripStatusLabel)sender).BorderStyle = Border3DStyle.Flat;
		}

		#endregion Pretend buttons for status bar

		private void UpdateEchoState()
		{
			logger.Trace("Echo set to {0}", _echoer.EchoState);

			switch (_echoer.EchoState)
			{
				case EchoState.Disabled:
					toolStripStatusLabelLocalEcho.Text = Resources.Text_Echo_Off;
					break;

				case EchoState.Enabled:
					toolStripStatusLabelLocalEcho.Text = Resources.Text_Echo_On;
					break;
			}
		}

		private void UpdateLoggingState()
		{
			logger.Trace("Logging set to {0}", _logger.LoggingState);

			switch (_logger.LoggingState)
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

		private void SetLoggingFile(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void FileSendPaneSend(object sender, EventArgs eventArgs, FileSendPane.SerialSendArgs serialSendArgs)
		{
			throw new NotImplementedException();
		}
	}
}