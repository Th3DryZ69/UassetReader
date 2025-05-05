using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace CUE4Parse.Encryption.Aes;

public static class Aes
{
	public const int ALIGN = 16;

	public const int BLOCK_SIZE = 128;

	private static readonly System.Security.Cryptography.Aes Provider;

	private static readonly byte[] _table1;

	private static readonly uint[] _table2;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte[] Decrypt(this byte[] encrypted, FAesKey key)
	{
		return Provider.CreateDecryptor(key.Key, null).TransformFinalBlock(encrypted, 0, encrypted.Length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte[] Decrypt(this byte[] encrypted, int beginOffset, int count, FAesKey key)
	{
		return Provider.CreateDecryptor(key.Key, null).TransformFinalBlock(encrypted, beginOffset, count);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte[] DecryptApexMobile(this byte[] encrypted, FAesKey key)
	{
		return ApexDecryptData(encrypted, 0, encrypted.Length, key);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte[] DecryptApexMobile(this byte[] encrypted, int beginOffset, int count, FAesKey key)
	{
		return ApexDecryptData(encrypted, beginOffset, count, key);
	}

	private unsafe static byte[] ApexDecryptData(byte[] encrypted, int beginOffset, int count, FAesKey key)
	{
		Span<uint> rk = stackalloc uint[32];
		ApexSetupDecrypt(key, rk);
		byte[] array = new byte[count];
		Buffer.BlockCopy(encrypted, beginOffset, array, 0, count);
		fixed (byte* ptr = array)
		{
			for (int i = 0; i < count; i += 16)
			{
				ApexDecrypt(rk, ptr + i);
			}
		}
		return array;
	}

	private unsafe static void ApexSetupDecrypt(FAesKey key, Span<uint> rk)
	{
		Span<uint> span = stackalloc uint[4];
		span[0] = BitConverter.ToUInt32(key.Key).ByteSwap() ^ 0x1E3DFA;
		span[1] = BitConverter.ToUInt32(key.Key, 4).ByteSwap() ^ 0x78F36777;
		span[2] = BitConverter.ToUInt32(key.Key, 8).ByteSwap() ^ 0xD99D2CF;
		span[3] = BitConverter.ToUInt32(key.Key, 12).ByteSwap() ^ 0x5E144852;
		for (int i = 0; i < 32; i++)
		{
			uint num = _table2[i] ^ span[(i + 1) % 4] ^ span[(i + 2) % 4] ^ span[(i + 3) % 4];
			byte* ptr = (byte*)(&num);
			uint num2 = (uint)(_table1[*ptr] | (_table1[ptr[1]] << 8));
			uint num3 = num2 | (uint)(_table1[ptr[2]] << 16);
			uint num4 = num3 | (uint)(_table1[ptr[3]] << 24);
			uint num5 = num4 ^ span[i % 4] ^ ((num4 >> 19) | (num3 << 13)) ^ ((num4 >> 9) | (num2 << 23));
			span[i % 4] = num5;
			rk[i] = num5;
		}
	}

	private unsafe static void ApexDecrypt(Span<uint> rk, byte* data)
	{
		Span<uint> span = stackalloc uint[4];
		span[0] = (*(uint*)data).ByteSwap();
		span[1] = ((uint*)data)[1].ByteSwap();
		span[2] = ((uint*)data)[2].ByteSwap();
		span[3] = ((uint*)data)[3].ByteSwap();
		for (int i = 0; i < 32; i++)
		{
			int num = i + 1;
			uint num2 = rk[rk.Length - num] ^ span[(i + 1) % 4] ^ span[(i + 2) % 4] ^ span[(i + 3) % 4];
			byte* ptr = (byte*)(&num2);
			byte b = _table1[ptr[3]];
			byte b2 = _table1[*ptr];
			uint num3 = (uint)(b2 | (_table1[ptr[1]] << 8));
			uint num4 = num3 | (uint)(_table1[ptr[2]] << 16);
			uint num5 = num4 | (uint)(b << 24);
			span[i % 4] ^= num5 ^ ((4 * num5) | (uint)(b >> 6)) ^ ((num5 >> 22) | (num4 << 10)) ^ ((num5 >> 14) | (num3 << 18)) ^ ((num5 >> 8) | (uint)(b2 << 24));
		}
		*(uint*)data = span[3].ByteSwap();
		((int*)data)[1] = (int)span[2].ByteSwap();
		((int*)data)[2] = (int)span[1].ByteSwap();
		((int*)data)[3] = (int)span[0].ByteSwap();
	}

	private static uint ByteSwap(this uint value)
	{
		return (value >> 24) | ((value >> 8) & 0xFF00) | ((value << 8) & 0xFF0000) | (value << 24);
	}

	static Aes()
	{
		_table1 = new byte[256]
		{
			32, 219, 75, 70, 238, 15, 188, 172, 67, 50,
			177, 30, 11, 124, 110, 154, 6, 85, 187, 166,
			250, 137, 65, 99, 35, 168, 206, 1, 83, 218,
			135, 120, 46, 129, 73, 112, 40, 207, 96, 43,
			64, 156, 95, 28, 68, 57, 77, 193, 33, 58,
			204, 180, 148, 175, 94, 174, 49, 111, 76, 236,
			157, 248, 212, 84, 254, 26, 18, 203, 244, 142,
			123, 235, 209, 52, 131, 80, 24, 100, 229, 234,
			179, 251, 109, 220, 14, 102, 61, 45, 228, 225,
			106, 27, 246, 240, 60, 13, 34, 194, 92, 81,
			186, 134, 245, 72, 113, 205, 239, 39, 233, 211,
			223, 162, 226, 253, 62, 115, 144, 192, 104, 0,
			255, 184, 127, 130, 138, 149, 178, 47, 29, 90,
			23, 128, 116, 158, 125, 10, 227, 108, 114, 69,
			36, 91, 169, 252, 132, 54, 55, 167, 139, 221,
			22, 86, 200, 201, 19, 66, 230, 196, 8, 161,
			152, 31, 41, 38, 198, 119, 53, 121, 56, 249,
			181, 63, 216, 107, 141, 153, 171, 151, 243, 105,
			173, 210, 145, 231, 155, 176, 165, 147, 160, 118,
			126, 25, 224, 59, 242, 44, 9, 213, 191, 190,
			164, 42, 143, 17, 74, 208, 88, 97, 93, 16,
			237, 202, 182, 87, 103, 214, 195, 163, 215, 189,
			82, 170, 122, 71, 51, 98, 241, 199, 101, 4,
			37, 117, 232, 21, 12, 136, 133, 150, 5, 2,
			146, 247, 3, 197, 78, 140, 217, 159, 185, 79,
			183, 222, 20, 89, 7, 48
		};
		_table2 = new uint[32]
		{
			650731u, 3494209395u, 2139842119u, 2647028271u, 2304468763u, 1438174275u, 596209879u, 2672262383u, 1297426507u, 2327328403u,
			2169967639u, 3054236895u, 2018984315u, 240745267u, 2149952967u, 2430603471u, 1957188123u, 4201348995u, 2740791223u, 2555705503u,
			2279742059u, 2952518435u, 1483080631u, 1988422047u, 3945114411u, 715656531u, 3745078887u, 1802362831u, 681064283u, 2178657459u,
			3995238935u, 2886985215u
		};
		Provider = System.Security.Cryptography.Aes.Create();
		Provider.Mode = CipherMode.ECB;
		Provider.Padding = PaddingMode.None;
		Provider.BlockSize = 128;
	}
}
