using System.Diagnostics;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Plugins;

[DebuggerDisplay("{File}")]
public class UPluginContents
{
	[JsonProperty]
	public string File { get; set; }

	[JsonProperty]
	public UPluginDescriptor Descriptor { get; set; }
}
