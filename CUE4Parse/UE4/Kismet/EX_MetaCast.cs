using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_MetaCast : EX_CastBase
{
	public override EExprToken Token => EExprToken.EX_MetaCast;

	public EX_MetaCast(FKismetArchive Ar)
		: base(Ar)
	{
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("Class");
		serializer.Serialize(writer, ClassPtr);
		writer.WritePropertyName("Target");
		serializer.Serialize(writer, Target);
	}
}
