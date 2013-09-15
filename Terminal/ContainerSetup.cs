using System.Reflection;
using Autofac;
using Terminal.GUI;
using Terminal.Interface;
using Terminal.Interface.GUI;

namespace Terminal
{
    public class ContainerSetup
    {
        public IContainer BuildWinFormsContainer()
        {
            var builder = new ContainerBuilder();

            // WinForms GUI wiring
            builder.RegisterAssemblyModules(new[] { Assembly.LoadFrom("HyperToken.WinFormsGUI.dll") });

            // Packetparser wiring
            builder.RegisterAssemblyModules(new[] { Assembly.LoadFrom("PacketParser.dll") });

            // Serial port wiring
            builder.RegisterType<SerialPortDataHandler>().As<ISerialPort>().SingleInstance();
            builder.RegisterType<HIDDataHandler>().As<IDataDevice>().SingleInstance();

            // GUI Extensions
            builder.RegisterType<SerialMenu>().As<IMainMenuExtension>().SingleInstance();
            builder.RegisterType<AboutGUI>().As<IMainMenuExtension>().SingleInstance();

            builder.RegisterType<FlowControlMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<BaudRateMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<PortMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<ParityMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<StopBitsMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<DataBitsMenu>().As<ISerialSettingsMenu>();

            builder.RegisterType<HidMenu>().As<IMainMenuExtension>();
            builder.RegisterType<DeviceSelectionMenu>().As<IHidSettingsMenu>();
            builder.RegisterType<HidDeviceConnection>().As<IHidSettingsMenu>();

            builder.RegisterType<CurrentDataDevice>().SingleInstance();
            builder.RegisterType<CurrentPacketInterpreter>().SingleInstance();
            builder.RegisterType<CurrentPacketParser>().SingleInstance();

            // Logger wiring
            builder.RegisterType<FileLogger>().As<ILogger>().SingleInstance();

            // Application wiring
            builder.RegisterType<InitableRunner>();

            return builder.Build();
        }
    }
}