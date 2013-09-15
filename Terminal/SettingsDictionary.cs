using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminal.Interface;
using Terminal.Interface.Events;

namespace Terminal
{
	public class SettingsDictionary : ISettings
	{
		private Dictionary<string, object> _settingsStore;

		public SettingsDictionary()
		{
			_settingsStore = new Dictionary<string, object>();
		}

		public void Set(string key, object value)
		{
			_settingsStore[key] = value;
			if (SettingChanged != null)
			{
				var args = new SettingChangedEventArgs(key);
				SettingChanged(this, args);
			}
		}

		public object Get(string key)
		{
			return _settingsStore[key];
		}

		public event SettingChangedEventHandler SettingChanged;
	}
}