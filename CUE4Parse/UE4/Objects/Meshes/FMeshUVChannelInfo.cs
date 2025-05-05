using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Meshes;

[StructFallback]
public class FMeshUVChannelInfo
{
	public bool bInitialized;

	public bool bOverrideDensities;

	public float[] LocalUVDensities;

	private const int TEXSTREAM_MAX_NUM_UVCHANNELS = 4;

	public FMeshUVChannelInfo(FArchive Ar)
	{
		bInitialized = Ar.ReadBoolean();
		bOverrideDensities = Ar.ReadBoolean();
		LocalUVDensities = Ar.ReadArray<float>(4);
	}

	public FMeshUVChannelInfo(FStructFallback fallback)
	{
		bInitialized = fallback.GetOrDefault("bInitialized", defaultValue: false);
		bOverrideDensities = fallback.GetOrDefault("bOverrideDensities", defaultValue: false);
		if (fallback.TryGetAllValues<float>(out float[] obj, "LocalUVDensities"))
		{
			LocalUVDensities = obj;
		}
	}
}
