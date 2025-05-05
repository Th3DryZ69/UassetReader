using System.Diagnostics;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Plugins;

[DebuggerDisplay("{Amount}")]
public class UPluginManifest
{
	[JsonProperty]
	public UPluginContents[] Contents { get; set; }

	public int Amount => Contents.Length;
}
