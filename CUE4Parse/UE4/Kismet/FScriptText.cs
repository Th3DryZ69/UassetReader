using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

[JsonConverter(typeof(FScriptTextConverter))]
public class FScriptText
{
	public EBlueprintTextLiteralType TextLiteralType;

	public KismetExpression? SourceString;

	public KismetExpression? KeyString;

	public KismetExpression? Namespace;

	public FPackageIndex? StringTableAsset;

	public KismetExpression? TableIdString;

	public FScriptText(FKismetArchive Ar)
	{
		TextLiteralType = (EBlueprintTextLiteralType)Ar.Read<byte>();
		switch (TextLiteralType)
		{
		case EBlueprintTextLiteralType.LocalizedText:
			SourceString = Ar.ReadExpression();
			KeyString = Ar.ReadExpression();
			Namespace = Ar.ReadExpression();
			break;
		case EBlueprintTextLiteralType.InvariantText:
			SourceString = Ar.ReadExpression();
			break;
		case EBlueprintTextLiteralType.LiteralString:
			SourceString = Ar.ReadExpression();
			break;
		case EBlueprintTextLiteralType.StringTableEntry:
			StringTableAsset = new FPackageIndex(Ar);
			TableIdString = Ar.ReadExpression();
			KeyString = Ar.ReadExpression();
			break;
		case EBlueprintTextLiteralType.Empty:
			break;
		}
	}
}
