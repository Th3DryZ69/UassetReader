using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Nanite;

public class FCluster
{
	public const int NANITE_MIN_POSITION_PRECISION = -8;

	public uint NumVerts;

	public uint PositionOffset;

	public uint NumTris;

	public uint IndexOffset;

	public uint ColorMin;

	public uint ColorBits;

	public uint GroupIndex;

	public FIntVector PosStart;

	public uint BitsPerIndex;

	public int PosPrecision;

	public uint PosBitsX;

	public uint PosBitsY;

	public uint PosBitsZ;

	public FVector4 LODBounds;

	public FVector BoxBoundsCenter;

	public float LODError;

	public float EdgeLength;

	public FVector BoxBoundsExtent;

	public uint Flags;

	public uint AttributeOffset;

	public uint BitsPerAttribute;

	public uint DecodeInfoOffset;

	public uint NumUVs;

	public uint ColorMode;

	public uint UV_Prec;

	public uint MaterialTableOffset;

	public uint MaterialTableLength;

	public uint Material0Index;

	public uint Material1Index;

	public uint Material2Index;

	public uint Material0Length;

	public uint Material1Length;

	public uint VertReuseBatchCountTableOffset;

	public uint VertReuseBatchCountTableSize;

	public TIntVector4<uint> VertReuseBatchInfo;

	public FCluster(FArchive Ar)
	{
		uint value = Ar.Read<uint>();
		NumVerts = GetBits(value, 9, 0);
		PositionOffset = GetBits(value, 23, 9);
		uint value2 = Ar.Read<uint>();
		NumTris = GetBits(value2, 8, 0);
		IndexOffset = GetBits(value2, 24, 8);
		ColorMin = Ar.Read<uint>();
		uint value3 = Ar.Read<uint>();
		ColorBits = GetBits(value3, 16, 0);
		GroupIndex = GetBits(value3, 16, 16);
		PosStart = Ar.Read<FIntVector>();
		uint value4 = Ar.Read<uint>();
		BitsPerIndex = GetBits(value4, 4, 0);
		PosPrecision = (int)GetBits(value4, 5, 4) + -8;
		PosBitsX = GetBits(value4, 5, 9);
		PosBitsY = GetBits(value4, 5, 14);
		PosBitsZ = GetBits(value4, 5, 19);
		LODBounds = Ar.Read<FVector4>();
		BoxBoundsCenter = Ar.Read<FVector>();
		uint num = Ar.Read<uint>();
		LODError = num;
		EdgeLength = num >> 16;
		BoxBoundsExtent = Ar.Read<FVector>();
		Flags = Ar.Read<uint>();
		uint value5 = Ar.Read<uint>();
		AttributeOffset = GetBits(value5, 22, 0);
		BitsPerAttribute = GetBits(value5, 10, 22);
		uint value6 = Ar.Read<uint>();
		DecodeInfoOffset = GetBits(value6, 22, 0);
		NumUVs = GetBits(value6, 3, 22);
		ColorMode = GetBits(value6, 2, 25);
		UV_Prec = Ar.Read<uint>();
		uint num2 = Ar.Read<uint>();
		if (num2 < 4261412864u)
		{
			MaterialTableOffset = 0u;
			MaterialTableLength = 0u;
			Material0Index = GetBits(num2, 6, 0);
			Material1Index = GetBits(num2, 6, 6);
			Material2Index = GetBits(num2, 6, 12);
			Material0Length = GetBits(num2, 7, 18) + 1;
			Material1Length = GetBits(num2, 7, 25);
			VertReuseBatchCountTableOffset = 0u;
			VertReuseBatchCountTableSize = 0u;
			VertReuseBatchInfo = Ar.Read<TIntVector4<uint>>();
		}
		else
		{
			MaterialTableOffset = GetBits(num2, 19, 0);
			MaterialTableLength = GetBits(num2, 6, 19) + 1;
			Material0Index = 0u;
			Material1Index = 0u;
			Material2Index = 0u;
			Material0Length = 0u;
			Material1Length = 0u;
			VertReuseBatchCountTableOffset = Ar.Read<uint>();
			VertReuseBatchCountTableSize = Ar.Read<uint>();
			VertReuseBatchInfo = default(TIntVector4<uint>);
		}
	}

	public static uint GetBits(uint value, int numBits, int offset)
	{
		uint num = (uint)((1 << numBits) - 1);
		return (value >> offset) & num;
	}
}
