using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using HyperToken_WinForms_GUI;
using PacketParser;
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
            builder.RegisterType<SerialPortDataHandler>().As<ISerialPort>().SingleInstance();
            builder.RegisterType<HIDDataHandler>().As<IDataDevice>().SingleInstance();

            //builder.RegisterType<HIDDataHandler>().As<ISerialPort>();

            // GUI Extensions
            builder.RegisterType<LoggingGui>().As<IMainMenuExtension>().SingleInstance();
            builder.RegisterType<LoggingGui>().As<IStatusbarExtension>().SingleInstance();
            builder.RegisterType<PacketParserGUI>().As<IMainMenuExtension>().SingleInstance();
            builder.RegisterType<SerialMenu>().As<IMainMenuExtension>();

            builder.RegisterType<SerialStatusbarPortMenu>().As<IStatusbarExtension>();
            builder.RegisterType<SerialStatusbarStatusLabel>().As<IStatusbarExtension>();
            builder.RegisterType<SerialStatusbarBaudMenu>().As<IStatusbarExtension>();

            builder.RegisterType<SerialStatusbarPortMenu>();
            builder.RegisterType<SerialStatusbarBaudMenu>();

            builder.RegisterType<CurrentDeviceSerialStatusLabel>().As<IStatusbarExtension>();

            builder.RegisterType<FlowControlMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<BaudRateMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<PortMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<ParityMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<StopBitsMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<DataBitsMenu>().As<ISerialSettingsMenu>();

            builder.RegisterType<WinformsMainMenuExtender>();

            builder.RegisterType<HidMenu>().As<IMainMenuExtension>();
            builder.RegisterType<DeviceSelectionMenu>().As<IHidSettingsMenu>();
            builder.RegisterType<HidDeviceConnection>().As<IHidSettingsMenu>();

            builder.RegisterType<CurrentDataDevice>().SingleInstance();
            builder.RegisterType<CurrentPacketInterpreter>().SingleInstance();

            builder.RegisterType<PacketParserHandler>().SingleInstance();
            builder.RegisterType<StringPacketInterpreter>().As<IPacketInterpreter>().SingleInstance();
            builder.RegisterType<PythonPacketParser>().As<IPacketInterpreter>().SingleInstance();
            builder.RegisterType<RawPacketParser>().As<IPacketInterpreter>().SingleInstance();


            // Logger wiring
            builder.RegisterType<FileLogger>().As<ILogger>().SingleInstance();

            // Application wiring
            builder.RegisterType<TerminalRunner>();
            builder.RegisterType<InitableRunner>();

            return builder.Build();
        }
    }
}