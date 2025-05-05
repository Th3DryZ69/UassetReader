using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class UProperty : UField
{
	public int ArrayDim;

	public EPropertyFlags PropertyFlags;

	public FName RepNotifyFunc;

	public ELifetimeCondition BlueprintReplicationCondition;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		ArrayDim = Ar.Read<int>();
		PropertyFlags = Ar.Read<EPropertyFlags>();
		RepNotifyFunc = Ar.ReadFName();
		if (FReleaseObjectVersion.Get(Ar) >= FReleaseObjectVersion.Type.PropertiesSerializeRepCondition)
		{
			BlueprintReplicationCondition = (ELifetimeCondition)Ar.Read<byte>();
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		if (ArrayDim != 1)
		{
			writer.WritePropertyName("ArrayDim");
			writer.WriteValue(ArrayDim);
		}
		if (PropertyFlags != EPropertyFlags.None)
		{
			writer.WritePropertyName("PropertyFlags");
			writer.WriteValue(PropertyFlags.ToStringBitfield());
		}
		if (!RepNotifyFunc.IsNone)
		{
			writer.WritePropertyName("RepNotifyFunc");
			serializer.Serialize(writer, RepNotifyFunc);
		}
		if (BlueprintReplicationCondition != ELifetimeCondition.COND_None)
		{
			writer.WritePropertyName("BlueprintReplicationCondition");
			writer.WriteValue(BlueprintReplicationCondition.ToString());
		}
	}
}
