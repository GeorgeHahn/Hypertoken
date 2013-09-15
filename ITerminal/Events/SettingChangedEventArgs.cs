using System;

namespace Terminal.Interface.Events
{
	public class SettingChangedEventArgs : EventArgs
	{
		private string _changedSetting;

		public SettingChangedEventArgs(string changedSetting)
			: base()
		{
			_changedSetting = changedSetting;
		}

		public string ChangedSetting
		{
			get { return _changedSetting; }
		}
	}
}