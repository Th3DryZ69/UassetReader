using System.Text;
using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_Jump : KismetExpression
{
	public uint CodeOffset;

	public StringBuilder ObjectPath = new StringBuilder();

	public override EExprToken Token => EExprToken.EX_Jump;

	public EX_Jump(FKismetArchive Ar)
	{
		CodeOffset = Ar.Read<uint>();
		ObjectPath.Append(Ar.Owner.Name);
		ObjectPath.Append('.');
		ObjectPath.Append(Ar.Name);
		ObjectPath.Append('[');
		ObjectPath.Append(CodeOffset);
		ObjectPath.Append(']');
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("CodeOffset");
		writer.WriteValue(CodeOffset);
		writer.WritePropertyName("ObjectPath");
		writer.WriteValue(ObjectPath.ToString());
	}
}
