using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine.Curves;

public class UCurveVector : CUE4Parse.UE4.Assets.Exports.UObject
{
	public readonly FRichCurve[] FloatCurves = new FRichCurve[3];

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		for (int i = 0; i < base.Properties.Count; i++)
		{
			if (base.Properties[i].Tag?.GenericValue is UScriptStruct { StructType: FStructFallback structType })
			{
				FloatCurves[i] = new FRichCurve(structType);
			}
		}
		if (FloatCurves.Length != 0)
		{
			base.Properties.Clear();
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("FloatCurves");
		writer.WriteStartArray();
		FRichCurve[] floatCurves = FloatCurves;
		foreach (FRichCurve value in floatCurves)
		{
			serializer.Serialize(writer, value);
		}
		writer.WriteEndArray();
	}
}
