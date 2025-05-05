using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class UField : CUE4Parse.UE4.Assets.Exports.UObject
{
	public FPackageIndex? Next;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		if (FFrameworkObjectVersion.Get(Ar) < FFrameworkObjectVersion.Type.RemoveUField_Next)
		{
			Next = new FPackageIndex(Ar);
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		if (Next != null)
		{
			writer.WritePropertyName("Next");
			serializer.Serialize(writer, Next);
		}
	}
}
