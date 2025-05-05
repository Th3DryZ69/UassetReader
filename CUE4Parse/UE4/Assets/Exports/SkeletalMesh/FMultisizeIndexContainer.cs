using System;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FMultisizeIndexContainer
{
	public ushort[] Indices16;

	public uint[] Indices32;

	public FMultisizeIndexContainer()
	{
		Indices16 = Array.Empty<ushort>();
		Indices32 = Array.Empty<uint>();
	}

	public FMultisizeIndexContainer(FArchive Ar)
		: this()
	{
		if (Ar.Ver < EUnrealEngineObjectUE4Version.KEEP_SKEL_MESH_INDEX_DATA)
		{
			Ar.Position += 4L;
		}
		if (Ar.Read<byte>() == 2)
		{
			Indices16 = Ar.ReadBulkArray<ushort>();
		}
		else
		{
			Indices32 = Ar.ReadBulkArray<uint>();
		}
	}
}
