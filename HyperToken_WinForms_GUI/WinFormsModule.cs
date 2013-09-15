using Autofac;
using HyperToken.WinForms.Menus;
using Terminal.Interface;
using Terminal.Interface.GUI;

namespace HyperToken.WinForms
{
    public class WinFormsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Initializer>().As<IInitable>();
            builder.RegisterType<MainForm>().As<ITerminal>();
            builder.RegisterType<AboutBox>().As<IAboutBox>();

            builder.RegisterType<WinformsMainMenuExtender>();

            builder.RegisterType<LoggingGui>().As<IMainMenuExtension>().SingleInstance();
            builder.RegisterType<LoggingGui>().As<IStatusbarExtension>().SingleInstance();
            builder.RegisterType<PacketParserGUI>().As<IMainMenuExtension>().SingleInstance();
            builder.RegisterType<FileSendGUI>().As<IToolbarExtension>().SingleInstance();

            builder.RegisterType<SerialStatusbarPortMenu>().As<IStatusbarExtension>();
            builder.RegisterType<SerialStatusbarStatusLabel>().As<IStatusbarExtension>();
            builder.RegisterType<SerialStatusbarBaudMenu>().As<IStatusbarExtension>();

            builder.RegisterType<SerialStatusbarPortMenu>();
            builder.RegisterType<SerialStatusbarBaudMenu>();
            builder.RegisterType<CurrentDeviceSerialStatusLabel>().As<IStatusbarExtension>();

            builder.RegisterType<SerialMenu>().As<IMainMenuExtension>().SingleInstance();
            builder.RegisterType<AboutMenu>().As<IMainMenuExtension>().SingleInstance();

            builder.RegisterType<FlowControlMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<BaudRateMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<PortMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<ParityMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<StopBitsMenu>().As<ISerialSettingsMenu>();
            builder.RegisterType<DataBitsMenu>().As<ISerialSettingsMenu>();

            builder.RegisterType<HidMenu>().As<IMainMenuExtension>();
            builder.RegisterType<DeviceSelectionMenu>().As<IHidSettingsMenu>();
            builder.RegisterType<HidDeviceConnection>().As<IHidSettingsMenu>();
        }
    }
}
