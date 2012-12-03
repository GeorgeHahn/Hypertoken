using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Terminal_Interface;

namespace Terminal
{
	internal class InitableRunner
	{
		private readonly IInitable _initable;

		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		public InitableRunner(IInitable initable)
		{
			logger.Trace("InitableRunner created");
			_initable = initable;
		}

		public void Init()
		{
			logger.Trace("Initializing initable");
			_initable.Init();
		}
	}
}