using System;
using System.Collections.Generic;
using System.Net;
using CUE4Parse.Encryption.Aes;
using CUE4Parse.UE4.Objects.Core.Misc;
using Newtonsoft.Json.Linq;
using Serilog;
using Uasset_Reader.Workspace.Utilities;

namespace Uasset_Reader.Workspace.Other;

public static class AES
{
	public static Dictionary<FGuid, FAesKey> Keys = new Dictionary<FGuid, FAesKey>();

	private const string Domain = "https://fortnite-api.com/v2/aes";

	private const string Dummy = "0x0000000000000000000000000000000000000000000000000000000000000000";

	public static void Download()
	{
		if (Keys.Count != 0)
		{
			Log.Information("Aes keys have already been downloaded! Skipping.");
			return;
		}
		Log.Information("Attempting to download aes keys");
		using WebClient webClient = new WebClient();
		try
		{
			string text = webClient.DownloadString("https://fortnite-api.com/v2/aes");
			if (!text.ValidJson() || string.IsNullOrEmpty(text))
			{
				Log.Warning("https://fortnite-api.com/v2/aes has returned with empty response! Loading dummy key.");
				Keys.Add(default(FGuid), new FAesKey("0x0000000000000000000000000000000000000000000000000000000000000000"));
				return;
			}
			JObject jObject = JObject.Parse(text);
			if (jObject["data"]["mainKey"] == null || string.IsNullOrEmpty(jObject["data"]["mainKey"].Value<string>()))
			{
				Log.Warning("Main key is null or empty! Loading dummy key.");
				Keys.Add(default(FGuid), new FAesKey("0x0000000000000000000000000000000000000000000000000000000000000000"));
			}
			else
			{
				Log.Information("Main key loading as " + jObject["data"]["mainKey"].Value<string>());
				Keys.Add(default(FGuid), new FAesKey(FormatAES(jObject["data"]["mainKey"].Value<string>())));
			}
			foreach (JToken item in (IEnumerable<JToken>)jObject["data"]["dynamicKeys"])
			{
				FGuid key = new FGuid(item["pakGuid"].Value<string>());
				FAesKey value = new FAesKey(FormatAES(item["key"].Value<string>()));
				if (!Keys.ContainsKey(key))
				{
					Keys.Add(key, value);
					Log.Information("Added FGuid with pakGuid: " + item["pakGuid"].Value<string>() + " with Aes key: " + item["key"].Value<string>());
				}
			}
		}
		catch (Exception ex)
		{
			Log.Warning("Failed to download response from https://fortnite-api.com/v2/aes loading with dummy key! " + ex.Message);
			Keys.Add(default(FGuid), new FAesKey("0x0000000000000000000000000000000000000000000000000000000000000000"));
		}
	}

	private static string FormatAES(string Key)
	{
		if (Key.StartsWith("0x"))
		{
			return Key;
		}
		return "0x" + Key;
	}
}
