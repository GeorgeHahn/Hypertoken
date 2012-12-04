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

namespace HyperToken_WinForms_GUI
{
	public class LoggingGui : IMainMenuExtension, IStatusbarExtension
	{
		private readonly ILogger _logger;

		private ToolStripMenuItem _mainMenuItem;
		private ToolStripStatusLabel _statusBarItem;
		private readonly SaveFileDialog selectLoggingFileDialog;

		public LoggingGui(ILogger logger)
		{
			_logger = logger;
			logger.PropertyChanged += LoggerOnPropertyChanged;

			selectLoggingFileDialog = new SaveFileDialog
									   {
										   DefaultExt = "txt",
										   Filter = "Text files|*.txt|All files|*.*",
										   OverwritePrompt = false,
										   Title = "Select Logging File"
									   };
		}

		public ToolStripMenuItem Menu
		{
			get
			{
				if (_mainMenuItem == null)
				{
					_mainMenuItem = new ToolStripMenuItem("Logging");
					_mainMenuItem.DropDownItems.AddRange(new ToolStripItem[]
						    {
							    new ToolStripMenuItem("Enable Logging", null, (s, a) => OnToggleLogging()),
							    new ToolStripMenuItem("Set Destination File", null, (s, a) => OnSetDestinationFile())
						    });
				}

				return _mainMenuItem;
			}
		}

		public ToolStripStatusLabel StatusBarItem
		{
			get
			{
				if (_statusBarItem == null)
				{
					_statusBarItem = new PretendStatusbarButton(Resources.Text_Logging_Disabled, null, (s, a) => OnToggleLogging());
				}

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
					((ToolStripMenuItem)(_mainMenuItem.DropDownItems[0])).Checked = false;
					break;

				case LoggingState.Enabled:
					_statusBarItem.Text = Resources.Text_Logging_Enabled;
					((ToolStripMenuItem)(_mainMenuItem.DropDownItems[0])).Checked = true;
					break;
			}
		}

		private void OnSetDestinationFile()
		{
			GetLoggerFilePath();
		}

		public string GetLoggingFilePath()
		{
			if (selectLoggingFileDialog.ShowDialog() == DialogResult.OK)
				return selectLoggingFileDialog.FileName;

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