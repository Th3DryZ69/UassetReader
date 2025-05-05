using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class FShadowMap2D : FShadowMap
{
	public readonly FPackageIndex Texture;

	public readonly FVector2D CoordinateScale;

	public readonly FVector2D CoordinateBias;

	public readonly bool[] bChannelValid;

	public readonly FVector4 InvUniformPenumbraSize;

	public FShadowMap2D(FAssetArchive Ar)
		: base(Ar)
	{
		Texture = new FPackageIndex(Ar);
		CoordinateScale = new FVector2D(Ar);
		CoordinateBias = new FVector2D(Ar);
		bChannelValid = Ar.ReadArray(4, () => Ar.ReadBoolean());
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.STATIC_SHADOWMAP_PENUMBRA_SIZE)
		{
			InvUniformPenumbraSize = Ar.Read<FVector4>();
		}
		else
		{
			InvUniformPenumbraSize = new FVector4(20f, 20f, 20f, 20f);
		}
	}
}
