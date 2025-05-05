using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;

namespace CUE4Parse.UE4.Assets.Exports.Material;

[StructFallback]
public class FMaterialInstanceBasePropertyOverrides
{
	public readonly EBlendMode BlendMode;

	public readonly EMaterialShadingModel ShadingModel;

	public readonly float OpacityMaskClipValue;

	public readonly bool DitheredLODTransition;

	public FMaterialInstanceBasePropertyOverrides(FStructFallback fallback)
	{
		BlendMode = fallback.GetOrDefault("BlendMode", EBlendMode.BLEND_Opaque);
		ShadingModel = fallback.GetOrDefault("ShadingModel", EMaterialShadingModel.MSM_Unlit);
		OpacityMaskClipValue = fallback.GetOrDefault("OpacityMaskClipValue", 0f);
		DitheredLODTransition = fallback.GetOrDefault("DitheredLODTransition", defaultValue: false);
	}
}
