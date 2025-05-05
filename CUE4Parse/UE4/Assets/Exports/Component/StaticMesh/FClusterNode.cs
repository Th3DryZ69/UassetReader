using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;

public class FClusterNode : FClusterNode_DEPRECATED
{
	public FVector MinInstanceScale;

	public FVector MaxInstanceScale;

	public FClusterNode(FArchive Ar)
		: base(Ar)
	{
		MinInstanceScale = Ar.Read<FVector>();
		MaxInstanceScale = Ar.Read<FVector>();
	}
}
