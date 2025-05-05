using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class UMaterialExpressionStaticBoolParameter : UMaterialExpressionParameter
{
	public bool DefaultValue { get; private set; } = true;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		DefaultValue = GetOrDefault("DefaultValue", defaultValue: false);
	}
}
