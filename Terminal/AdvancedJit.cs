using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;

namespace Terminal
{
	public static class AdvancedJIT
	{
		// Code from Rick Brewster
		// http://blog.getpaint.net/2012/09/08/using-multi-core-jit-from-net-4-0-if-net-4-5-is-installed/
		public static void SetupJIT()
		{
			Type systemRuntimeProfileOptimizationType = Type.GetType("System.Runtime.ProfileOptimization", false);
			if (systemRuntimeProfileOptimizationType != null)
			{
				MethodInfo setProfileRootMethod = systemRuntimeProfileOptimizationType.GetMethod("SetProfileRoot", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
				MethodInfo startProfileMethod = systemRuntimeProfileOptimizationType.GetMethod("StartProfile", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null);

				if (setProfileRootMethod != null && startProfileMethod != null)
				{
					try
					{
						// Figure out where to put the profile (go ahead and customize this for your application)
						// This code will end up using something like, C:\Users\UserName\AppData\Local\YourAppName\StartupProfile\
						string localSettingsDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
						string localAppSettingsDir = Path.Combine(localSettingsDir, "Hypertoken");
						string profileDir = Path.Combine(localAppSettingsDir, "ProfileOptimization");
						Directory.CreateDirectory(profileDir);

						setProfileRootMethod.Invoke(null, new object[] { profileDir });
						startProfileMethod.Invoke(null, new object[] { "Startup.profile" }); // don’t need to be too clever here
					}
					catch (Exception e)
					{
						logger.DebugException("Failed to set up JIT profiling", e);
					}
				}
			}
		}

		private static Logger logger = LogManager.GetCurrentClassLogger();
	}
}