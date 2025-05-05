using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class UAnimCurveCompressionSettings : UObject
{
	public FPackageIndex? Codec;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		Codec = GetOrDefault<FPackageIndex>("Codec");
	}

	public UAnimCurveCompressionCodec? GetCodec(string path)
	{
		return Codec?.Load<UAnimCurveCompressionCodec>()?.GetCodec(path);
	}
}
