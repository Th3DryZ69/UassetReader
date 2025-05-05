using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FDistanceFieldVolumeData
{
	public ushort[] DistanceFieldVolume;

	public FIntVector Size;

	public FBox LocalBoundingBox;

	public bool bMeshWasClosed;

	public bool bBuiltAsIfTwoSided;

	public bool bMeshWasPlane;

	public byte[] CompressedDistanceFieldVolume;

	public FVector2D DistanceMinMax;

	public FDistanceFieldVolumeData(FArchive Ar)
	{
		if (Ar.Game >= EGame.GAME_UE4_16)
		{
			CompressedDistanceFieldVolume = Ar.ReadArray<byte>();
			Size = Ar.Read<FIntVector>();
			LocalBoundingBox = Ar.Read<FBox>();
			DistanceMinMax = Ar.Read<FVector2D>();
			bMeshWasClosed = Ar.ReadBoolean();
			bBuiltAsIfTwoSided = Ar.ReadBoolean();
			bMeshWasPlane = Ar.ReadBoolean();
			DistanceFieldVolume = new ushort[0];
		}
		else
		{
			DistanceFieldVolume = Ar.ReadArray<ushort>();
			Size = Ar.Read<FIntVector>();
			LocalBoundingBox = Ar.Read<FBox>();
			bMeshWasClosed = Ar.ReadBoolean();
			bBuiltAsIfTwoSided = Ar.Ver >= EUnrealEngineObjectUE4Version.RENAME_CROUCHMOVESCHARACTERDOWN && Ar.ReadBoolean();
			bMeshWasPlane = Ar.Ver >= EUnrealEngineObjectUE4Version.DEPRECATE_UMG_STYLE_ASSETS && Ar.ReadBoolean();
			CompressedDistanceFieldVolume = new byte[0];
			DistanceMinMax = new FVector2D(0f, 0f);
		}
	}
}
