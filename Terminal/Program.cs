using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Bugsense.WPF;
using HyperToken_WinForms_GUI;
using NLog;
using Terminal_Interface;

namespace Terminal
{
	internal class Program
	{
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		public static string GetVersion()
		{
			return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		[STAThread]
		private static void Main()
		{
			//try
			//{
			logger.Trace("Initialize BugSense");
			BugSense.Init("9eacbe2e", GetVersion(), "http://www.bugsense.com/api/errors");
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

			logger.Trace("Initializing advanced JIT");
			AdvancedJIT.SetupJIT();

			logger.Trace("Building IOC container");
			ContainerSetup containerSetup = new ContainerSetup();
			IContainer container = containerSetup.BuildContainer();

			logger.Trace("Running ITerminal");
			if (container.IsRegistered<InitableRunner>())
				container.Resolve<InitableRunner>().Init();
			container.Resolve<TerminalRunner>().Run();

			//}
			//catch (Exception e)
			//{
			//	logger.Fatal(e.Message);
			//	BugSense.SendException(e);
			//}
		}

		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
		{
			// Pop a GUI
			throw (Exception)unhandledExceptionEventArgs.ExceptionObject;
		}
	}
}