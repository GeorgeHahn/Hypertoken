namespace Terminal.Interface
{
    public interface IPacketInterpreter
    {
        string Name { get; }

        void Create();
        void Release();

        string InterpretPacket(byte[] packet);
    }

    public interface IHIDPreparser
    {
        string Name { get; }
        byte[] InterpretPacket(byte[] packet);
    }
}