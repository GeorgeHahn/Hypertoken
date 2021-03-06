﻿using System;

namespace Terminal.Interface.Exceptions
{
	public class FileSelectionCanceledException : ApplicationException
	{
		public FileSelectionCanceledException()
			: base()
		{ }

		public FileSelectionCanceledException(string message)
			: base(message)
		{ }

		public FileSelectionCanceledException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}