namespace Terminal.Interface
{
    public interface IPacketSender
    {
        byte[] GetPacketToSend(string data);
        string Name { get; }
    }
}