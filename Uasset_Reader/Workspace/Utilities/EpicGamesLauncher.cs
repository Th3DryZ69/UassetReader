using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Uasset_Reader.Workspace.Utilities;

public static class EpicGamesLauncher
{
	private const string AppName = "Fortnite";

	private const string LaunchArg = "com.epicgames.launcher://apps/Fortnite?action=launch&silent=true";

	private const string VerifyArg = "com.epicgames.launcher://apps/Fortnite?action=verify&silent=false";

	private static readonly string UnrealEngineLauncher = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Epic\\UnrealEngineLauncher";

	public static string InstallLocation(string InstallLocation = "")
	{
		TryInstallLocation(ref InstallLocation);
		return InstallLocation;
	}

	public static bool TryInstallLocation(ref string InstallLocation)
	{
		if (!Directory.Exists(UnrealEngineLauncher))
		{
			return false;
		}
		if (!File.Exists(UnrealEngineLauncher + "\\LauncherInstalled.dat"))
		{
			return false;
		}
		string text = File.ReadAllText(UnrealEngineLauncher + "\\LauncherInstalled.dat");
		if (string.IsNullOrEmpty(text) || !text.ValidJson())
		{
			return false;
		}
		foreach (JToken item in (IEnumerable<JToken>)JObject.Parse(text)["InstallationList"])
		{
			if (item["AppName"].Value<string>() == "Fortnite")
			{
				string text2 = item["InstallLocation"].Value<string>();
				if (Directory.Exists(text2) && Directory.Exists(text2 + "\\FortniteGame\\Content\\Paks"))
				{
					InstallLocation = text2;
					return true;
				}
			}
		}
		return false;
	}

	public static void Launch()
	{
		TryLaunch();
	}

	public static bool TryLaunch()
	{
		try
		{
			"com.epicgames.launcher://apps/Fortnite?action=launch&silent=true".UrlStart();
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static void Close()
	{
		TryClose();
	}

	public static bool TryClose()
	{
		string[] array = new string[6] { "EpicGamesLauncher", "FortniteLauncher", "FortniteClient-Win64-Shipping", "FortniteClient-Win64-Shipping_BE", "FortniteClient-Win64-Shipping_EAC", "CrashReportClient" };
		foreach (string processName in array)
		{
			try
			{
				Process[] processesByName = Process.GetProcessesByName(processName);
				if (processesByName.Length != 0)
				{
					processesByName[0].Kill();
					processesByName[0].WaitForExit();
				}
			}
			catch
			{
				return false;
			}
		}
		return true;
	}

	public static void Verify()
	{
		TryVerify();
	}

	public static bool TryVerify()
	{
		try
		{
			"com.epicgames.launcher://apps/Fortnite?action=verify&silent=false".UrlStart();
			return true;
		}
		catch
		{
			return false;
		}
	}
}
