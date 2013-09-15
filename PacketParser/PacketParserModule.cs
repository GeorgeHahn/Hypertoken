using Autofac;
using Terminal.Interface;

namespace PacketParser
{
    public class PacketParserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StringPacketInterpreter>().As<IPacketInterpreter>().SingleInstance();
            builder.RegisterType<PythonPacketParser>().As<IPacketInterpreter>().SingleInstance();
            builder.RegisterType<RawPacketParser>().As<IPacketInterpreter>().SingleInstance();
            builder.RegisterType<HIDPreparser>().As<IHIDPreparser>().SingleInstance();
        }
    }
}
