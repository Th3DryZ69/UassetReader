using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class UAnimBoneCompressionSettings : UObject
{
	public FPackageIndex[] Codecs;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		Codecs = Get<FPackageIndex[]>("Codecs");
	}

	public UAnimBoneCompressionCodec? GetCodec(string ddcHandle)
	{
		UAnimBoneCompressionCodec uAnimBoneCompressionCodec = null;
		FPackageIndex[] codecs = Codecs;
		for (int i = 0; i < codecs.Length; i++)
		{
			UAnimBoneCompressionCodec uAnimBoneCompressionCodec2 = codecs[i].Load<UAnimBoneCompressionCodec>();
			if (uAnimBoneCompressionCodec2 != null)
			{
				uAnimBoneCompressionCodec = uAnimBoneCompressionCodec2.GetCodec(ddcHandle);
				if (uAnimBoneCompressionCodec != null)
				{
					break;
				}
			}
		}
		return uAnimBoneCompressionCodec;
	}
}
