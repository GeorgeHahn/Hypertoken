using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Anotar.NLog;
using CustomControls;
using HyperToken.WinFormsGUI.Properties;
using Terminal.Interface;
using Terminal.Interface.Enums;
using Terminal.Interface.Events;
using Terminal.Interface.Exceptions;
using Anotar;
// TODO Add 'human readable version' for Rob
// TODO Custom Baud setting
using Terminal.Interface.GUI;

namespace HyperToken.WinFormsGUI
{
    public partial class MainForm : Form, ITerminal
    {
        private readonly IAboutBox _aboutBox;
        private readonly ILogger _logger;
        private readonly CurrentDataDevice _currentDataDevice;
        private readonly WinformsMainMenuExtender _mainMenuExtender;
        private readonly IEnumerable<IStatusbarExtension> _statusbarExtensions;
        private readonly IEnumerable<IToolbarExtension> _toolbarExtensions;

        private IDataDevice _dataDevice;
        private IEnumerable<ToolStripMenuItem> _menuExtensions;


        public MainForm(IAboutBox aboutBox,
                        ILogger logger,
                        CurrentDataDevice dataDevice,
                        WinformsMainMenuExtender mainMenuExtender,
                        IEnumerable<IStatusbarExtension> statusbarExtensions,
                        IEnumerable<IToolbarExtension> toolbarExtensions)
        {
            _aboutBox = aboutBox;
            _logger = logger;
            _mainMenuExtender = mainMenuExtender;
            _statusbarExtensions = statusbarExtensions;
            _toolbarExtensions = toolbarExtensions;

            _currentDataDevice = dataDevice;
            _currentDataDevice.PropertyChanged += (sender, args) =>
                                                    {
                                                        var oldDataDevice = _dataDevice;
                                                        _dataDevice = _currentDataDevice.CurrentDevice;
                                                        if (_dataDevice != null)
                                                        {
                                                            _dataDevice.PropertyChanged += DataDeviceOnPropertyChanged;
                                                            _dataDevice.DataReceived += DataDeviceOnDataReceived;
                                                        }
                                                        if (oldDataDevice != null)
                                                        {
                                                            oldDataDevice.PropertyChanged -= DataDeviceOnPropertyChanged;
                                                            oldDataDevice.DataReceived -= DataDeviceOnDataReceived;
                                                        }
                                                    };

            _dataDevice = _currentDataDevice.CurrentDevice;

            LogTo.Debug("Mainform object created");
        }

        public event SaveSessionEventHandler OnSaveSession;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get { return Text; }
            set { Text = value; }
        }

        private void Initialize()
        {
            LogTo.Debug("Initializing MainForm");
            InitializeComponent();

            SetMonospacedFont();

            if (System.Diagnostics.Debugger.IsAttached)
                saveEntireSessionToolStripMenuItem.Visible = true;

            fileSendLoadingCircle.Alignment = ToolStripItemAlignment.Right;

            _menuExtensions = _mainMenuExtender.GetMenus();

            if (_menuExtensions != null)
            {
                LogTo.Trace("Registered {0} menu extensions", _menuExtensions.Count());
                foreach (var mainMenuExtension in _menuExtensions)
                    menuStrip1.Items.Add(mainMenuExtension);
            }

            if (_statusbarExtensions != null)
            {
                LogTo.Trace("Registered {0} statusbar extensions", _statusbarExtensions.Count());
                foreach (var statusbarExtension in _statusbarExtensions)
                    statusStrip.Items.Add(statusbarExtension.StatusBarItem);
            }

            if (_toolbarExtensions != null)
            {
                LogTo.Trace("Registered {0} toolbar extensions", _toolbarExtensions.Count());
                foreach (var extension in _toolbarExtensions)
                    toolStrip1.Items.Add(extension.ToolBarItem);
            }

            SetupFileSendSpinnerSpokes();

            ShowVersionInformation();

            LogTo.Warn("MainForm initialization complete");
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
            LogTo.Info("Menu click: Exit");
            Application.Exit();
        }

        // Send LF
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
            LogTo.Debug("User clicked menu item: New");
            DialogResult result = MessageBox.Show("Would you like to save current session?", Resources.MainForm_SaveSession_Save_Session, MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Cancel)
            {
                LogTo.Debug("User canceled");
                return;
            }

            if (result == DialogResult.No)
            {
                LogTo.Debug("User cleared session");
                IOBox.Text = "";
                return;
            }

            if (result == DialogResult.Yes)
            {
                LogTo.Debug("User saved session");
                if (SaveSession())
                {
                    LogTo.Debug("Session cleared");
                    IOBox.Text = "";
                }
                else
                    LogTo.Debug("Session not cleared");
            }
        }

        private void SaveEntireSessionToolStripMenuItemClick(object sender, EventArgs e)
        {
            SaveSession();
        }

        private bool SaveSession()
        {
            LogTo.Info("Saving session");
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
                        LogTo.Debug("Session saved");
                        return true;
                    }
                    LogTo.Warn("OnSaveSession event has no handlers");
                }
                else
                    LogTo.Error("Filename null or empty");
            }
            else
                LogTo.Debug("User canceled");

            LogTo.Warn("Session not saved");
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

            if(_dataDevice != null)
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
                    LogTo.Info("Picked font: {0}", thisFont.Name);
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
            LogTo.Warn("Version {0}", GetVersion());
            Title = "HyperToken";
#if DEBUG
            Title += " [Debug]";
            LogTo.Warn("Debug version");
#endif

            if (!System.Diagnostics.Debugger.IsAttached)
                return;

            LogTo.Warn("Debugger attached");
            Title += " (" + GetVersion() + ')';
            Title += " [Debugger attached]";
        }

        private void ToggleConnection(object sender, EventArgs e)
        {
            LogTo.Debug("ToggleConnection");
            if (_dataDevice == null)
            {
                LogTo.Debug("No dataDevice");
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