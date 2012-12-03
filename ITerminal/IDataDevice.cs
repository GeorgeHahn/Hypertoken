using System.Collections.Generic;

namespace Terminal_Interface
{
	public interface IDataDevice : IDataWriter, IDataReader
	{
		new void Close();

		new void Open();

		IEnumerable<string> ListAvailable();

		new bool IsOpen { get; }

		string Name { get; set; }

		string StatusLabel { get; }
	}
}