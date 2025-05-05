using System;
using System.Numerics;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.RenderCore;

[JsonConverter(typeof(FPackedNormalConverter))]
public class FPackedNormal
{
	public uint Data;

	public float X => (float)(Data & 0xFF) / 127.5f - 1f;

	public float Y => (float)((Data >> 8) & 0xFF) / 127.5f - 1f;

	public float Z => (float)((Data >> 16) & 0xFF) / 127.5f - 1f;

	public float W => (float)((Data >> 24) & 0xFF) / 127.5f - 1f;

	public FPackedNormal(FArchive Ar)
	{
		Data = Ar.Read<uint>();
		if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.IncreaseNormalPrecision)
		{
			Data ^= 2155905152u;
		}
	}

	public FPackedNormal(uint data)
	{
		Data = data;
	}

	public FPackedNormal(FVector vector)
	{
		Data = (uint)((int)((double)vector.X + 127.5) + (int)((double)vector.Y + 127.5) << 8 + (int)((double)vector.Z + 127.5) << 16);
	}

	public FPackedNormal(FVector4 vector)
	{
		Data = (uint)((int)((double)vector.X + 127.5) + (int)((double)vector.Y + 127.5) << 8 + (int)((double)vector.Z + 127.5) << 16 + (int)((double)vector.W + 127.5) << 24);
	}

	public void SetW(float value)
	{
		Data = (Data & 0xFFFFFF) | (uint)((int)Math.Round(value * 127f) << 24);
	}

	public float GetW()
	{
		return (float)(int)(byte)(Data >> 24) / 127f;
	}

	public static explicit operator FVector(FPackedNormal packedNormal)
	{
		return new FVector(packedNormal.X, packedNormal.Y, packedNormal.Z);
	}

	public static implicit operator FVector4(FPackedNormal packedNormal)
	{
		return new FVector4(packedNormal.X, packedNormal.Y, packedNormal.Z, packedNormal.W);
	}

	public static explicit operator Vector3(FPackedNormal packedNormal)
	{
		return new Vector3(packedNormal.X, packedNormal.Y, packedNormal.Z);
	}

	public static implicit operator Vector4(FPackedNormal packedNormal)
	{
		return new Vector4(packedNormal.X, packedNormal.Y, packedNormal.Z, packedNormal.W);
	}

	public static bool operator ==(FPackedNormal a, FPackedNormal b)
	{
		if (a.Data == b.Data && a.X == b.X && a.Y == b.Y && a.Z == b.Z)
		{
			return a.W == b.W;
		}
		return false;
	}

	public static bool operator !=(FPackedNormal a, FPackedNormal b)
	{
		if (a.Data == b.Data && a.X == b.X && a.Y == b.Y && a.Z == b.Z)
		{
			return a.W != b.W;
		}
		return true;
	}
}
