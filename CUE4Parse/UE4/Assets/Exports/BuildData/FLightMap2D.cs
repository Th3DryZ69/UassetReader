using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

[JsonConverter(typeof(FLightMap2DConverter))]
public class FLightMap2D : FLightMap
{
	private const int NUM_STORED_LIGHTMAP_COEF = 4;

	public readonly FPackageIndex[]? Textures;

	public readonly FPackageIndex? SkyOcclusionTexture;

	public readonly FPackageIndex? AOMaterialMaskTexture;

	public readonly FPackageIndex? ShadowMapTexture;

	public readonly FPackageIndex[]? VirtualTextures;

	public readonly FVector4[]? ScaleVectors;

	public readonly FVector4[]? AddVectors;

	public readonly FVector2D? CoordinateScale;

	public readonly FVector2D? CoordinateBias;

	public readonly FVector4? InvUniformPenumbraSize;

	public readonly bool[]? bShadowChannelValid;

	public FLightMap2D(FAssetArchive Ar)
		: base(Ar)
	{
		Textures = new FPackageIndex[2];
		VirtualTextures = new FPackageIndex[2];
		ScaleVectors = new FVector4[4];
		AddVectors = new FVector4[4];
		if (Ar.Ver <= EUnrealEngineObjectUE4Version.LOW_QUALITY_DIRECTIONAL_LIGHTMAPS)
		{
			for (int i = 0; i < 3; i++)
			{
				Ar.Position += 36L;
			}
		}
		else if (Ar.Ver <= EUnrealEngineObjectUE4Version.COMBINED_LIGHTMAP_TEXTURES)
		{
			for (int j = 0; j < 4; j++)
			{
				Ar.Position += 36L;
			}
		}
		else
		{
			Textures[0] = new FPackageIndex(Ar);
			Textures[1] = new FPackageIndex(Ar);
			if (Ar.Ver >= EUnrealEngineObjectUE4Version.SKY_LIGHT_COMPONENT)
			{
				SkyOcclusionTexture = new FPackageIndex(Ar);
				if (Ar.Ver >= EUnrealEngineObjectUE4Version.AO_MATERIAL_MASK)
				{
					AOMaterialMaskTexture = new FPackageIndex(Ar);
				}
			}
			for (int k = 0; k < 4; k++)
			{
				ScaleVectors[k] = Ar.Read<FVector4>();
				AddVectors[k] = Ar.Read<FVector4>();
			}
		}
		CoordinateScale = new FVector2D(Ar);
		CoordinateBias = new FVector2D(Ar);
		if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.LightmapHasShadowmapData)
		{
			bShadowChannelValid = Ar.ReadArray(4, () => Ar.ReadBoolean());
			InvUniformPenumbraSize = Ar.Read<FVector4>();
		}
		if (FRenderingObjectVersion.Get(Ar) < FRenderingObjectVersion.Type.VirtualTexturedLightmaps)
		{
			return;
		}
		if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.VirtualTexturedLightmapsV2)
		{
			if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.VirtualTexturedLightmapsV3)
			{
				VirtualTextures[0] = new FPackageIndex(Ar);
				VirtualTextures[1] = new FPackageIndex(Ar);
			}
			else
			{
				VirtualTextures[0] = new FPackageIndex(Ar);
			}
		}
		else
		{
			VirtualTextures[0] = new FPackageIndex(Ar);
		}
	}
}
