using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CustomControls;
using HyperToken_WinForms_GUI.Helpers;
using HyperToken_WinForms_GUI.Properties;
using NLog;
using Terminal_GUI_Interface;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Terminal_Interface.Events;
using Terminal_Interface.Exceptions;
using Anotar;

// TODO Add 'human readable version' for Rob
// TODO Custom Baud setting

namespace HyperToken_WinForms_GUI
{
    public partial class MainForm : Form, ITerminal, INotifyPropertyChanged
    {
        private readonly IAboutBox _aboutBox;
        private readonly IEchoer _echoer;
        private readonly ILogger _logger;
        private readonly IFileSender _fileSender;
        private readonly CurrentDataDevice _currentDataDevice;
        private readonly WinformsMainMenuExtender _mainMenuExtender;
        private readonly IEnumerable<IStatusbarExtension> _statusbarExtensions;

        private IDataDevice _dataDevice;
        private IEnumerable<ToolStripMenuItem> _menuExtensions;


        public MainForm(IAboutBox aboutBox,
                        ILogger logger,
                        IEchoer echoer,
                        IFileSender fileSender,
                        CurrentDataDevice dataDevice,
                        WinformsMainMenuExtender mainMenuExtender,
                        IEnumerable<IStatusbarExtension> statusbarExtensions)
        {
            _aboutBox = aboutBox;
            _logger = logger;
            _echoer = echoer;
            _fileSender = fileSender;
            _mainMenuExtender = mainMenuExtender;
            _statusbarExtensions = statusbarExtensions;

            _currentDataDevice = dataDevice;
            _currentDataDevice.PropertyChanged += (sender, args) =>
                {
                    _dataDevice = _currentDataDevice.CurrentDevice;
                    if (_dataDevice == null)
                        return;

                    _dataDevice.PropertyChanged += DataDeviceOnPropertyChanged;
                    _dataDevice.DataReceived += DataDeviceOnDataReceived;
                };

            _dataDevice = _currentDataDevice.CurrentDevice;

            Log.Debug("Mainform object created");
        }

        public MainForm(IAboutBox aboutBox, CurrentDataDevice dataDevice, ILogger logger, WinformsMainMenuExtender mainMenuExtender, IEnumerable<IStatusbarExtension> statusbarExtensions)
            : this(aboutBox, logger, null, null, dataDevice, mainMenuExtender, statusbarExtensions)
        { }

        public MainForm(IAboutBox aboutBox, CurrentDataDevice dataDevice, WinformsMainMenuExtender mainMenuExtender, IEnumerable<IStatusbarExtension> statusbarExtensions)
            : this(aboutBox, null, null, null, dataDevice, mainMenuExtender, statusbarExtensions)
        { }

        public event SaveSessionEventHandler OnSaveSession;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get { return Text; }
            set { Text = value; }
        }

        private void Initialize()
        {
            Log.Debug("Initializing MainForm");
            InitializeComponent();

            SetMonospacedFont();

            if (System.Diagnostics.Debugger.IsAttached)
                saveEntireSessionToolStripMenuItem.Visible = true;

            fileSendLoadingCircle.Alignment = ToolStripItemAlignment.Right;

            _menuExtensions = from menu in _mainMenuExtender.GetMenus()
                        where menu != null
                        select menu;

            foreach (var menu in _menuExtensions)
                menuStrip1.Items.Add(menu);

            if (_statusbarExtensions != null)
                foreach (var statusbarExtension in _statusbarExtensions)
                    statusStrip.Items.Add(statusbarExtension.StatusBarItem);

            SetupFileSendSpinnerSpokes();

            ShowVersionInformation();

            Log.Warn("MainForm initialization complete");
        }

        public void Run()
        {
            Initialize();
            Application.Run(this);
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
                           IOBox.AppendText(line);
                           IOBox.Scrolling.ScrollBy(0, IOBox.Lines.Count);
                       }));
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

        //Initialize application shutdown
        private void Exit(object sender, EventArgs e)
        {
            Log.Info("Menu click: Exit");
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Debug("User clicked menu item: New");
            DialogResult result = MessageBox.Show("Would you like to save current session?", Resources.MainForm_SaveSession_Save_Session, MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Cancel)
            {
                Log.Debug("User canceled");
                return;
            }

            if (result == DialogResult.No)
            {
                Log.Debug("User cleared session");
                IOBox.Text = "";
                return;
            }

            if (result == DialogResult.Yes)
            {
                Log.Debug("User saved session");
                if (SaveSession())
                {
                    Log.Debug("Session cleared");
                    IOBox.Text = "";
                }
                else
                    Log.Debug("Session not cleared");
            }
        }

        private void SaveEntireSessionToolStripMenuItemClick(object sender, EventArgs e)
        {
            SaveSession();
        }

        private bool SaveSession()
        {
            Log.Info("Saving session");
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
                        Log.Debug("Session saved");
                        return true;
                    }
                    Log.Warn("OnSaveSession event has no handlers");
                }
                else
                    Log.Error("Filename null or empty");
            }
            else
                Log.Debug("User canceled");

            Log.Warn("Session not saved");
            return false;
        }

        //TODO Fix SendDroppedText
        // Drop handler
        private void SendDroppedText(object sender, DragEventArgs e)
        {
            //Serial send the dropped text

            if (e.Effect != DragDropEffects.None)
                return;
        }

        // Why both of these? - they catch from different controls
        // Key press handler
        private void SendKey(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

            _dataDevice.Write(e.KeyChar);
        }

        private void DataDeviceOnDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            AddLine(dataReceivedEventArgs.Data);
            if (_logger != null)
                if (_logger.LoggingState == LoggingState.Enabled)
                    _logger.Write(dataReceivedEventArgs.Data);
        }

        private void DataDeviceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "PortState")
                UpdatePortState();
        }

        // TODO Install Inconsolata (license?),  Deja Vu Sans Mono (public domain!), or Droid Sans Mono (Apache)
        // Deja Vu Sans Mono: http://dejavu-fonts.org/wiki/index.php?title=Main_Page
        private void SetMonospacedFont()
        {
            // The lower indices should be preferred fonts. Higher indices should be generic fonts.
            var fontList = new[] { "Consolas", "Inconsolata", "Deja Vu Sans Mono", "Lucida Console", "Courier New", "Courier" };

            foreach (string font in fontList)
            {
                var thisFont = new Font(font, 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);

                if (thisFont.Name == font)
                {
                    // Got a font on our list
                    IOBox.Font = thisFont;
                    Log.Info("Picked font: {0}", thisFont.Name);
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
            Log.Warn("Version {0}", GetVersion());

            Title = "HyperToken";

#if DEBUG
            Title += " [Debug]";

            Log.Warn("Debug version");
#endif

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Title += " (" + GetVersion() + ')';

                Log.Warn("Debugger attached");
                Title += " [Debugger attached]";
            }
        }

        private void ToggleConnection(object sender, EventArgs e)
        {
            Log.Debug("ToggleConnection");
            if (_dataDevice == null)
            {
                Log.Debug("No dataDevice");
                return;
            }

            _dataDevice.PortState = _dataDevice.PortState == PortState.Open ? PortState.Closed : PortState.Open;
        }

        //Show file send pane
        private void toolStripButtonSendFile_Click(object sender, EventArgs e)
        {
            fileSendPane1.Visible = !fileSendPane1.Visible;
        }

        private void FileSendPaneSend(object sender, EventArgs eventArgs, FileSendPane.SerialSendArgs serialSendArgs)
        {
            throw new NotImplementedException();
        }
    }
}