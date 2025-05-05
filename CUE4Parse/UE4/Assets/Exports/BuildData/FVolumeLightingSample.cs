using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class FVolumeLightingSample
{
	public FVector Position;

	public float Radius;

	public float[][] Lighting;

	public FColor PackedSkyBentNormal;

	public float DirectionalLightShadowing;

	public FVolumeLightingSample(FAssetArchive Ar)
	{
		Position = Ar.Read<FVector>();
		Radius = Ar.Read<float>();
		Lighting = Ar.ReadArray(3, () => Ar.ReadArray<float>(9));
		PackedSkyBentNormal = Ar.Read<FColor>();
		DirectionalLightShadowing = Ar.Read<float>();
	}
}
