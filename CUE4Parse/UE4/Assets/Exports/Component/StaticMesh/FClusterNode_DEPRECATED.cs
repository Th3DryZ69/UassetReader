using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;

public class FClusterNode_DEPRECATED
{
	public FVector BoundMin;

	public int FirstChild;

	public FVector BoundMax;

	public int LastChild;

	public int FirstInstance;

	public int LastInstance;

	public FClusterNode_DEPRECATED(FArchive Ar)
	{
		BoundMin = Ar.Read<FVector>();
		FirstChild = Ar.Read<int>();
		BoundMax = Ar.Read<FVector>();
		LastChild = Ar.Read<int>();
		FirstInstance = Ar.Read<int>();
		LastInstance = Ar.Read<int>();
	}
}
