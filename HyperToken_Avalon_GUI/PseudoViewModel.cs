using System.ComponentModel;
using System.Windows;
using NLog;
using Terminal_Interface;
using Terminal_Interface.Enums;

namespace HyperToken_Avalon_GUI
{
	public class PseudoViewModel : ITerminal, INotifyPropertyChanged
	{
		private IBackend _backend;

		public void TrimLines(int trimTo)
		{
			throw new System.NotImplementedException();
		}

		public void SetBackend(IBackend backend)
		{
			this._backend = backend;
		}

		public event SendFileEventHandler OnSendFile;

		public event SetLoggingPathEventHandler OnSetLoggingPath;

		public event OnKeyPressedEventHandler OnKeyPressed;

		public event SaveSessionEventHandler OnSaveSession;

		private string _title = "";

		public string Title { get; set; }

		public void OnTitleChanged()
		{
			logger.Debug("Title set to {0}", Title);
		}

		public string LoggingFilePath { get; set; }

		public LoggingState loggingState { get; set; }

		public EchoState echoState { get; set; }

		public PortState portState { get; set; }

		private void OnportStateChanged()
		{
			logger.Trace("Setting port to {0}", portState);
		}

		public FileSendState fileSendState { get; set; }

		public string COMPort { get; set; }

		private void OnCOMPortChanged()
		{
			logger.Debug("COMPort setting to {0}", COMPort);
		}

		public int baud { get; set; }

		public StopBits stopBits { get; set; }

		private void OnstopBitsChanged()
		{
			logger.Trace("StopBits setting to {0}", stopBits);
		}

		public int dataBits { get; set; }

		public FlowControl flowControl { get; set; }

		public Parity parity { get; set; }

		// TODO Will need a message from backend to tell PseudoViewModel to kick the view with a PropertyChanged event
		public string[] serialPorts
		{
			get
			{
				return _backend.GetSerialPorts();
			}
			set { } // Empty to allow WPF binding
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void Changed(string prop)
		{
			PropertyChangedEventHandler handler = PropertyChanged;

			if (handler != null)
				handler(this, new PropertyChangedEventArgs(prop));
		}

		#endregion INotifyPropertyChanged Members

		// OLD API BELOW

		public void AppendTitle(string title)
		{
			logger.Trace("Ignoring old API call: AppendTitle({0})", title);
		}

		public void AddLine(string line)
		{
			logger.Trace("Adding line: {0}", line);
		}

		public string GetLoggingFilePath()
		{
			logger.Trace("GetLoggingFilePath()");
			return "";
		}

		public void SetLoggingState(LoggingState state)
		{
			logger.Trace("SetLoggingState {0}", state);
		}

		public void SetFileSendState(FileSendState fileSendState)
		{
			logger.Trace("FileSendState {0}", fileSendState);
		}

		public void AddChar(char c)
		{
			logger.Trace("AddChar {0}", c);
		}

		public void Run()
		{
			Application app = new Application();
			MainWindow window = new MainWindow();
			window.viewModel = this;

			Changed("serialPorts");

			app.Run(window);
		}

		public PseudoViewModel()
		{
			logger.Trace("Instantiated PseudoViewModel");
		}

		private static Logger logger = LogManager.GetCurrentClassLogger();

		internal void KeyPressed(char p)
		{
			_backend.KeyPressed(p);
		}
	}
}