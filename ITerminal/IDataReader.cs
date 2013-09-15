using Terminal.Interface.Events;

namespace Terminal.Interface
{
	public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);

	public interface IDataReader
	{
		event DataReceivedEventHandler DataReceived;
	}
}