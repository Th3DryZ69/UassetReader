using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

[StructFallback]
public class FCompositeSection : FAnimLinkableElement
{
	public FName SectionName;

	public FName NextSectionName;

	public UAnimMetaData[] MetaData;

	public FCompositeSection(FStructFallback fallback)
		: base(fallback)
	{
		SectionName = fallback.GetOrDefault<FName>("SectionName");
		NextSectionName = fallback.GetOrDefault<FName>("NextSectionName");
		MetaData = fallback.GetOrDefault<UAnimMetaData[]>("MetaData");
	}
}
