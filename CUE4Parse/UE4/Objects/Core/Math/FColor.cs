using System;
using System.Numerics;
using CUE4Parse.UE4.Writers;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Core.Math;

public readonly struct FColor : IUStruct
{
	public readonly byte B;

	public readonly byte G;

	public readonly byte R;

	public readonly byte A;

	public string Hex
	{
		get
		{
			byte a = A;
			if (a != 1 && a != 0)
			{
				return UnsafePrint.BytesToHex(A, R, G, B);
			}
			return UnsafePrint.BytesToHex(R, G, B);
		}
	}

	public FColor(byte r, byte g, byte b, byte a)
	{
		R = r;
		G = g;
		B = b;
		A = a;
	}

	public void Serialize(FArchiveWriter Ar)
	{
		Ar.Write(R);
		Ar.Write(G);
		Ar.Write(B);
		Ar.Write(A);
	}

	public static implicit operator Vector4(FColor color)
	{
		return new Vector4(Convert.ToSingle(color.R) / 255f, Convert.ToSingle(color.G) / 255f, Convert.ToSingle(color.B) / 255f, Convert.ToSingle(color.A) / 255f);
	}

	public override string ToString()
	{
		return Hex;
	}

	public static byte Requantize16to8(int value16)
	{
		if (value16 < 0 || value16 > 65535)
		{
			throw new ArgumentException("value16");
		}
		return (byte)(value16 * 255 + 32895 >> 16);
	}

	public int ToPackedARGB()
	{
		return A << 24 + R << 16 + G << 8 + B;
	}
}
