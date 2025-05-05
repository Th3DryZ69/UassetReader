using System;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.Meshes;
using CUE4Parse.UE4.Objects.RenderCore;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

[JsonConverter(typeof(FStaticMeshVertexBufferConverter))]
public class FStaticMeshVertexBuffer
{
	public readonly int NumTexCoords;

	public readonly int Strides;

	public readonly int NumVertices;

	public readonly bool UseFullPrecisionUVs;

	public readonly bool UseHighPrecisionTangentBasis;

	public readonly FStaticMeshUVItem[] UV;

	public FStaticMeshVertexBuffer(FArchive Ar)
	{
		FStaticMeshVertexBuffer fStaticMeshVertexBuffer = this;
		FStripDataFlags fStripDataFlags = new FStripDataFlags(Ar, FPackageFileVersion.CreateUE4Version(EUnrealEngineObjectUE4Version.STATIC_SKELETAL_MESH_SERIALIZATION_FIX));
		NumTexCoords = Ar.Read<int>();
		Strides = ((Ar.Game < EGame.GAME_UE4_19) ? Ar.Read<int>() : (-1));
		NumVertices = Ar.Read<int>();
		UseFullPrecisionUVs = Ar.ReadBoolean();
		UseHighPrecisionTangentBasis = Ar.Game >= EGame.GAME_UE4_12 && Ar.ReadBoolean();
		if (!fStripDataFlags.IsDataStrippedForServer())
		{
			if (Ar.Game < EGame.GAME_UE4_19)
			{
				UV = Ar.ReadBulkArray(() => new FStaticMeshUVItem(Ar, fStaticMeshVertexBuffer.UseHighPrecisionTangentBasis, fStaticMeshVertexBuffer.NumTexCoords, fStaticMeshVertexBuffer.UseFullPrecisionUVs));
				return;
			}
			FPackedNormal[][] array = Array.Empty<FPackedNormal[]>();
			EGame game = Ar.Game;
			int num;
			int num2;
			long position;
			if ((game != EGame.GAME_StarWarsJediFallenOrder && game != EGame.GAME_StarWarsJediSurvivor) || !Ar.ReadBoolean())
			{
				num = Ar.Read<int>();
				num2 = Ar.Read<int>();
				position = Ar.Position;
				if (num2 != NumVertices)
				{
					throw new ParserException($"NumVertices={num2} != NumVertices={NumVertices}");
				}
				array = Ar.ReadArray(NumVertices, () => FStaticMeshUVItem.SerializeTangents(Ar, fStaticMeshVertexBuffer.UseHighPrecisionTangentBasis));
				if (Ar.Position - position != num2 * num)
				{
					throw new ParserException($"Read incorrect amount of tangent bytes, at {Ar.Position}, should be: {position + num * num2} behind: {position + num * num2 - Ar.Position}");
				}
			}
			num = Ar.Read<int>();
			num2 = Ar.Read<int>();
			position = Ar.Position;
			if (num2 != NumVertices * NumTexCoords)
			{
				throw new ParserException($"NumVertices={num2} != {NumVertices * NumTexCoords}");
			}
			FMeshUVFloat[][] array2 = Ar.ReadArray(NumVertices, () => FStaticMeshUVItem.SerializeTexcoords(Ar, fStaticMeshVertexBuffer.NumTexCoords, fStaticMeshVertexBuffer.UseFullPrecisionUVs));
			if (Ar.Position - position != num2 * num)
			{
				throw new ParserException($"Read incorrect amount of Texture Coordinate bytes, at {Ar.Position}, should be: {position + num * num2} behind: {position + num * num2 - Ar.Position}");
			}
			UV = new FStaticMeshUVItem[NumVertices];
			for (int num3 = 0; num3 < NumVertices; num3++)
			{
				game = Ar.Game;
				if ((game == EGame.GAME_StarWarsJediFallenOrder || game == EGame.GAME_StarWarsJediSurvivor) && array.Length == 0)
				{
					UV[num3] = new FStaticMeshUVItem(new FPackedNormal[3]
					{
						new FPackedNormal(0u),
						new FPackedNormal(0u),
						new FPackedNormal(0u)
					}, array2[num3]);
				}
				else
				{
					UV[num3] = new FStaticMeshUVItem(array[num3], array2[num3]);
				}
			}
		}
		else
		{
			UV = Array.Empty<FStaticMeshUVItem>();
		}
	}
}
