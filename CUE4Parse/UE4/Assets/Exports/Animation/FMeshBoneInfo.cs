using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

[JsonConverter(typeof(FMeshBoneInfoConverter))]
public struct FMeshBoneInfo
{
	public readonly FName Name;

	public readonly int ParentIndex;

	public FMeshBoneInfo(FArchive Ar)
	{
		Name = Ar.ReadFName();
		ParentIndex = Ar.Read<int>();
		if (Ar.Ver < EUnrealEngineObjectUE4Version.REFERENCE_SKELETON_REFACTOR)
		{
			Ar.Read<FColor>();
		}
	}

	public FMeshBoneInfo(FName name, int parentIndex)
	{
		Name = name;
		ParentIndex = parentIndex;
	}

	public override string ToString()
	{
		return $"{Name}";
	}
}
