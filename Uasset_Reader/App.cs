using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using Galaxy_Swapper_v2.Workspace.CUE4Parse;
using Newtonsoft.Json.Linq;
using Serilog;
using Uasset_Reader.Workspace.Other;
using Uasset_Reader.Workspace.Utilities;
using Uasset_Reader.Workspace.Views;

namespace Uasset_Reader;

public partial class App : Application
{
	private void Application_Startup(object sender, StartupEventArgs e)
	{
		Settings.Initialize();
		Presence.Initialize();
		Log.Information("Downloading AES keys");
		AES.Download();
		Log.Information("Initializing CUE4Parse");
		CProvider.Initialize($"{Settings.Read(Settings.Type.Installtion)}\\FortniteGame\\Content\\Paks");
        // Log.Information("Attempting to verify permissions.. Make sure Discord is open during this process.");
        // while (Presence.ID == 0L)
        // {
        // }
        // JToken jToken = Endpoint.Download(Endpoint.Type.PermittedUsers);
        // Log.Information("Username: " + Presence.Username);
        // Log.Information($"ID: {Presence.ID}");
        // Log.Information("Checking if you are allowed to use this.");
        // if (jToken["CheckRequired"].Value<bool>())
        // {
        // bool flag = false;
        // foreach (JToken item in (IEnumerable<JToken>)jToken["Users"])
        // {
        // if ((string?)item == Presence.ID.ToString())
        // {
        // flag = true;
        // break;
        // }
        // }
        // if (!flag)
        // {
        // Log.Fatal("Contact agentamme on discord if you get this error\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0");
        // Thread.Sleep(2500);
        // Environment.Exit(0);
        // }
        // }
        // Log.Information("Verification successful! Welcome " + Presence.Username + ".");
        if (!Settings.Read(Settings.Type.Console).Value<bool>())
		{
			Log.Information("Console is disabled! Hiding..");
            Uasset_Reader.Workspace.Other.Console.Hide();
		}
		Log.Information("Displaying MainView");
		new MainView().ShowDialog();
		Application.Current.Shutdown();
	}
}
