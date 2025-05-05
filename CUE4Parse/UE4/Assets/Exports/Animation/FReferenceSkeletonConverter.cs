using System;
using CUE4Parse.UE4.Objects.Core.Math;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FReferenceSkeletonConverter : JsonConverter<FReferenceSkeleton>
{
	public override void WriteJson(JsonWriter writer, FReferenceSkeleton value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("FinalRefBoneInfo");
		writer.WriteStartArray();
		FMeshBoneInfo[] finalRefBoneInfo = value.FinalRefBoneInfo;
		foreach (FMeshBoneInfo fMeshBoneInfo in finalRefBoneInfo)
		{
			serializer.Serialize(writer, fMeshBoneInfo);
		}
		writer.WriteEndArray();
		writer.WritePropertyName("FinalRefBonePose");
		writer.WriteStartArray();
		FTransform[] finalRefBonePose = value.FinalRefBonePose;
		foreach (FTransform value2 in finalRefBonePose)
		{
			serializer.Serialize(writer, value2);
		}
		writer.WriteEndArray();
		writer.WritePropertyName("FinalNameToIndexMap");
		serializer.Serialize(writer, value.FinalNameToIndexMap);
		writer.WriteEndObject();
	}

	public override FReferenceSkeleton ReadJson(JsonReader reader, Type objectType, FReferenceSkeleton existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
