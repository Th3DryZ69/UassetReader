using System;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Engine.Curves;

[StructFallback]
public class FSimpleCurve : FRealCurve
{
	public ERichCurveInterpMode InterpMode;

	public FSimpleCurveKey[] Keys;

	public FSimpleCurve()
	{
		InterpMode = ERichCurveInterpMode.RCIM_Linear;
		Keys = Array.Empty<FSimpleCurveKey>();
	}

	public FSimpleCurve(FStructFallback data)
	{
		InterpMode = data.GetOrDefault("InterpMode", ERichCurveInterpMode.RCIM_Linear);
		Keys = data.GetOrDefault("Keys", Array.Empty<FSimpleCurveKey>());
	}

	public override void RemapTimeValue(ref float inTime, ref float cycleValueOffset)
	{
		int num = Keys.Length;
		if (num < 2)
		{
			return;
		}
		if (inTime <= Keys[0].Time)
		{
			if (PreInfinityExtrap != ERichCurveExtrapolation.RCCE_Linear && PreInfinityExtrap != ERichCurveExtrapolation.RCCE_Constant)
			{
				float time = Keys[0].Time;
				float time2 = Keys[num - 1].Time;
				int cycleCount = 0;
				FRealCurve.CycleTime(time, time2, ref inTime, ref cycleCount);
				if (PreInfinityExtrap == ERichCurveExtrapolation.RCCE_CycleWithOffset)
				{
					float num2 = Keys[0].Value - Keys[num - 1].Value;
					cycleValueOffset = num2 * (float)cycleCount;
				}
				else if (PreInfinityExtrap == ERichCurveExtrapolation.RCCE_Oscillate && cycleCount % 2 == 1)
				{
					inTime = time + (time2 - inTime);
				}
			}
		}
		else if (inTime >= Keys[num - 1].Time && PostInfinityExtrap != ERichCurveExtrapolation.RCCE_Linear && PostInfinityExtrap != ERichCurveExtrapolation.RCCE_Constant)
		{
			float time3 = Keys[0].Time;
			float time4 = Keys[num - 1].Time;
			int cycleCount2 = 0;
			FRealCurve.CycleTime(time3, time4, ref inTime, ref cycleCount2);
			if (PostInfinityExtrap == ERichCurveExtrapolation.RCCE_CycleWithOffset)
			{
				float num3 = Keys[num - 1].Value - Keys[0].Value;
				cycleValueOffset = num3 * (float)cycleCount2;
			}
			else if (PostInfinityExtrap == ERichCurveExtrapolation.RCCE_Oscillate && cycleCount2 % 2 == 1)
			{
				inTime = time3 + (time4 - inTime);
			}
		}
	}

	public override float Eval(float inTime, float inDefaultValue = 0f)
	{
		float num = 0f;
		float inTime2 = inTime;
		float cycleValueOffset = num;
		RemapTimeValue(ref inTime2, ref cycleValueOffset);
		inTime = inTime2;
		num = cycleValueOffset;
		int num2 = Keys.Length;
		float num3 = ((DefaultValue == float.MaxValue) ? inDefaultValue : DefaultValue);
		if (num2 != 0)
		{
			if (num2 < 2 || inTime <= Keys[0].Time)
			{
				if (PreInfinityExtrap == ERichCurveExtrapolation.RCCE_Linear && num2 > 1)
				{
					float num4 = Keys[1].Time - Keys[0].Time;
					num3 = ((!(Math.Abs(num4) <= 1E-08f)) ? ((Keys[1].Value - Keys[0].Value) / num4 * (inTime - Keys[0].Time) + Keys[0].Value) : Keys[0].Value);
				}
				else
				{
					num3 = Keys[0].Value;
				}
			}
			else if (inTime < Keys[num2 - 1].Time)
			{
				int num5 = 1;
				int num6 = num2 - 1 - num5;
				while (num6 > 0)
				{
					int num7 = num6 / 2;
					int num8 = num5 + num7;
					if (inTime >= Keys[num8].Time)
					{
						num5 = num8 + 1;
						num6 -= num7 + 1;
					}
					else
					{
						num6 = num7;
					}
				}
				num3 = EvalForTwoKeys(Keys[num5 - 1], Keys[num5], inTime);
			}
			else if (PostInfinityExtrap == ERichCurveExtrapolation.RCCE_Linear)
			{
				float num9 = Keys[num2 - 2].Time - Keys[num2 - 1].Time;
				num3 = ((!(Math.Abs(num9) <= 1E-08f)) ? ((Keys[num2 - 2].Value - Keys[num2 - 1].Value) / num9 * (inTime - Keys[num2 - 1].Time) + Keys[num2 - 1].Value) : Keys[num2 - 1].Value);
			}
			else
			{
				num3 = Keys[num2 - 1].Value;
			}
		}
		return num3 + num;
	}

	private float EvalForTwoKeys(FSimpleCurveKey key1, FSimpleCurveKey key2, float inTime)
	{
		float num = key2.Time - key1.Time;
		if (num > 0f && InterpMode != ERichCurveInterpMode.RCIM_Constant)
		{
			float alpha = (inTime - key1.Time) / num;
			float value = key1.Value;
			float value2 = key2.Value;
			return MathUtils.Lerp(value, value2, alpha);
		}
		return key1.Value;
	}
}
