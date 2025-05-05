using System;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FSkinWeightInfo
{
	private const int NUM_INFLUENCES_UE4 = 4;

	private const int MAX_TOTAL_INFLUENCES_UE4 = 8;

	public readonly ushort[] BoneIndex;

	public readonly byte[] BoneWeight;

	public FSkinWeightInfo()
	{
		BoneIndex = new ushort[4];
		BoneWeight = new byte[4];
	}

	public FSkinWeightInfo(FArchive Ar, bool bExtraBoneInfluences, bool bUse16BitBoneIndex = false, int length = 0)
	{
		int length2 = (bExtraBoneInfluences ? 8 : 4);
		if (length > 0)
		{
			length2 = length;
		}
		BoneIndex = (bUse16BitBoneIndex ? Ar.ReadArray<ushort>(length2) : Ar.ReadArray(length2, (Func<ushort>)(() => Ar.Read<byte>())));
		BoneWeight = Ar.ReadArray<byte>(length2);
	}
}
