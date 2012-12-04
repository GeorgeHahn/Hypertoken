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
	public class LoggingGui : IMainMenuExtension
	{
		private readonly ILogger _logger;

		private ToolStripMenuItem _mainMenuItem;
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
							    new ToolStripMenuItem("Enable Logging", null, OnToggleLogging),
							    new ToolStripMenuItem("Set Destination File", null, OnSetDestinationFile)
						    });
				}

				return _mainMenuItem;
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

					//toolStripLoggingEnabled.Text = Resources.Text_Logging_Disabled;
					((ToolStripMenuItem)(_mainMenuItem.DropDownItems[0])).Checked = false;
					break;

				case LoggingState.Enabled:

					//toolStripLoggingEnabled.Text = Resources.Text_Logging_Enabled;
					((ToolStripMenuItem)(_mainMenuItem.DropDownItems[0])).Checked = true;
					break;
			}
		}

		private void OnSetDestinationFile(object sender, EventArgs eventArgs)
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

		private void OnToggleLogging(object sender, EventArgs eventArgs)
		{
			Log.Debug("Toggle logging");
			if (_logger.LoggingFilePath == null)
				if (!GetLoggerFilePath())
					return;

			_logger.LoggingState = _logger.LoggingState == LoggingState.Disabled ? LoggingState.Enabled : LoggingState.Disabled;
		}
	}
}