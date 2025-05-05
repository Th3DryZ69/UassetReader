using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Core.i18N;

[JsonConverter(typeof(FTextConverter))]
public class FText : IUStruct
{
	public readonly uint Flags;

	public readonly ETextHistoryType HistoryType;

	public readonly FTextHistory TextHistory;

	public string Text => TextHistory.Text;

	public FText(FAssetArchive Ar)
	{
		Flags = Ar.Read<uint>();
		HistoryType = Ar.Read<ETextHistoryType>();
		TextHistory = HistoryType switch
		{
			ETextHistoryType.Base => new FTextHistory.Base(Ar), 
			ETextHistoryType.NamedFormat => new FTextHistory.NamedFormat(Ar), 
			ETextHistoryType.OrderedFormat => new FTextHistory.OrderedFormat(Ar), 
			ETextHistoryType.ArgumentFormat => new FTextHistory.ArgumentFormat(Ar), 
			ETextHistoryType.AsNumber => new FTextHistory.FormatNumber(Ar, HistoryType), 
			ETextHistoryType.AsPercent => new FTextHistory.FormatNumber(Ar, HistoryType), 
			ETextHistoryType.AsCurrency => new FTextHistory.FormatNumber(Ar, HistoryType), 
			ETextHistoryType.AsDate => new FTextHistory.AsDate(Ar), 
			ETextHistoryType.AsTime => new FTextHistory.AsTime(Ar), 
			ETextHistoryType.AsDateTime => new FTextHistory.AsDateTime(Ar), 
			ETextHistoryType.Transform => new FTextHistory.Transform(Ar), 
			ETextHistoryType.StringTableEntry => new FTextHistory.StringTableEntry(Ar), 
			ETextHistoryType.TextGenerator => new FTextHistory.TextGenerator(Ar), 
			_ => new FTextHistory.None(Ar), 
		};
	}

	public FText(string sourceString, string localizedString = "")
		: this("", "", sourceString, localizedString)
	{
	}

	public FText(string @namespace, string key, string sourceString, string localizedString = "")
		: this(0u, ETextHistoryType.Base, new FTextHistory.Base(@namespace, key, sourceString, localizedString))
	{
	}

	public FText(uint flags, ETextHistoryType historyType, FTextHistory textHistory)
	{
		Flags = flags;
		HistoryType = historyType;
		TextHistory = textHistory;
	}

	public override string ToString()
	{
		return Text;
	}
}
