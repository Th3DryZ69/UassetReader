using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class FPrecomputedLightVolumeData
{
	public FBox Bounds;

	public float SampleSpacing;

	public int NumSHSamples;

	public FVolumeLightingSample[] HighQualitySamples;

	public FVolumeLightingSample[]? LowQualitySamples;

	public FPrecomputedLightVolumeData(FAssetArchive Ar)
	{
		if (!Ar.ReadBoolean() || !Ar.ReadBoolean())
		{
			return;
		}
		Bounds = new FBox(Ar);
		SampleSpacing = Ar.Read<float>();
		NumSHSamples = 4;
		if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.IndirectLightingCache3BandSupport)
		{
			NumSHSamples = Ar.Read<int>();
		}
		HighQualitySamples = Ar.ReadArray(() => new FVolumeLightingSample(Ar));
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.VOLUME_SAMPLE_LOW_QUALITY_SUPPORT)
		{
			LowQualitySamples = Ar.ReadArray(() => new FVolumeLightingSample(Ar));
		}
	}
}
