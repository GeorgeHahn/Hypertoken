using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anotar;
using NLog;
using Terminal_Interface;

namespace Terminal
{
	internal class InitableRunner
	{
		private readonly IInitable _initable;

		public InitableRunner(IInitable initable)
		{
			Log.Debug("InitableRunner created");
			_initable = initable;
		}

		public void Init()
		{
			Log.Debug("Initializing initable");
			_initable.Init();
		}
	}
}