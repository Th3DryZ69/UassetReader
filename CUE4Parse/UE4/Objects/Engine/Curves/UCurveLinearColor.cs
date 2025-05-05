using System;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine.Curves;

public class UCurveLinearColor : CUE4Parse.UE4.Assets.Exports.UObject
{
	public readonly FRichCurve[] FloatCurves = new FRichCurve[4];

	private float AdjustBrightness;

	private float AdjustBrightnessCurve;

	private float AdjustVibrance;

	private float AdjustSaturation;

	private float AdjustHue;

	private float AdjustMinAlpha;

	private float AdjustMaxAlpha;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		AdjustBrightness = GetOrDefault("AdjustBrightness", 0f);
		AdjustBrightnessCurve = GetOrDefault("AdjustBrightnessCurve", 0f);
		AdjustVibrance = GetOrDefault("AdjustVibrance", 0f);
		AdjustSaturation = GetOrDefault("AdjustSaturation", 0f);
		AdjustHue = GetOrDefault("AdjustHue", 0f);
		AdjustMinAlpha = GetOrDefault("AdjustMinAlpha", 0f);
		AdjustMaxAlpha = GetOrDefault("AdjustMaxAlpha", 0f);
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

	public FLinearColor GetUnadjustedLinearColorValue(float inTime)
	{
		return new FLinearColor(FloatCurves[0].Eval(inTime), FloatCurves[1].Eval(inTime), FloatCurves[2].Eval(inTime), (FloatCurves[3].Keys.Length == 0) ? 1f : FloatCurves[3].Eval(inTime));
	}

	public FLinearColor GetLinearColorValue(float inTime)
	{
		FLinearColor unadjustedLinearColorValue = GetUnadjustedLinearColorValue(inTime);
		bool num = unadjustedLinearColorValue.R <= 1f && unadjustedLinearColorValue.G <= 1f && unadjustedLinearColorValue.B <= 1f;
		FLinearColor fLinearColor = unadjustedLinearColorValue.LinearRGBToHsv();
		float r = fLinearColor.R;
		float num2 = fLinearColor.G;
		float b = fLinearColor.B;
		b *= AdjustBrightness;
		if (!UnrealMath.IsNearlyEqual(AdjustBrightnessCurve, 1f, 0.0001f) && AdjustBrightnessCurve != 0f)
		{
			b = (float)Math.Pow(b, AdjustBrightnessCurve);
		}
		if (!UnrealMath.IsNearlyZero(AdjustBrightness))
		{
			double num3 = Math.Pow(1f - num2, 5.0);
			double num4 = (double)(Math.Clamp(AdjustVibrance, 0f, 1f) * 0.5f) * num3;
			num2 += (float)num4;
		}
		num2 *= AdjustSaturation;
		r += AdjustHue;
		r = UnrealMath.Fmod(r, 360f);
		if (r < 0f)
		{
			r += 360f;
		}
		num2 = Math.Clamp(num2, 0f, 1f);
		if (num)
		{
			b = Math.Clamp(b, 0f, 1f);
		}
		FLinearColor result = fLinearColor.HSVToLinearRGB();
		result.A = MathUtils.Lerp(AdjustMinAlpha, AdjustMaxAlpha, unadjustedLinearColorValue.A);
		return result;
	}
}
