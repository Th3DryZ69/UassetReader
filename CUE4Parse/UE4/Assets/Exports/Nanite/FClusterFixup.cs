using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Nanite;

public class FClusterFixup
{
	public const int NANITE_MAX_CLUSTERS_PER_PAGE_BITS = 8;

	public const int NANITE_MAX_CLUSTERS_PER_PAGE = 256;

	public uint PageIndex;

	public uint ClusterIndex;

	public uint PageDependencyStart;

	public uint PageDependencyNum;

	public FClusterFixup(FArchive Ar)
	{
		uint num = Ar.Read<uint>();
		PageIndex = num >> 8;
		ClusterIndex = num & 0xFF;
		uint num2 = Ar.Read<uint>();
		PageDependencyStart = num2 >> 3;
		PageDependencyNum = num2 & 7;
	}
}
