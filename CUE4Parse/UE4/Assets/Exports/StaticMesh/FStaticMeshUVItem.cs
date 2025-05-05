using CUE4Parse.UE4.Objects.Meshes;
using CUE4Parse.UE4.Objects.RenderCore;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

[JsonConverter(typeof(FStaticMeshUVItemConverter))]
public class FStaticMeshUVItem
{
	public readonly FPackedNormal[] Normal;

	public readonly FMeshUVFloat[] UV;

	public FStaticMeshUVItem(FArchive Ar, bool useHighPrecisionTangents, int numStaticUVSets, bool useStaticFloatUVs)
	{
		Normal = SerializeTangents(Ar, useHighPrecisionTangents);
		UV = SerializeTexcoords(Ar, numStaticUVSets, useStaticFloatUVs);
	}

	public FStaticMeshUVItem(FPackedNormal[] normal, FMeshUVFloat[] uv)
	{
		Normal = normal;
		UV = uv;
	}

	public static FPackedNormal[] SerializeTangents(FArchive Ar, bool useHighPrecisionTangents)
	{
		if (useHighPrecisionTangents)
		{
			return new FPackedNormal[3]
			{
				(FPackedNormal)new FPackedRGBA16N(Ar),
				new FPackedNormal(0u),
				(FPackedNormal)new FPackedRGBA16N(Ar)
			};
		}
		return new FPackedNormal[3]
		{
			new FPackedNormal(Ar),
			new FPackedNormal(0u),
			new FPackedNormal(Ar)
		};
	}

	public static FMeshUVFloat[] SerializeTexcoords(FArchive Ar, int numStaticUVSets, bool useStaticFloatUVs)
	{
		FMeshUVFloat[] array = new FMeshUVFloat[numStaticUVSets];
		if (useStaticFloatUVs)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Ar.Read<FMeshUVFloat>();
			}
		}
		else
		{
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = (FMeshUVFloat)Ar.Read<FMeshUVHalf>();
			}
		}
		return array;
	}
}
