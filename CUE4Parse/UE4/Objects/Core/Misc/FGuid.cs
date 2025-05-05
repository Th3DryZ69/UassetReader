using System;
using System.Linq;
using System.Text;
using CUE4Parse.UE4.Objects.Core.Math;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Core.Misc;

[JsonConverter(typeof(FGuidConverter))]
public struct FGuid : IUStruct
{
	public readonly uint A;

	public readonly uint B;

	public readonly uint C;

	public readonly uint D;

	public FGuid(uint v)
	{
		A = (B = (C = (D = v)));
	}

	public FGuid(uint a, uint b, uint c, uint d)
	{
		A = a;
		B = b;
		C = c;
		D = d;
	}

	public FGuid(string hexString)
	{
		A = Convert.ToUInt32(hexString.Substring(0, 8), 16);
		B = Convert.ToUInt32(hexString.Substring(8, 8), 16);
		C = Convert.ToUInt32(hexString.Substring(16, 8), 16);
		D = Convert.ToUInt32(hexString.Substring(24, 8), 16);
	}

	public bool IsValid()
	{
		return (A | B | C | D) != 0;
	}

	public readonly string ToString(EGuidFormats guidFormat)
	{
		switch (guidFormat)
		{
		case EGuidFormats.DigitsWithHyphens:
			return $"{A:X8}-{B >> 16:X4}-{B & 0xFFFF:X4}-{C >> 16:X4}-{C & 0xFFFF:X4}{D:X8}";
		case EGuidFormats.DigitsWithHyphensInBraces:
			return $"{{{A:X8}-{B >> 16:X4}-{B & 0xFFFF:X4}-{C >> 16:X4}-{C & 0xFFFF:X4}{D:X8}}}";
		case EGuidFormats.DigitsWithHyphensInParentheses:
			return $"({A:X8}-{B >> 16:X4}-{B & 0xFFFF:X4}-{C >> 16:X4}-{C & 0xFFFF:X4}{D:X8})";
		case EGuidFormats.HexValuesInBraces:
			return $"{{0x{A:X8},0x{B >> 16:X4},0x{B & 0xFFFF:X4},{{0x{C >> 24:X2},0x{(C >> 16) & 0xFF:X2},0x{(C >> 8) & 0xFF:X2},0x{C & 0xFF:X2},0x{D >> 24:X2},0x{(D >> 16) & 0xFF:X2},0x{(D >> 8) & 0xFF:X2},0x{D & 0xFF:X2}}}}}";
		case EGuidFormats.UniqueObjectGuid:
			return $"{A:X8}-{B:X8}-{C:X8}-{D:X8}";
		case EGuidFormats.Short:
		{
			string text = Convert.ToBase64String(BitConverter.GetBytes(A).Concat(BitConverter.GetBytes(B)).Concat(BitConverter.GetBytes(C))
				.Concat(BitConverter.GetBytes(D))
				.ToArray()).Replace('+', '-').Replace('/', '_');
			if (text.Length == 24)
			{
				text = text.Substring(0, text.Length - 2);
			}
			return text;
		}
		case EGuidFormats.Base36Encoded:
		{
			char[] array = new char[36]
			{
				'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
				'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
				'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
				'U', 'V', 'W', 'X', 'Y', 'Z'
			};
			FUInt128 other = new FUInt128(0uL);
			FUInt128 fUInt = new FUInt128(A, B, C, D);
			StringBuilder stringBuilder = new StringBuilder(26);
			while (fUInt.IsGreater(other))
			{
				fUInt = fUInt.Divide(36u, out var remainder);
				stringBuilder.Insert(0, array[remainder]);
			}
			for (int i = stringBuilder.Length; i < 25; i++)
			{
				stringBuilder.Insert(0, '0');
			}
			stringBuilder.Insert(0, 0);
			return stringBuilder.ToString();
		}
		default:
			return $"{A:X8}{B:X8}{C:X8}{D:X8}";
		}
	}

	public override string ToString()
	{
		return ToString(EGuidFormats.Digits);
	}

	public static bool operator ==(FGuid one, FGuid two)
	{
		if (one.A == two.A && one.B == two.B && one.C == two.C)
		{
			return one.D == two.D;
		}
		return false;
	}

	public static bool operator !=(FGuid one, FGuid two)
	{
		if (one.A == two.A && one.B == two.B && one.C == two.C)
		{
			return one.D != two.D;
		}
		return true;
	}

	public static implicit operator FGuid(Guid g)
	{
		return new FGuid(g.ToString().Replace("-", ""));
	}
}
