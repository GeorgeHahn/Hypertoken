using System;

namespace Terminal.Interface.Events
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