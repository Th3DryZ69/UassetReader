using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Animation.ACL;

public class UAnimBoneCompressionCodec_ACL : UAnimBoneCompressionCodec_ACLBase
{
	public FPackageIndex SafetyFallbackCodec;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		SafetyFallbackCodec = GetOrDefault<FPackageIndex>("SafetyFallbackCodec");
	}

	public override UAnimBoneCompressionCodec? GetCodec(string ddcHandle)
	{
		UAnimBoneCompressionCodec uAnimBoneCompressionCodec = ((GetCodecDDCHandle() == ddcHandle) ? this : null);
		if (uAnimBoneCompressionCodec == null && SafetyFallbackCodec.TryLoad(out UAnimBoneCompressionCodec export))
		{
			uAnimBoneCompressionCodec = export.GetCodec(ddcHandle);
		}
		return uAnimBoneCompressionCodec;
	}
}
