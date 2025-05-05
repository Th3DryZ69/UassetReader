using System.Diagnostics;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Plugins;

[DebuggerDisplay("{CanContainContent}")]
public class UPluginDescriptor
{
	[JsonProperty]
	public bool CanContainContent { get; set; }
}
