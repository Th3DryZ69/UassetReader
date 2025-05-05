using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Meshes;

[JsonConverter(typeof(FPositionVertexBufferConverter))]
public class FPositionVertexBuffer
{
	public readonly FVector[] Verts;

	public readonly int Stride;

	public readonly int NumVertices;

	public FPositionVertexBuffer(FArchive Ar)
	{
		Stride = Ar.Read<int>();
		NumVertices = Ar.Read<int>();
		if (Ar.Game == EGame.GAME_Valorant)
		{
			bool num = Ar.ReadBoolean();
			new FBoxSphereBounds(Ar);
			if (!num)
			{
				FVector4Half[] array = Ar.ReadBulkArray<FVector4Half>();
				Verts = new FVector[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					Verts[i] = array[i];
				}
				return;
			}
		}
		if (Ar.Game == EGame.GAME_Gollum)
		{
			Ar.Position += 25L;
		}
		Verts = Ar.ReadBulkArray<FVector>();
	}
}
