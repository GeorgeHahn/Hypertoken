using System;
using System.Reflection;
using System.Threading.Tasks;
using Anotar.NLog;
using Autofac;
using Bugsense.WPF;
using Terminal.Interface;

namespace Terminal
{
	internal class Program
	{
		public static string GetVersion()
		{
			return Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		[STAThread]
		private static void Main()
		{
			//try
			//{

		    Task.Factory.StartNew(() =>
		    {
		        NLog.LogManager.ThrowExceptions = true;

		        LogTo.Debug("Initialize BugSense");
		        BugSense.Init("9eacbe2e", GetVersion(), "http://www.bugsense.com/api/errors");
		        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

		        LogTo.Debug("Initializing advanced JIT");
		        AdvancedJIT.SetupJIT();
		    });

            LogTo.Debug("Building IOC container");
			ContainerSetup containerSetup = new ContainerSetup();
			var container = containerSetup.BuildWinFormsContainer();
		    //var container = containerSetup.BuildAvalonContainer();

            LogTo.Debug("Running ITerminal");
			if (container.IsRegistered<InitableRunner>())
				container.Resolve<InitableRunner>().Init();
            container.Resolve<ITerminal>().Run();

			//}
			//catch (Exception e)
			//{
			//	LogTo.Fatal(e.Message);
			//	BugSense.SendException(e);
			//	MessageBox.Show(e.Message, "Unhandled exception");
			//}
		}

		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
		{
			// Pop a GUI
			var e = (Exception)unhandledExceptionEventArgs.ExceptionObject;
			BugSense.SendException(e);
			throw e;
		}
	}
}