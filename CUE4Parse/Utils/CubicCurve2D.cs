using System;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.Utils;

public class CubicCurve2D
{
	public static int SolveCubic(ref double[] coeff, ref double[] solution)
	{
		double num = coeff[2] / coeff[3];
		double num2 = coeff[1] / coeff[3];
		double num3 = coeff[0] / coeff[3];
		double num4 = num * num;
		double num5 = 1.0 / 3.0 * (-1.0 / 3.0 * num4 + num2);
		double num6 = 0.5 * (2.0 / 27.0 * num * num4 - 1.0 / 3.0 * num * num2 + num3);
		double num7 = num5 * num5 * num5;
		double num8 = num6 * num6 + num7;
		int num9;
		if (UnrealMath.IsNearlyZero(num8))
		{
			if (UnrealMath.IsNearlyZero(num6))
			{
				solution[0] = 0.0;
				num9 = 1;
			}
			else
			{
				double num10 = Cbrt(0.0 - num6);
				solution[0] = 2.0 * num10;
				solution[1] = 0.0 - num10;
				num9 = 2;
			}
		}
		else if (num8 < 0.0)
		{
			double num11 = 1.0 / 3.0 * Math.Acos((0.0 - num6) / Math.Sqrt(0.0 - num7));
			double num12 = 2.0 * Math.Sqrt(0.0 - num5);
			solution[0] = num12 * Math.Cos(num11);
			solution[1] = (0.0 - num12) * Math.Cos(num11 + Math.PI / 3.0);
			solution[2] = (0.0 - num12) * Math.Cos(num11 - Math.PI / 3.0);
			num9 = 3;
		}
		else
		{
			double num13 = Math.Sqrt(num8);
			double num14 = Cbrt(num13 - num6);
			double num15 = 0.0 - Cbrt(num13 + num6);
			solution[0] = num14 + num15;
			num9 = 1;
		}
		double num16 = 1.0 / 3.0 * num;
		for (int i = 0; i < num9; i++)
		{
			solution[i] -= num16;
		}
		return num9;
	}

	private static double Cbrt(double x)
	{
		if (!(x > 0.0))
		{
			if (!(x < 0.0))
			{
				return 0.0;
			}
			return 0.0 - Math.Pow(0.0 - x, 1.0 / 3.0);
		}
		return Math.Pow(x, 1.0 / 3.0);
	}
}
