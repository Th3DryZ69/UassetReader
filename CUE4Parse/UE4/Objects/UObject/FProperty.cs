using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FProperty : FField
{
	public int ArrayDim;

	public int ElementSize;

	public EPropertyFlags PropertyFlags;

	public ushort RepIndex;

	public FName RepNotifyFunc;

	public ELifetimeCondition BlueprintReplicationCondition;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		ArrayDim = Ar.Read<int>();
		ElementSize = Ar.Read<int>();
		PropertyFlags = Ar.Read<EPropertyFlags>();
		RepIndex = Ar.Read<ushort>();
		RepNotifyFunc = Ar.ReadFName();
		BlueprintReplicationCondition = (ELifetimeCondition)Ar.Read<byte>();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		if (ArrayDim != 1)
		{
			writer.WritePropertyName("ArrayDim");
			writer.WriteValue(ArrayDim);
		}
		if (ElementSize != 0)
		{
			writer.WritePropertyName("ElementSize");
			writer.WriteValue(ElementSize);
		}
		if (PropertyFlags != EPropertyFlags.None)
		{
			writer.WritePropertyName("PropertyFlags");
			writer.WriteValue(PropertyFlags.ToStringBitfield());
		}
		if (RepIndex != 0)
		{
			writer.WritePropertyName("RepIndex");
			writer.WriteValue(RepIndex);
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
