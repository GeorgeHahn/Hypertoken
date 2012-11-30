using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminal_Interface.Events
{
	public class OnSettingChangedEventArgs : EventArgs
	{
		private string _settingName;

		public OnSettingChangedEventArgs(string settingName)
			: base()
		{
			_settingName = settingName;
		}

		public string SettingName
		{
			get { return _settingName; }
		}
	}
}