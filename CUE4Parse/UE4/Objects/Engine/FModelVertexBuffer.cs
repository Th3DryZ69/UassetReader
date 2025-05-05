using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine;

public class FModelVertexBuffer : IUStruct
{
	public readonly FModelVertex[] Vertices;

	public FModelVertexBuffer(FArchive Ar)
	{
		if (FRenderingObjectVersion.Get(Ar) < FRenderingObjectVersion.Type.IncreaseNormalPrecision)
		{
			FDeprecatedModelVertex[] array = Ar.ReadBulkArray<FDeprecatedModelVertex>();
			Vertices = new FModelVertex[array.Length];
			for (int i = 0; i < Vertices.Length; i++)
			{
				Vertices[i] = array[i];
			}
		}
		else
		{
			Vertices = Ar.ReadArray(() => new FModelVertex(Ar));
		}
	}
}
