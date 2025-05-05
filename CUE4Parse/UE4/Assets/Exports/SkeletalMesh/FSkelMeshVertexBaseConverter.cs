using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FSkelMeshVertexBaseConverter : JsonConverter<FSkelMeshVertexBase>
{
	public override void WriteJson(JsonWriter writer, FSkelMeshVertexBase value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		if (!value.Pos.IsZero())
		{
			writer.WritePropertyName("Pos");
			serializer.Serialize(writer, value.Pos);
		}
		if (value.Normal.Length != 0)
		{
			writer.WritePropertyName("Normal");
			serializer.Serialize(writer, value.Normal);
		}
		if (value.Infs != null)
		{
			writer.WritePropertyName("Infs");
			serializer.Serialize(writer, value.Infs);
		}
		writer.WriteEndObject();
	}

	public override FSkelMeshVertexBase ReadJson(JsonReader reader, Type objectType, FSkelMeshVertexBase existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
