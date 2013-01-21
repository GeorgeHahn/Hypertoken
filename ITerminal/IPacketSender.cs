namespace Terminal_Interface
{
    public interface IPacketSender
    {
        byte[] GetPacketToSend(string data);
        string Name { get; }
    }
}