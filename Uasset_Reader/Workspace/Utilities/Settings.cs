using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Events;

namespace Uasset_Reader.Workspace.Utilities;

public static class Settings
{
	public enum Type
	{
		Installtion,
		UtocModification,
		FormatSwap,
		Console,
		Presence,
		IsNew
	}

	public static readonly string Path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Uasset-Reader-v2";

	private static Dictionary<string, dynamic> Cache = new Dictionary<string, object>();

	private static JObject Parse;

	public static void Initialize()
	{
		if (File.Exists(Path + "\\Key.config"))
		{
			Directory.Delete(Path, recursive: true);
		}
		string[] array = new string[3]
		{
			Path,
			Path + "\\DLLS",
			Path + "\\LOGS"
		};
		foreach (string path in array)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
		Serilog();
		if (File.Exists(Path + "\\Settings.json"))
		{
			string text = File.ReadAllText(Path + "\\Settings.json");
			if (text.ValidJson())
			{
				Parse = JObject.Parse(text);
				array = Enum.GetNames(typeof(Type));
				foreach (string propertyName in array)
				{
					if (!Parse.ContainsKey(propertyName))
					{
						Create();
						break;
					}
				}
			}
			else
			{
				Create();
			}
		}
		else
		{
			Create();
		}
		Populate();
		PopulateDLLS();
		Log.Information("Successfully initialized settings");
	}

	public static void Create()
	{
		try
		{
			JObject jObject = (Parse = JObject.FromObject(new
			{
				Installtion = EpicGamesLauncher.InstallLocation(),
				UtocModification = false,
				FormatSwap = false,
				Console = true,
				Presence = true,
				IsNew = true
			}));
			if (File.Exists(Path + "\\Settings.json"))
			{
				File.Delete(Path + "\\Settings.json");
			}
			File.WriteAllText(Path + "\\Settings.json", jObject.ToString());
			Log.Information("Successfully created settings file to " + Path + "\\Settings.json");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Failed to create settings file", ex.Message);
		}
	}

	public static void Populate()
	{
		try
		{
			string[] names = Enum.GetNames(typeof(Type));
			foreach (string text in names)
			{
				((Dictionary<string, object>)Cache).Add(text, (dynamic)Parse[text].Value<object>());
			}
			Log.Information("Successfully populated cache");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Failed to populate settings file cache", ex.Message);
		}
	}

	public static JToken Read(Type Type)
	{
		return Cache[Type.ToString()];
	}

	public static bool Edit(Type Key, JToken Value)
	{
		Cache[Key.ToString()] = Value;
		JObject jObject = JObject.FromObject(new { });
		foreach (KeyValuePair<string, object> item in Cache)
		{
			jObject.Add(item.Key, (dynamic)item.Value);
		}
		File.WriteAllText(Path + "\\Settings.json", jObject.ToString());
		Log.Information($"Successfully edited settings from {Key} to {Value}");
		return true;
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern bool SetDllDirectory(string lpPathName);

	private static void PopulateDLLS()
	{
		WebClient webClient = new WebClient();
		if (!File.Exists(Path + "\\DLLS\\oo2core_5_win64.dll"))
		{
			Log.Information("Downloading oo2core_5_win64.dll to " + Path + "\\DLLS\\oo2core_5_win64.dll");
			webClient.DownloadFile("https://github.com/CodeAmme/Fortnite-Uasset-Editor/raw/main/API/DLLS/oo2core_5_win64.dll", Path + "\\DLLS\\oo2core_5_win64.dll");
		}
		if (!File.Exists(Path + "\\DLLS\\oo2core_9_win64.dll"))
		{
			Log.Information("Downloading oo2core_9_win64.dll to " + Path + "\\DLLS\\oo2core_9_win64.dll");
			webClient.DownloadFile("https://github.com/CodeAmme/Fortnite-Uasset-Editor/raw/main/API/DLLS/oo2core_9_win64.dll", Path + "\\DLLS\\oo2core_9_win64.dll");
		}
		webClient.Dispose();
		SetDllDirectory(Path + "\\DLLS");
		Log.Information("Set kernal DLL location to " + Path + "\\DLLS");
	}

	private static bool Serilog()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Path + "\\LOGS");
		if (directoryInfo.GetFiles().Count() > 10)
		{
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				try
				{
					File.Delete(fileInfo.FullName);
				}
				catch
				{
				}
			}
		}
		DateTime now = DateTime.Now;
		string path = Path + "\\LOGS\\Uasset-Reader-" + now.ToString("yyyy.MM.dd.hh.mm") + ".log";
		Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File(path, LogEventLevel.Verbose, "[{Level:u3}] {Message:lj}{NewLine}{Exception}", null, null, null, buffered: false, shared: false, null, RollingInterval.Infinite, rollOnFileSizeLimit: false, null).WriteTo.Console(LogEventLevel.Verbose, "[{Level:u3}] {Message:lj}{NewLine}{Exception}").CreateLogger();
		Log.Information("Successfully created log file at " + now.ToString("yyyy.MM.dd.hh.mm"));
		return true;
	}
}
