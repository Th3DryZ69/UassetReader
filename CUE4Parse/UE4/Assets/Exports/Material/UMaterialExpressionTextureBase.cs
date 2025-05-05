using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class UMaterialExpressionTextureBase : UMaterialExpression
{
	public UTexture Texture { get; private set; }

	public EMaterialSamplerType SamplerType { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		SamplerType = GetOrDefault("SamplerType", EMaterialSamplerType.SAMPLERTYPE_Color);
		if (TryGetValue<FPackageIndex>(out var obj, "Texture") && obj.TryLoad(out UTexture export))
		{
			Texture = export;
		}
	}
}
