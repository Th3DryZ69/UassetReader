using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class UMaterialExpressionTextureSample : UMaterialExpressionTextureBase
{
	public FExpressionInput Coordinates { get; private set; }

	public FExpressionInput TextureObject { get; private set; }

	public FExpressionInput MipValue { get; private set; }

	public FExpressionInput CoordinatesDX { get; private set; }

	public FExpressionInput CoordinatesDY { get; private set; }

	public FExpressionInput AutomaticViewMipBiasValue { get; private set; }

	public ETextureMipValueMode MipValueMode { get; private set; }

	public ESamplerSourceMode SamplerSource { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		Coordinates = GetOrDefault<FExpressionInput>("Coordinates");
		TextureObject = GetOrDefault<FExpressionInput>("TextureObject");
		MipValue = GetOrDefault<FExpressionInput>("MipValue");
		CoordinatesDX = GetOrDefault<FExpressionInput>("CoordinatesDX");
		CoordinatesDY = GetOrDefault<FExpressionInput>("CoordinatesDY");
		AutomaticViewMipBiasValue = GetOrDefault<FExpressionInput>("AutomaticViewMipBiasValue");
		MipValueMode = GetOrDefault("MipValueMode", ETextureMipValueMode.TMVM_None);
		SamplerSource = GetOrDefault("SamplerSource", ESamplerSourceMode.SSM_FromTextureAsset);
	}
}
