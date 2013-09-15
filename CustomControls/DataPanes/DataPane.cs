namespace Sniffer
{
    public interface DataPane
    {
        void AddData(long[] timeStamp, byte[] addr, byte[][] data);
    }
}
