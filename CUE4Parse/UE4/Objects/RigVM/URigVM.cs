using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.RigVM;

public class URigVM : CUE4Parse.UE4.Assets.Exports.UObject
{
	public FRigVMMemoryContainer WorkMemoryStorage;

	public FRigVMMemoryContainer LiteralMemoryStorage;

	public FRigVMByteCode ByteCodeStorage;

	public FRigVMParameter[] Parameters;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		Ar.Position = validPos;
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("WorkMemoryStorage");
		serializer.Serialize(writer, WorkMemoryStorage);
		writer.WritePropertyName("LiteralMemoryStorage");
		serializer.Serialize(writer, LiteralMemoryStorage);
		writer.WritePropertyName("ByteCodeStorage");
		serializer.Serialize(writer, ByteCodeStorage);
		writer.WritePropertyName("Parameters");
		serializer.Serialize(writer, Parameters);
	}
}
