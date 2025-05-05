using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Nanite;

public class FHierarchyFixup
{
	public const int NANITE_MAX_HIERACHY_CHILDREN_BITS = 6;

	public const int NANITE_MAX_GROUP_PARTS_BITS = 3;

	public const int NANITE_MAX_HIERACHY_CHILDREN = 64;

	public const int NANITE_MAX_GROUP_PARTS_MASK = 7;

	public uint PageIndex;

	public uint NodeIndex;

	public uint ChildIndex;

	public uint ClusterGroupPartStartIndex;

	public uint PageDependencyStart;

	public uint PageDependencyNum;

	public FHierarchyFixup(FArchive Ar)
	{
		PageIndex = Ar.Read<uint>();
		uint num = Ar.Read<uint>();
		NodeIndex = num >> 6;
		ChildIndex = num & 0x3F;
		ClusterGroupPartStartIndex = Ar.Read<uint>();
		uint num2 = Ar.Read<uint>();
		PageDependencyStart = num2 >> 3;
		PageDependencyNum = num2 & 7;
	}
}
