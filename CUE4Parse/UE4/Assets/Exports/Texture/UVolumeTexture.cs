using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public class UVolumeTexture : UTexture
{
	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		new FStripDataFlags(Ar);
		if (Ar.ReadBoolean())
		{
			DeserializeCookedPlatformData(Ar);
		}
	}
}
