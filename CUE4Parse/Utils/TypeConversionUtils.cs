using System;

namespace CUE4Parse.Utils;

public static class TypeConversionUtils
{
	public static float HalfToFloat(ushort fp16)
	{
		float num = BitConverter.Int32BitsToSingle(947912704);
		int num2 = (fp16 & 0x7FFF) << 13;
		long num3 = 0xF800000L & (long)num2;
		num2 += 939524096;
		switch (num3)
		{
		case 260046848L:
			num2 += 939524096;
			break;
		case 0L:
			num2 += 8388608;
			num2 = BitConverter.SingleToInt32Bits(BitConverter.Int32BitsToSingle(num2) - num);
			break;
		}
		num2 |= (fp16 & 0x8000) << 16;
		return BitConverter.Int32BitsToSingle(num2);
	}
}
