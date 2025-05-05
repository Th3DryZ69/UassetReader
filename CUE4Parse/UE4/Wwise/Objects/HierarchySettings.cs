using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Wwise.Enums;

namespace CUE4Parse.UE4.Wwise.Objects;

public class HierarchySettings : AbstractHierarchy
{
	public readonly EHierarchyParameterType[] Types;

	public readonly float[] Values;

	public HierarchySettings(FArchive Ar)
		: base(Ar)
	{
		int length = Ar.ReadByte();
		Types = Ar.ReadArray<EHierarchyParameterType>(length);
		Values = Ar.ReadArray<float>(length);
	}
}
