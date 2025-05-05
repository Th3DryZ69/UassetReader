using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.i18N;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(TextPropertyConverter))]
public class TextProperty : FPropertyTagType<FText>
{
	public TextProperty(FAssetArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? new FText(Ar) : new FText(0u, ETextHistoryType.None, new FTextHistory.None()));
	}
}
