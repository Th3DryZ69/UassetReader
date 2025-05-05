using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.RenderCore;

[JsonConverter(typeof(FPackedRGBA16NConverter))]
public class FPackedRGBA16N
{
	public readonly uint X;

	public readonly uint Y;

	public readonly uint Z;

	public readonly uint W;

	public FPackedRGBA16N(FArchive Ar)
	{
		X = Ar.Read<ushort>();
		Y = Ar.Read<ushort>();
		Z = Ar.Read<ushort>();
		W = Ar.Read<ushort>();
		if (Ar.Game >= EGame.GAME_UE4_20)
		{
			X ^= 32768u;
			Y ^= 32768u;
			Z ^= 32768u;
			W ^= 32768u;
		}
	}

	public static explicit operator FVector(FPackedRGBA16N packedRGBA16N)
	{
		float x = ((float)packedRGBA16N.X - 32767.5f) / 32767.5f;
		float y = ((float)packedRGBA16N.Y - 32767.5f) / 32767.5f;
		float z = ((float)packedRGBA16N.Z - 32767.5f) / 32767.5f;
		return new FVector(x, y, z);
	}

	public static explicit operator FVector4(FPackedRGBA16N packedRGBA16N)
	{
		float x = ((float)packedRGBA16N.X - 32767.5f) / 32767.5f;
		float y = ((float)packedRGBA16N.Y - 32767.5f) / 32767.5f;
		float z = ((float)packedRGBA16N.Z - 32767.5f) / 32767.5f;
		float w = ((float)packedRGBA16N.W - 32767.5f) / 32767.5f;
		return new FVector4(x, y, z, w);
	}

	public static explicit operator FPackedNormal(FPackedRGBA16N packedRGBA16N)
	{
		return new FPackedNormal((FVector)packedRGBA16N);
	}
}
