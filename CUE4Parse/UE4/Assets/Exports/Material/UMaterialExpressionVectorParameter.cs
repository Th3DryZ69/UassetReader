using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class UMaterialExpressionVectorParameter : UMaterialExpressionParameter
{
	public FLinearColor DefaultValue { get; private set; }

	public FParameterChannelNames ChannelNames { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		DefaultValue = GetOrDefault<FLinearColor>("DefaultValue");
		ChannelNames = GetOrDefault<FParameterChannelNames>("ChannelNames");
	}
}
