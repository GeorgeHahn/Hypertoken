using System;
using System.ComponentModel;
using System.IO;
using Anotar;
using Anotar.NLog;
using NLog;
using Terminal.Interface;
using Terminal.Interface.Enums;

namespace Terminal
{
	public class FileLogger : ILogger
	{
		private StreamWriter _loggingStreamWriter;
		private FileStream _loggingStream;
		private string _loggingFilePath;

		private void Open()
		{
			if (_loggingFilePath == null)
				return;

			LogTo.Debug("Creating new FileStream for _loggingStream");
			_loggingStream = File.Open(_loggingFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
			_loggingStreamWriter = new StreamWriter(_loggingStream);
		}

		private void Close()
		{
			if (_loggingStream == null)
				return;

			LogTo.Debug("Flushing _loggingStream");
			_loggingStreamWriter.Flush();
			_loggingStream.Flush();

			LogTo.Debug("Closing _loggingStream");
			_loggingStreamWriter.Close();
			_loggingStream.Close();
		}

		public string LoggingFilePath
		{
			get { return _loggingFilePath; }
			set
			{
				LogTo.Debug("Setting LoggingFilePath to {0}", value);
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

		// TODO implement proper return values or change to voids
		public int Write(byte[] data)
		{
			if (LoggingState != LoggingState.Enabled)
				return 0;

			_loggingStreamWriter.Write(data);
			return 0;
		}

		public int Write(byte data)
		{
			if (LoggingState != LoggingState.Enabled)
				return 0;

			_loggingStreamWriter.Write(data);
			return 0;
		}

		public int Write(char data)
		{
			if (LoggingState != LoggingState.Enabled)
				return 0;

			_loggingStreamWriter.Write(data);
			return 0;
		}

		public int Write(string data)
		{
			if (LoggingState != LoggingState.Enabled)
				return 0;

			_loggingStreamWriter.Write(data);
			return 0;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}