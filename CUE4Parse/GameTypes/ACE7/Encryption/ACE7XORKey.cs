using System.Numerics;

namespace CUE4Parse.GameTypes.ACE7.Encryption;

public class ACE7XORKey
{
	public int NameKey;

	public int Offset;

	public int pk1;

	public int pk2;

	private static int CalcNameKey(string fname)
	{
		fname = fname.ToUpper();
		int num = 0;
		for (int i = 0; i < fname.Length; i++)
		{
			int num2 = (byte)fname[i];
			num ^= num2;
			num2 = num * 8;
			num2 ^= num;
			int num3 = num + num;
			num2 = ~num2;
			num2 = (num2 >> 7) & 1;
			num = num2 | num3;
		}
		return num;
	}

	private static void CalcPKeyFromNKey(int nkey, int dataoffset, out int pk1, out int pk2)
	{
		long num = (uint)((long)nkey * 7L);
		BigInteger bigInteger = new BigInteger(5440514381186227205L);
		num += dataoffset;
		long num2 = (long)(bigInteger * num >> 70);
		long num3 = num2 >> 63;
		num2 += num3;
		num3 = num2 * 217;
		num -= num3;
		pk1 = (int)(num & 0xFFFFFFFFu);
		long num4 = (uint)((long)nkey * 11L) + dataoffset;
		num2 = 0L;
		num2 &= 0x3FF;
		long num5 = ((num4 + num2) & 0x3FF) - num2;
		pk2 = (int)(num5 & 0xFFFFFFFFu);
	}

	public ACE7XORKey(string fname)
	{
		NameKey = CalcNameKey(fname);
		Offset = 4;
		CalcPKeyFromNKey(NameKey, Offset, out pk1, out pk2);
	}
}
