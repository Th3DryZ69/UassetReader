using System;

namespace CUE4Parse.Utils;

public static class UnsafePrint
{
	public unsafe static string BytesToHex(byte* bytes, uint length)
	{
		char[] array = new char[length * 2];
		int num = 0;
		int num2 = 0;
		while (num < length)
		{
			byte b = (byte)(bytes[num] >> 4);
			array[num2] = (char)((b > 9) ? (b - 10 + 65) : (b + 48));
			b = (byte)(bytes[num] & 0xF);
			array[++num2] = (char)((b > 9) ? (b - 10 + 65) : (b + 48));
			num++;
			num2++;
		}
		return new string(array);
	}

	public static string BytesToHex(params byte[] bytes)
	{
		return BitConverter.ToString(bytes).Replace("-", "");
	}
}
