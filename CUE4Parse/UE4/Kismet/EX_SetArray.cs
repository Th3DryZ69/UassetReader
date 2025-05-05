using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_SetArray : KismetExpression
{
	public KismetExpression? AssigningProperty;

	public FPackageIndex? ArrayInnerProp;

	public KismetExpression[] Elements;

	public override EExprToken Token => EExprToken.EX_SetArray;

	public EX_SetArray(FKismetArchive Ar)
	{
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.CHANGE_SETARRAY_BYTECODE)
		{
			AssigningProperty = Ar.ReadExpression();
		}
		else
		{
			ArrayInnerProp = new FPackageIndex(Ar);
		}
		Elements = Ar.ReadExpressionArray(EExprToken.EX_EndArray);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		if (AssigningProperty != null)
		{
			writer.WritePropertyName("AssigningProperty");
			serializer.Serialize(writer, AssigningProperty);
		}
		else
		{
			writer.WritePropertyName("ArrayInnerProp");
			serializer.Serialize(writer, ArrayInnerProp);
		}
		writer.WritePropertyName("Elements");
		serializer.Serialize(writer, Elements);
	}
}
