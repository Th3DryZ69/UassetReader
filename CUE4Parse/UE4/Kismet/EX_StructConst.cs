using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_StructConst : KismetExpression
{
	public FPackageIndex Struct;

	public int StructSize;

	public KismetExpression[] Properties;

	public override EExprToken Token => EExprToken.EX_StructConst;

	public EX_StructConst(FKismetArchive Ar)
	{
		Struct = new FPackageIndex(Ar);
		StructSize = Ar.Read<int>();
		Properties = Ar.ReadExpressionArray(EExprToken.EX_EndStructConst);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("Struct");
		serializer.Serialize(writer, Struct);
		writer.WritePropertyName("Properties");
		serializer.Serialize(writer, Properties);
	}
}
