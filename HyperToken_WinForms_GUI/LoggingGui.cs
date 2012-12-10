using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Anotar;
using HyperToken_WinForms_GUI.Properties;
using Terminal_GUI_Interface;
using Terminal_Interface;
using Terminal_Interface.Enums;
using Terminal_Interface.Exceptions;
using Menu = Terminal_GUI_Interface.Menu;

namespace HyperToken_WinForms_GUI
{
	public class LoggingGui : IMainMenuExtension, IStatusbarExtension
	{
		private readonly ILogger _logger;

		private Menu _mainMenuItem;
		private ToolStripStatusLabel _statusBarItem;
		private readonly SaveFileDialog _selectLoggingFileDialog;

		public LoggingGui(ILogger logger)
		{
			_logger = logger;
			logger.PropertyChanged += LoggerOnPropertyChanged;

			_selectLoggingFileDialog = new SaveFileDialog
									   {
										   DefaultExt = "txt",
										   Filter = "Text files|*.txt|All files|*.*",
										   OverwritePrompt = false,
										   Title = "Select Logging File"
									   };
		}

		public Menu Menu
		{
			get
			{
				if (_mainMenuItem == null)
				{
					_mainMenuItem = new Menu("Logging");
					_mainMenuItem.Items.Add(new Menu("Enable Logging", (s, a) => OnToggleLogging()));
					_mainMenuItem.Items.Add(new Menu("Set Destination File", (s, a) => OnSetDestinationFile()));
				}

				return _mainMenuItem;
			}
		}

		public ToolStripItem StatusBarItem
		{
			get
			{
				if (_statusBarItem == null)
					_statusBarItem = new PretendStatusbarButton(Resources.Text_Logging_Disabled, null, (s, a) => OnToggleLogging());

				return _statusBarItem;
			}
		}

		private void LoggerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Log.Warn("Logger binding shim fired for {0}", propertyChangedEventArgs.PropertyName);
			switch (propertyChangedEventArgs.PropertyName)
			{
				case "LoggingState":
					UpdateLoggingState();
					break;
			}
		}

		private void UpdateLoggingState()
		{
			Log.Debug("Logging set to {0}", _logger.LoggingState);

			switch (_logger.LoggingState)
			{
				case LoggingState.Disabled:
					_statusBarItem.Text = Resources.Text_Logging_Disabled;
					_mainMenuItem.Items[0].Checked = false;
					break;

				case LoggingState.Enabled:
					_statusBarItem.Text = Resources.Text_Logging_Enabled;
					_mainMenuItem.Items[0].Checked = true;
					break;
			}
		}

		private void OnSetDestinationFile()
		{
			GetLoggerFilePath();
		}

		public string GetLoggingFilePath()
		{
			if (_selectLoggingFileDialog.ShowDialog() == DialogResult.OK)
				return _selectLoggingFileDialog.FileName;

			throw new FileSelectionCanceledException();
		}

		private bool GetLoggerFilePath()
		{
			try
			{
				_logger.LoggingFilePath = GetLoggingFilePath();
				return true;
			}
			catch (FileSelectionCanceledException)
			{
				return false;
			}
		}

		private void OnToggleLogging()
		{
			Log.Debug("Toggle logging");
			if (_logger.LoggingFilePath == null)
				if (!GetLoggerFilePath())
					return;

			_logger.LoggingState = _logger.LoggingState == LoggingState.Disabled ? LoggingState.Enabled : LoggingState.Disabled;
		}
	}
}