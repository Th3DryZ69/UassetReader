using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class FPrecomputedVolumetricLightmapData
{
	public FBox Bounds;

	public FIntVector IndirectionTextureDimensions;

	public FVolumetricLightmapDataLayer IndirectionTexture;

	public int BrickSize;

	public FIntVector BrickDataDimensions;

	public FVolumetricLightmapBrickLayer BrickData;

	public FIntVector[]? SubLevelBrickPositions;

	public FColor[]? IndirectionTextureOriginalValues;

	public FPrecomputedVolumetricLightmapData(FArchive Ar)
	{
		if (Ar.ReadBoolean())
		{
			if (Ar.Game == EGame.GAME_StarWarsJediSurvivor)
			{
				Ar.Position += 8L;
			}
			Bounds = new FBox(Ar);
			IndirectionTextureDimensions = Ar.Read<FIntVector>();
			IndirectionTexture = new FVolumetricLightmapDataLayer(Ar);
			BrickSize = Ar.Read<int>();
			BrickDataDimensions = Ar.Read<FIntVector>();
			BrickData = new FVolumetricLightmapBrickLayer
			{
				AmbientVector = new FVolumetricLightmapDataLayer(Ar),
				SHCoefficients = new FVolumetricLightmapDataLayer[6]
			};
			for (int i = 0; i < BrickData.SHCoefficients.Length; i++)
			{
				BrickData.SHCoefficients[i] = new FVolumetricLightmapDataLayer(Ar);
			}
			BrickData.SkyBentNormal = new FVolumetricLightmapDataLayer(Ar);
			BrickData.DirectionalLightShadowing = new FVolumetricLightmapDataLayer(Ar);
			if (FMobileObjectVersion.Get(Ar) >= FMobileObjectVersion.Type.LQVolumetricLightmapLayers)
			{
				BrickData.LQLightColor = new FVolumetricLightmapDataLayer(Ar);
				BrickData.LQLightDirection = new FVolumetricLightmapDataLayer(Ar);
			}
			if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.VolumetricLightmapStreaming)
			{
				SubLevelBrickPositions = Ar.ReadArray<FIntVector>();
				IndirectionTextureOriginalValues = Ar.ReadArray<FColor>();
			}
		}
	}
}
