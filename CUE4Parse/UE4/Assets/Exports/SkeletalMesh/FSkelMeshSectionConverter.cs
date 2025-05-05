using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FSkelMeshSectionConverter : JsonConverter<FSkelMeshSection>
{
	public override void WriteJson(JsonWriter writer, FSkelMeshSection value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("MaterialIndex");
		writer.WriteValue(value.MaterialIndex);
		writer.WritePropertyName("BaseIndex");
		writer.WriteValue(value.BaseIndex);
		writer.WritePropertyName("NumTriangles");
		writer.WriteValue(value.NumTriangles);
		writer.WritePropertyName("bRecomputeTangent");
		writer.WriteValue(value.bRecomputeTangent);
		writer.WritePropertyName("RecomputeTangentsVertexMaskChannel");
		writer.WriteValue(value.RecomputeTangentsVertexMaskChannel.ToString());
		writer.WritePropertyName("bCastShadow");
		writer.WriteValue(value.bCastShadow);
		writer.WritePropertyName("bVisibleInRayTracing");
		writer.WriteValue(value.bVisibleInRayTracing);
		writer.WritePropertyName("bLegacyClothingSection");
		writer.WriteValue(value.bLegacyClothingSection);
		writer.WritePropertyName("CorrespondClothSectionIndex");
		writer.WriteValue(value.CorrespondClothSectionIndex);
		writer.WritePropertyName("BaseVertexIndex");
		writer.WriteValue(value.BaseVertexIndex);
		writer.WritePropertyName("NumVertices");
		writer.WriteValue(value.NumVertices);
		writer.WritePropertyName("MaxBoneInfluences");
		writer.WriteValue(value.MaxBoneInfluences);
		writer.WritePropertyName("bUse16BitBoneIndex");
		writer.WriteValue(value.bUse16BitBoneIndex);
		writer.WritePropertyName("CorrespondClothAssetIndex");
		writer.WriteValue(value.CorrespondClothAssetIndex);
		writer.WritePropertyName("bDisabled");
		writer.WriteValue(value.bDisabled);
		writer.WritePropertyName("GenerateUpToLodIndex");
		writer.WriteValue(value.GenerateUpToLodIndex);
		writer.WritePropertyName("OriginalDataSectionIndex");
		writer.WriteValue(value.OriginalDataSectionIndex);
		writer.WritePropertyName("ChunkedParentSectionIndex");
		writer.WriteValue(value.ChunkedParentSectionIndex);
		writer.WriteEndObject();
	}

	public override FSkelMeshSection ReadJson(JsonReader reader, Type objectType, FSkelMeshSection existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
