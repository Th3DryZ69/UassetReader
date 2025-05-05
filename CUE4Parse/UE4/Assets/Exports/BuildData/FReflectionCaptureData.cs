using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

[JsonConverter(typeof(FReflectionCaptureDataConverter))]
public class FReflectionCaptureData
{
	public int CubemapSize;

	public float AverageBrightness;

	public float Brightness;

	public byte[]? FullHDRCapturedData;

	public FPackageIndex? EncodedCaptureData;

	public FReflectionCaptureData(FAssetArchive Ar)
	{
		CubemapSize = Ar.Read<int>();
		AverageBrightness = Ar.Read<float>();
		if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.StoreReflectionCaptureBrightnessForCooking && FUE5ReleaseStreamObjectVersion.Get(Ar) < FUE5ReleaseStreamObjectVersion.Type.ExcludeBrightnessFromEncodedHDRCubemap)
		{
			Brightness = Ar.Read<float>();
		}
		FullHDRCapturedData = Ar.ReadArray<byte>();
		if (FMobileObjectVersion.Get(Ar) >= FMobileObjectVersion.Type.StoreReflectionCaptureCompressedMobile && FUE5ReleaseStreamObjectVersion.Get(Ar) < FUE5ReleaseStreamObjectVersion.Type.StoreReflectionCaptureEncodedHDRDataInRG11B10Format)
		{
			EncodedCaptureData = new FPackageIndex(Ar);
		}
		else
		{
			Ar.ReadArray<byte>();
		}
	}
}
