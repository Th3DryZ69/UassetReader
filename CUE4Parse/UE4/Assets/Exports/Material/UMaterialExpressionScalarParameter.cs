using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class UMaterialExpressionScalarParameter : UMaterialExpressionParameter
{
	public float DefaultValue { get; private set; }

	public float SliderMin { get; private set; }

	public float SliderMax { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		DefaultValue = GetOrDefault("DefaultValue", 0f);
		SliderMin = GetOrDefault("SliderMin", 0f);
		SliderMax = GetOrDefault("SliderMax", 0f);
	}
}
