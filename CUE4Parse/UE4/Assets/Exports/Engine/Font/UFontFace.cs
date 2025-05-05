using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Engine.Font;

public class UFontFace : UObject
{
	public FFontFaceData? FontFaceData;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		if (Ar.ReadBoolean())
		{
			FontFaceData = new FFontFaceData(Ar);
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		_ = FontFaceData;
	}
}
