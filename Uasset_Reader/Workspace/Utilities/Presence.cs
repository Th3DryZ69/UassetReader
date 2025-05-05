using DiscordRPC;
using DiscordRPC.Message;
using Newtonsoft.Json.Linq;

namespace Uasset_Reader.Workspace.Utilities;

public static class Presence
{
	private static RichPresence RichPresence;

	public static DiscordRpcClient Client;

	public static ulong ID;

	public static string Username { get; set; }

	public static void Initialize()
	{
		Client = new DiscordRpcClient("1212467147385872384");
		Client.Initialize();
		Client.OnReady += delegate(object sender, ReadyMessage e)
		{
			ID = e.User.ID;
			Username = e.User.Username;
		};
		RichPresence richPresence = new RichPresence();
		richPresence.Details = "Dashboard";
		richPresence.State = "v2.02 (Cracked)";
		richPresence.Timestamps = Timestamps.Now;
		richPresence.Buttons = new Button[1]
		{
			new Button
			{
				Label = "GitHub",
				Url = "https://github.com/CodeAmme/Fortnite-Uasset-Editor"
			}
		};
		richPresence.Assets = new Assets
		{
			LargeImageKey = "logo",
			LargeImageText = "Love Modding\0",
			SmallImageKey = "pfp",
			SmallImageText = "Cracked by agentamme"
		};
		RichPresence = richPresence;
		if (Settings.Read(Settings.Type.Presence).Value<bool>())
		{
			Client.SetPresence(RichPresence);
		}
	}

	public static void Update(string Details)
	{
		if (Settings.Read(Settings.Type.Presence).Value<bool>())
		{
			Client.UpdateDetails(Details);
		}
	}
}
