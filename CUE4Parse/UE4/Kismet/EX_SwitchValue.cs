using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_SwitchValue : KismetExpression
{
	public uint EndGotoOffset;

	public KismetExpression IndexTerm;

	public KismetExpression DefaultTerm;

	public FKismetSwitchCase[] Cases;

	public override EExprToken Token => EExprToken.EX_SwitchValue;

	public EX_SwitchValue(FKismetArchive Ar)
	{
		ushort length = Ar.Read<ushort>();
		EndGotoOffset = Ar.Read<uint>();
		IndexTerm = Ar.ReadExpression();
		Cases = Ar.ReadArray(length, () => new FKismetSwitchCase(Ar));
		DefaultTerm = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("IndexTerm");
		serializer.Serialize(writer, IndexTerm);
		writer.WritePropertyName("EndGotoOffset");
		writer.WriteValue(EndGotoOffset);
		writer.WritePropertyName("Cases");
		serializer.Serialize(writer, Cases);
		writer.WritePropertyName("DefaultTerm");
		serializer.Serialize(writer, DefaultTerm);
	}
}
