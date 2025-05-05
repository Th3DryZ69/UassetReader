using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;

public class FInstancedStaticMeshInstanceData
{
	private readonly FMatrix Transform;

	public readonly FTransform TransformData = new FTransform();

	public FInstancedStaticMeshInstanceData(FArchive Ar)
	{
		Transform = new FMatrix(Ar);
		if (Ar.Game == EGame.GAME_HogwartsLegacy)
		{
			Ar.SkipFixedArray(4);
		}
		TransformData.SetFromMatrix(Transform);
	}

	public override string ToString()
	{
		return TransformData.ToString();
	}
}
