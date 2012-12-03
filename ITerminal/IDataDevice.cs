namespace Terminal_Interface
{
	public interface IDataDevice : IDataWriter, IDataReader
	{
		new void Close();

		new void Open();

		new bool IsOpen { get; }
	}
}