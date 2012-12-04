using System;
using System.IO;
using Anotar;
using NLog;
using Terminal_Interface;
using Terminal_Interface.Enums;

namespace Terminal
{
	public class FileLogger : ILogger
	{
		private FileStream _loggingStream;
		private string _loggingFilePath;

		public FileLogger()
		{
		}

		private void Open()
		{
			if (_loggingStream == null)
				return;

			Log.Debug("Creating new FileStream for _loggingStream");
			_loggingStream = File.Open(_loggingFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
		}

		private void Close()
		{
			if (_loggingStream == null)
				return;

			Log.Debug("Closing _loggingStream");

			_loggingStream.Close();
		}

		public string LoggingFilePath
		{
			get { return _loggingFilePath; }
			set
			{
				Log.Debug("Setting LoggingFilePath to {0}", value);
				_loggingFilePath = value;

				Close();

				Open();
			}
		}

		private LoggingState _loggingState;

		public LoggingState LoggingState
		{
			get { return _loggingState; }
			set
			{
				if (_loggingFilePath == null) // TODO subclass UserException for a more appropriate exception here
					throw new NullReferenceException("LoggingFilePath must be set before logging is enabled");

				_loggingState = value;
			}
		}

		public int Write(byte[] data)
		{
			if (LoggingState != LoggingState.Enabled)
				return 0;

			throw new System.NotImplementedException();
		}

		public int Write(byte data)
		{
			if (LoggingState != LoggingState.Enabled)
				return 0;

			throw new System.NotImplementedException();
		}

		public int Write(char data)
		{
			if (LoggingState != LoggingState.Enabled)
				return 0;

			throw new System.NotImplementedException();
		}

		public int Write(string data)
		{
			if (LoggingState != LoggingState.Enabled)
				return 0;

			throw new System.NotImplementedException();
		}
	}
}