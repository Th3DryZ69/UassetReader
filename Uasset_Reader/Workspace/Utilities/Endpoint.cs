using System;
using System.Net;
using System.Windows;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Uasset_Reader.Workspace.Utilities;

public static class Endpoint
{
	public enum Type
	{
		Versions,
		PermittedUsers
	}

	private const string GitHub = "https://raw.githubusercontent.com/CodeAmme/Fortnite-Uasset-Editor/main/API/";

	public static JToken Download(Type Type)
	{
		string text = $"{"https://raw.githubusercontent.com/CodeAmme/Fortnite-Uasset-Editor/main/API/"}{"2.02"}/{Type}.json";
		if (Type == Type.Versions)
		{
			text = $"{"https://raw.githubusercontent.com/CodeAmme/Fortnite-Uasset-Editor/main/API/"}{Type}.json";
		}
		Log.Information("Downloading response from " + text);
		using WebClient webClient = new WebClient();
		string text2 = webClient.DownloadString(text);
		if (text2.IsEncrypted())
		{
			Log.Information(text + " responded in a encrypted format! Decrypting..");
			text2 = text2.Decompress();
		}
		if (!text2.ValidJson())
		{
			Log.Fatal(text + " responded in a invalid JSON format!");
			MessageBox.Show("Endpoint did not respond in a valid JSON format! Please contact Wslt about this issue.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
			Environment.Exit(0);
		}
		return JObject.Parse(text2);
	}
}
