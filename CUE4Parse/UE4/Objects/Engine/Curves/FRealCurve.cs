using System;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CUE4Parse.UE4.Objects.Engine.Curves;

[StructFallback]
public abstract class FRealCurve : IUStruct
{
	public float DefaultValue;

	[JsonConverter(typeof(StringEnumConverter))]
	public ERichCurveExtrapolation PreInfinityExtrap;

	[JsonConverter(typeof(StringEnumConverter))]
	public ERichCurveExtrapolation PostInfinityExtrap;

	public FRealCurve()
	{
		DefaultValue = float.MaxValue;
		PreInfinityExtrap = ERichCurveExtrapolation.RCCE_Constant;
		PostInfinityExtrap = ERichCurveExtrapolation.RCCE_Constant;
	}

	public FRealCurve(FStructFallback data)
	{
		DefaultValue = data.GetOrDefault("DefaultValue", float.MaxValue);
		PreInfinityExtrap = data.GetOrDefault("PreInfinityExtrap", ERichCurveExtrapolation.RCCE_Constant);
		PostInfinityExtrap = data.GetOrDefault("PostInfinityExtrap", ERichCurveExtrapolation.RCCE_Constant);
	}

	public abstract void RemapTimeValue(ref float inTime, ref float cycleValueOffset);

	public abstract float Eval(float inTime, float inDefaultTime = 0f);

	protected static void CycleTime(float minTime, float maxTime, ref float inTime, ref int cycleCount)
	{
		float num = inTime;
		float num2 = maxTime - minTime;
		if (inTime > maxTime)
		{
			cycleCount = (int)((maxTime - inTime) / num2);
			inTime += num2 * (float)cycleCount;
		}
		else if (inTime < minTime)
		{
			cycleCount = (int)((inTime - minTime) / num2);
			inTime -= num2 * (float)cycleCount;
		}
		if (inTime == maxTime && num < minTime)
		{
			inTime = minTime;
		}
		if (inTime == minTime && num > maxTime)
		{
			inTime = maxTime;
		}
		cycleCount = Math.Abs(cycleCount);
	}
}
