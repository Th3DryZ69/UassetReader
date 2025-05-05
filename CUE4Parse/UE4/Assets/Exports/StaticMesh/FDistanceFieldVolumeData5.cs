using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FDistanceFieldVolumeData5
{
	public FBox LocalSpaceMeshBounds;

	public bool bMostlyTwoSided;

	public FSparseDistanceFieldMip[] Mips;

	public byte[] AlwaysLoadedMip;

	public FByteBulkData StreamableMips;

	public FDistanceFieldVolumeData5(FAssetArchive Ar)
	{
		LocalSpaceMeshBounds = new FBox(Ar);
		bMostlyTwoSided = Ar.ReadBoolean();
		Mips = Ar.ReadArray(3, () => new FSparseDistanceFieldMip(Ar));
		AlwaysLoadedMip = Ar.ReadArray<byte>();
		StreamableMips = new FByteBulkData(Ar);
	}
}
