using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine.Animation;

public readonly struct FSmartName : IUStruct
{
	public readonly FName DisplayName;

	public FSmartName(FArchive Ar)
	{
		DisplayName = Ar.ReadFName();
		if (FAnimPhysObjectVersion.Get(Ar) < FAnimPhysObjectVersion.Type.RemoveUIDFromSmartNameSerialize)
		{
			Ar.Read<ushort>();
		}
		if (FAnimPhysObjectVersion.Get(Ar) < FAnimPhysObjectVersion.Type.SmartNameRefactorForDeterministicCooking)
		{
			Ar.Read<FGuid>();
		}
	}

	public FSmartName(FStructFallback data)
	{
		DisplayName = data.GetOrDefault<FName>("DisplayName");
	}
}
