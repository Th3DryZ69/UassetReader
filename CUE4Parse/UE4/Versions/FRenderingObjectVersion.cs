using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FRenderingObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		IndirectLightingCache3BandSupport = 1,
		CustomReflectionCaptureResolutionSupport = 2,
		RemovedTextureStreamingLevelData = 3,
		IntroducedMeshDecals = 4,
		ReflectionCapturesStoreAverageBrightness = 5,
		ChangedPlanarReflectionFadeDefaults = 6,
		RemovedRenderTargetSize = 7,
		MovedParticleCutoutsToRequiredModule = 8,
		MapBuildDataSeparatePackage = 9,
		TextureStreamingMeshUVChannelData = 10,
		TypeHandlingForMaterialSqrtNodes = 11,
		FixedBSPLightmaps = 12,
		DistanceFieldSelfShadowBias = 13,
		FixedLegacyMaterialAttributeNodeTypes = 14,
		ShaderResourceCodeSharing = 15,
		MotionBlurAndTAASupportInSceneCapture2d = 16,
		AddedTextureRenderTargetFormats = 17,
		FixedMeshUVDensity = 18,
		AddedbUseShowOnlyList = 19,
		VolumetricLightmaps = 20,
		MaterialAttributeLayerParameters = 21,
		StoreReflectionCaptureBrightnessForCooking = 22,
		ModelVertexBufferSerialization = 23,
		ReplaceLightAsIfStatic = 24,
		ShaderPermutationId = 25,
		IncreaseNormalPrecision = 26,
		VirtualTexturedLightmaps = 27,
		GeometryCacheFastDecoder = 28,
		LightmapHasShadowmapData = 29,
		DiaphragmDOFOnlyForDeferredShadingRenderer = 30,
		VirtualTexturedLightmapsV2 = 31,
		SkyAtmosphereStaticLightingVersioning = 32,
		ExplicitSRGBSetting = 33,
		VolumetricLightmapStreaming = 34,
		RemovedSM4 = 35,
		MaterialShaderMapIdSerialization = 36,
		StaticMeshSectionForceOpaqueField = 37,
		AutoExposureChanges = 38,
		RemovedEmulatedInstancing = 39,
		PerInstanceCustomData = 40,
		AnisotropicMaterial = 41,
		AutoExposureForceOverrideBiasFlag = 42,
		AutoExposureDefaultFix = 43,
		VolumeExtinctionBecomesRGB = 44,
		VirtualTexturedLightmapsV3 = 45,
		VersionPlusOne = 46,
		LatestVersion = 45
	}

	public static readonly FGuid GUID = new FGuid(318278559u, 2289388284u, 2793199884u, 943373609u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game < EGame.GAME_UE4_20)
		{
			if (game < EGame.GAME_UE4_16)
			{
				if (game < EGame.GAME_UE4_13)
				{
					if (game < EGame.GAME_UE4_12)
					{
						return Type.BeforeCustomVersionWasAdded;
					}
					return Type.CustomReflectionCaptureResolutionSupport;
				}
				if (game >= EGame.GAME_UE4_14)
				{
					if (game == EGame.GAME_TEKKEN7)
					{
						return Type.MapBuildDataSeparatePackage;
					}
					return Type.FixedBSPLightmaps;
				}
				return Type.IntroducedMeshDecals;
			}
			if (game < EGame.GAME_UE4_18)
			{
				if (game < EGame.GAME_UE4_17)
				{
					return Type.ShaderResourceCodeSharing;
				}
				return Type.AddedbUseShowOnlyList;
			}
			if (game < EGame.GAME_UE4_19)
			{
				return Type.VolumetricLightmaps;
			}
			return Type.ShaderPermutationId;
		}
		if (game < EGame.GAME_UE4_24)
		{
			if (game < EGame.GAME_UE4_22)
			{
				if (game < EGame.GAME_UE4_21)
				{
					return Type.IncreaseNormalPrecision;
				}
				return Type.VirtualTexturedLightmaps;
			}
			if (game < EGame.GAME_UE4_23)
			{
				return Type.GeometryCacheFastDecoder;
			}
			return Type.VirtualTexturedLightmapsV2;
		}
		if (game < EGame.GAME_UE4_26)
		{
			if (game < EGame.GAME_UE4_25)
			{
				return Type.MaterialShaderMapIdSerialization;
			}
			return Type.AutoExposureDefaultFix;
		}
		if (game < EGame.GAME_UE4_27)
		{
			return Type.VolumeExtinctionBecomesRGB;
		}
		return Type.VirtualTexturedLightmapsV3;
	}
}
