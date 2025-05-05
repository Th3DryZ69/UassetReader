using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class FVolumetricLightmapDataLayer
{
	public byte[] Data;

	public string PixelFormatString;

	public FVolumetricLightmapDataLayer(FArchive Ar)
	{
		Data = Ar.ReadArray<byte>();
		PixelFormatString = Ar.ReadFString();
	}
}
