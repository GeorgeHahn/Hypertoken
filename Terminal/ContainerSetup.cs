using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using HyperToken_WinForms_GUI;
using Terminal_GUI_Interface;
using Terminal_Interface;

namespace Terminal
{
	public class ContainerSetup
	{
		private ContainerBuilder builder;

		public IContainer BuildContainer()
		{
			builder = new ContainerBuilder();

			// WinForms GUI wiring
			builder.RegisterType<HyperToken_WinForms_GUI.Initializer>().As<IInitable>();
			builder.RegisterType<HyperToken_WinForms_GUI.MainForm>().As<ITerminal>();
			builder.RegisterType<HyperToken_WinForms_GUI.AboutBox>().As<IAboutBox>();

			// Serial port wiring
			builder.RegisterType<SerialPortDataHandler>().As<ISerialPort>();

			//builder.RegisterType<HIDDataHandler>().As<ISerialPort>();

			// GUI Extensions
			builder.RegisterType<LoggingGui>().As<IMainMenuExtension>();

			// Logger wiring
			builder.RegisterType<FileLogger>().As<ILogger>();

			// Application wiring
			builder.RegisterType<TerminalRunner>();
			builder.RegisterType<InitableRunner>();

			return builder.Build();
		}
	}
}