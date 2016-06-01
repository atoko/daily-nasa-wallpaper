using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceADay
{
	class RegistryConfig
	{
		static private RegistryKey startupKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
		static private RegistryKey configKey = Registry.CurrentUser.CreateSubKey(String.Format("SOFTWARE\\atoko\\{0}", APP_KEY));
		private const string APP_KEY = "spacetoday";

		static public bool OnStartup(bool? enable)
		{
			if (enable == true)
			{
				startupKey.SetValue(APP_KEY, System.Reflection.Assembly.GetEntryAssembly().Location);
			}
			else
			{
				startupKey.DeleteValue(APP_KEY);
			}

			return startupKey.GetValue(APP_KEY) == null;
		}

		static public T Get<T>(string key)
		{
			return (T)configKey.GetValue(key);
		}
		static public void Set(string key, string value)
		{
			if (value != null)
			{
				configKey.SetValue(key, value);
			}
			else
			{
				configKey.DeleteValue(value);
			}
		}
	}
}
