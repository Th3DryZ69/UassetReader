using System;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FRawStaticIndexBuffer
{
	public ushort[] Indices16;

	public uint[] Indices32;

	public int Length
	{
		get
		{
			if (Indices32.Length != 0)
			{
				return Indices32.Length;
			}
			if (Indices16.Length != 0)
			{
				return Indices16.Length;
			}
			return -1;
		}
	}

	public int this[int i]
	{
		get
		{
			if (Indices32.Length != 0)
			{
				return (int)Indices32[i];
			}
			if (Indices16.Length != 0)
			{
				return Indices16[i];
			}
			throw new IndexOutOfRangeException();
		}
	}

	public int this[long i]
	{
		get
		{
			if (Indices32.Length != 0)
			{
				return (int)Indices32[i];
			}
			if (Indices16.Length != 0)
			{
				return Indices16[i];
			}
			throw new IndexOutOfRangeException();
		}
	}

	public FRawStaticIndexBuffer()
	{
		Indices16 = Array.Empty<ushort>();
		Indices32 = Array.Empty<uint>();
	}

	public FRawStaticIndexBuffer(FArchive Ar)
		: this()
	{
		if (Ar.Ver < EUnrealEngineObjectUE4Version.SUPPORT_32BIT_STATIC_MESH_INDICES)
		{
			Indices16 = Ar.ReadBulkArray<ushort>();
			return;
		}
		bool flag = Ar.ReadBoolean();
		byte[] data = Ar.ReadBulkArray<byte>();
		FByteArchive fByteArchive = new FByteArchive("IndicesReader", data, Ar.Versions);
		if (Ar.Versions["RawIndexBuffer.HasShouldExpandTo32Bit"])
		{
			Ar.ReadBoolean();
		}
		if (fByteArchive.Length == 0L)
		{
			fByteArchive.Dispose();
			return;
		}
		if (flag)
		{
			int length = (int)fByteArchive.Length / 4;
			Indices32 = fByteArchive.ReadArray<uint>(length);
		}
		else
		{
			int length2 = (int)fByteArchive.Length / 2;
			Indices16 = fByteArchive.ReadArray<ushort>(length2);
		}
		fByteArchive.Dispose();
	}
}
