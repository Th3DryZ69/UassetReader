using System;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FSkinWeightVertexBuffer
{
	private const int _NUM_INFLUENCES_UE4 = 4;

	public readonly FSkinWeightInfo[] Weights;

	public FSkinWeightVertexBuffer(FArchive Ar, bool numSkelCondition)
	{
		bool flag = FAnimObjectVersion.Get(Ar) >= FAnimObjectVersion.Type.UnlimitedBoneInfluences;
		FStripDataFlags fStripDataFlags = Ar.Read<FStripDataFlags>();
		bool flag2 = false;
		bool bUse16BitBoneIndex = false;
		bool bExtraBoneInfluences;
		uint num;
		if (!Ar.Versions["SkeletalMesh.UseNewCookedFormat"])
		{
			bExtraBoneInfluences = Ar.ReadBoolean();
			num = Ar.Read<uint>();
			uint num2 = (bExtraBoneInfluences ? 8u : 4u);
		}
		else if (!flag)
		{
			bExtraBoneInfluences = Ar.ReadBoolean();
			if (FSkeletalMeshCustomVersion.Get(Ar) >= FSkeletalMeshCustomVersion.Type.SplitModelAndRenderData)
			{
				Ar.Position += 4L;
			}
			num = Ar.Read<uint>();
			uint num2 = (bExtraBoneInfluences ? 8u : 4u);
			flag2 = false;
		}
		else
		{
			flag2 = Ar.ReadBoolean();
			uint num2 = Ar.Read<uint>();
			Ar.Read<uint>();
			num = Ar.Read<uint>();
			bExtraBoneInfluences = num2 > 4;
			if (FAnimObjectVersion.Get(Ar) >= FAnimObjectVersion.Type.IncreaseBoneIndexLimitPerChunk)
			{
				bUse16BitBoneIndex = Ar.ReadBoolean();
			}
		}
		byte[] array = Array.Empty<byte>();
		if (!fStripDataFlags.IsDataStrippedForServer())
		{
			if (!flag)
			{
				Weights = Ar.ReadBulkArray(() => new FSkinWeightInfo(Ar, bExtraBoneInfluences, bUse16BitBoneIndex));
			}
			else
			{
				array = Ar.ReadBulkArray<byte>();
			}
		}
		else
		{
			bExtraBoneInfluences = numSkelCondition;
		}
		if (!flag)
		{
			return;
		}
		uint[] array2 = Array.Empty<uint>();
		FStripDataFlags fStripDataFlags2 = Ar.Read<FStripDataFlags>();
		Ar.Read<int>();
		if (!fStripDataFlags2.IsDataStrippedForServer())
		{
			array2 = Ar.ReadBulkArray<uint>();
		}
		if (array.Length == 0)
		{
			return;
		}
		using FByteArchive fByteArchive = new FByteArchive("WeightsReader", array, Ar.Versions);
		Weights = new FSkinWeightInfo[num];
		if (flag2)
		{
			if (array2.Length != num)
			{
				throw new ParserException($"LookupData NumVertices={array2.Length} != NumVertices={num}");
			}
			for (int num3 = 0; num3 < Weights.Length; num3++)
			{
				fByteArchive.Position = array2[num3] >> 8;
				Weights[num3] = new FSkinWeightInfo(fByteArchive, bExtraBoneInfluences, bUse16BitBoneIndex, (byte)array2[num3]);
			}
		}
		else
		{
			for (int num4 = 0; num4 < Weights.Length; num4++)
			{
				Weights[num4] = new FSkinWeightInfo(fByteArchive, bExtraBoneInfluences, bUse16BitBoneIndex);
			}
		}
	}

	public static int MetadataSize(FArchive Ar)
	{
		int num = 0;
		bool flag = FAnimObjectVersion.Get(Ar) >= FAnimObjectVersion.Type.UnlimitedBoneInfluences;
		if (!Ar.Versions["SkeletalMesh.UseNewCookedFormat"])
		{
			num = 8;
		}
		else if (!flag)
		{
			num = 12;
		}
		else
		{
			num = 16;
			if (FAnimObjectVersion.Get(Ar) >= FAnimObjectVersion.Type.IncreaseBoneIndexLimitPerChunk)
			{
				num += 4;
			}
		}
		if (flag)
		{
			num += 4;
		}
		return num;
	}
}
