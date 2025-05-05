using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public class ULightMapTexture2D : UTexture2D
{
	public ELightMapFlags LightmapFlags;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		LightmapFlags = Ar.Read<ELightMapFlags>();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("LightmapFlags");
		writer.WriteValue(LightmapFlags.ToStringBitfield());
	}
}
