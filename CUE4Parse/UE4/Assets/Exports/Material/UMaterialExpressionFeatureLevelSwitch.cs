using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class UMaterialExpressionFeatureLevelSwitch : UMaterialExpression
{
	public FExpressionInput[] Inputs = new FExpressionInput[5];

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		List<FPropertyTag> list = new List<FPropertyTag>();
		int num = 0;
		foreach (FPropertyTag property in base.Properties)
		{
			if (property.Tag?.GenericValue is UScriptStruct { StructType: FExpressionInput structType } && property.Name.Text == "Inputs")
			{
				Inputs[num] = structType;
				list.Add(property);
				num++;
			}
		}
		foreach (FPropertyTag item in list)
		{
			base.Properties.Remove(item);
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("Inputs");
		writer.WriteStartArray();
		FExpressionInput[] inputs = Inputs;
		foreach (FExpressionInput value in inputs)
		{
			serializer.Serialize(writer, value);
		}
		writer.WriteEndArray();
	}
}
