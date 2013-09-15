using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Terminal_GUI_Interface;
using Terminal_Interface;

namespace HyperToken.WinFormsGUI
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
        }
    }
}
