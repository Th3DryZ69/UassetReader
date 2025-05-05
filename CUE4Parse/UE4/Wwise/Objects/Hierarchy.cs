using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Wwise.Enums;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Wwise.Objects;

[JsonConverter(typeof(HierarchyConverter))]
public readonly struct Hierarchy
{
	public readonly EHierarchyObjectType Type;

	public readonly int Length;

	public readonly AbstractHierarchy? Data;

	public Hierarchy(FArchive Ar)
	{
		Type = Ar.Read<EHierarchyObjectType>();
		Length = Ar.Read<int>();
		Ar.Position += Length;
		AbstractHierarchy data = null;
		Data = data;
	}
}
