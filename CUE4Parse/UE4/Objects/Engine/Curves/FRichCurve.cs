using System;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Engine.Curves;

public class FRichCurve : FRealCurve
{
	public FRichCurveKey[] Keys;

	public FRichCurve()
	{
		Keys = Array.Empty<FRichCurveKey>();
	}

	public FRichCurve(FStructFallback data)
		: base(data)
	{
		Keys = data.GetOrDefault("Keys", Array.Empty<FRichCurveKey>());
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

	private float EvalForTwoKeys(FRichCurveKey key1, FRichCurveKey key2, float inTime)
	{
		float num = key2.Time - key1.Time;
		if (num > 0f && key1.InterpMode != ERichCurveInterpMode.RCIM_Constant)
		{
			float alpha = (inTime - key1.Time) / num;
			float value = key1.Value;
			float value2 = key2.Value;
			if (key1.InterpMode == ERichCurveInterpMode.RCIM_Linear)
			{
				return MathUtils.Lerp(value, value2, alpha);
			}
			if (IsItNotWeighted(key1, key2))
			{
				float p = value + key1.LeaveTangent * num * (1f / 3f);
				float p2 = value2 - key2.ArriveTangent * num * (1f / 3f);
				return BezierInterp(value, p, p2, value2, alpha);
			}
			return WeightedEvalForTwoKeys(key1, key2, inTime);
		}
		return key1.Value;
	}

	private float WeightedEvalForTwoKeys(FRichCurveKey key1, FRichCurveKey key2, float inTime)
	{
		float num = key2.Time - key1.Time;
		float num2 = (inTime - key1.Time) / num;
		float value = key1.Value;
		float value2 = key2.Value;
		float num3 = 1f / 3f;
		float time = key1.Time;
		float time2 = key2.Time;
		float num4 = time2 - time;
		double num5 = Math.Atan(key1.LeaveTangent);
		double num6 = Math.Cos(num5);
		double num7 = Math.Sin(num5);
		double num8 = key1.LeaveTangentWeight;
		ERichCurveTangentWeightMode tangentWeightMode = key1.TangentWeightMode;
		if (tangentWeightMode == ERichCurveTangentWeightMode.RCTWM_WeightedNone || tangentWeightMode == ERichCurveTangentWeightMode.RCTWM_WeightedArrive)
		{
			float num9 = key1.LeaveTangent * num4;
			num8 = Math.Sqrt(num4 * num4 + num9 * num9) * (double)num3;
		}
		double num10 = num6 * num8 + (double)time;
		double num11 = num7 * num8 + (double)key1.Value;
		double d = Math.Atan(key2.ArriveTangent);
		num6 = Math.Cos(d);
		double num12 = Math.Cos(d);
		double num13 = key2.ArriveTangentWeight;
		tangentWeightMode = key2.TangentWeightMode;
		if (tangentWeightMode == ERichCurveTangentWeightMode.RCTWM_WeightedNone || tangentWeightMode == ERichCurveTangentWeightMode.RCTWM_WeightedLeave)
		{
			float num14 = key2.ArriveTangent * num4;
			num13 = Math.Sqrt(num4 * num4 + num14 * num14) * (double)num3;
		}
		double num15 = (0.0 - num6) * num13 + (double)time2;
		double num16 = (0.0 - num12) * num13 + (double)key2.Value;
		float num17 = time2 - time;
		double num18 = num10 - (double)time;
		double num19 = num15 - (double)time;
		double b = num18 / (double)num17;
		double c = num19 / (double)num17;
		double[] solution = new double[3];
		BezierToPower(0.0, b, c, 1.0, out double[] output);
		output[0] -= num2;
		float num20;
		if (CubicCurve2D.SolveCubic(ref output, ref solution) == 1)
		{
			num20 = (float)solution[0];
		}
		else
		{
			num20 = float.MinValue;
			double[] array = solution;
			foreach (double num21 in array)
			{
				if (num21 >= 0.0 && num21 <= 1.0 && (num20 < 0f || num21 > (double)num20))
				{
					num20 = (float)num21;
				}
			}
			if (num20 == float.MinValue)
			{
				num20 = 0f;
			}
		}
		return BezierInterp(value, (float)num11, (float)num16, value2, num20);
	}

	private void BezierToPower(double a1, double b1, double c1, double d1, out double[] output)
	{
		double[] array = new double[4];
		double num = b1 - a1;
		double num2 = c1 - b1;
		double num3 = d1 - c1;
		double num4 = num2 - num;
		array[3] = num3 - num2 - num4;
		array[2] = 3.0 * num4;
		array[1] = 3.0 * num;
		array[0] = a1;
		output = array;
	}

	private float BezierInterp(float p0, float p1, float p2, float p3, float alpha)
	{
		float a = MathUtils.Lerp(p0, p1, alpha);
		float num = MathUtils.Lerp(p1, p2, alpha);
		float b = MathUtils.Lerp(p2, p3, alpha);
		float a2 = MathUtils.Lerp(a, num, alpha);
		float b2 = MathUtils.Lerp(num, b, alpha);
		return MathUtils.Lerp(a2, b2, alpha);
	}

	private static bool IsItNotWeighted(FRichCurveKey key1, FRichCurveKey key2)
	{
		ERichCurveTangentWeightMode tangentWeightMode = key1.TangentWeightMode;
		if (tangentWeightMode == ERichCurveTangentWeightMode.RCTWM_WeightedNone || tangentWeightMode == ERichCurveTangentWeightMode.RCTWM_WeightedArrive)
		{
			tangentWeightMode = key2.TangentWeightMode;
			return tangentWeightMode == ERichCurveTangentWeightMode.RCTWM_WeightedNone || tangentWeightMode == ERichCurveTangentWeightMode.RCTWM_WeightedLeave;
		}
		return false;
	}
}
