using System.Windows;
using NLog;

namespace HyperToken_Avalon_GUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			logger.Trace("Initializing components");

			// This next line will throw an error if our DLL doesn't have access to its dependencies.
			// This situation arises when it is loaded from a separate folder at runtime.
			InitializeComponent();
			logger.Trace("Components initialized");
		}

		private PseudoViewModel _viewModel;

		public PseudoViewModel viewModel
		{
			get { return _viewModel; }
			set
			{
				DataContext = value;
				_viewModel = value;
			}
		}

		private static Logger logger = LogManager.GetCurrentClassLogger();

		private void TextBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			viewModel.KeyPressed('a');
		}
	}
}