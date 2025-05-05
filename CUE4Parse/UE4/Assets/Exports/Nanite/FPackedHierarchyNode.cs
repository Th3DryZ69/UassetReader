using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Nanite;

public class FPackedHierarchyNode
{
	public class FMisc0
	{
		public FVector BoxBoundsCenter;

		public float MinLODError;

		public float MaxParentLODError;

		public FMisc0(FArchive Ar)
		{
			BoxBoundsCenter = Ar.Read<FVector>();
			uint num = Ar.Read<uint>();
			MinLODError = num;
			MaxParentLODError = num >> 16;
		}
	}

	public struct FMisc1
	{
		public FVector BoxBoundsExtent;

		public uint ChildStartReference;

		public bool bLoaded => ChildStartReference != uint.MaxValue;
	}

	public class FMisc2
	{
		public const int NANITE_MAX_CLUSTERS_PER_GROUP_BITS = 9;

		public const int NANITE_MAX_RESOURCE_PAGES_BITS = 20;

		public uint NumChildren;

		public uint NumPages;

		public uint StartPageIndex;

		public bool bEnabled;

		public bool bLeaf;

		public FMisc2(FArchive Ar)
		{
			uint num = Ar.Read<uint>();
			NumChildren = FCluster.GetBits(num, 9, 0);
			NumPages = FCluster.GetBits(num, 3, 9);
			StartPageIndex = FCluster.GetBits(num, 20, 12);
			bEnabled = num != 0;
			bLeaf = num != uint.MaxValue;
		}
	}

	public const int NANITE_MAX_BVH_NODE_FANOUT_BITS = 2;

	public const int NANITE_MAX_BVH_NODE_FANOUT = 4;

	public FVector4[] LODBounds;

	public FMisc0[] Misc0;

	public FMisc1[] Misc1;

	public FMisc2[] Misc2;

	public FPackedHierarchyNode(FArchive Ar)
	{
		LODBounds = Ar.ReadArray<FVector4>(4);
		Misc0 = Ar.ReadArray(4, () => new FMisc0(Ar));
		Misc1 = Ar.ReadArray<FMisc1>(4);
		Misc2 = Ar.ReadArray(4, () => new FMisc2(Ar));
	}
}
