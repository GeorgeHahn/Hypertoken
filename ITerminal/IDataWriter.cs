﻿namespace Terminal.Interface
{
	public interface IDataWriter
	{
		int Write(byte[] data);

		int Write(byte data);

		int Write(char data);

		int Write(string data);
	}
}