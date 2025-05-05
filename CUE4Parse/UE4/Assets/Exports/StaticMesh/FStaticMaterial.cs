using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Meshes;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

[StructFallback]
public class FStaticMaterial
{
	public ResolvedObject? MaterialInterface;

	public FName MaterialSlotName;

	public FName ImportedMaterialSlotName;

	public FMeshUVChannelInfo? UVChannelData;

	public FStaticMaterial(FAssetArchive Ar)
	{
		MaterialInterface = new FPackageIndex(Ar).ResolvedObject;
		MaterialSlotName = Ar.ReadFName();
		if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.TextureStreamingMeshUVChannelData)
		{
			UVChannelData = new FMeshUVChannelInfo(Ar);
		}
	}

	public FStaticMaterial(FStructFallback fallback)
	{
		MaterialInterface = fallback.GetOrDefault("MaterialInterface", new FPackageIndex().ResolvedObject);
		MaterialSlotName = fallback.GetOrDefault("MaterialSlotName", "None");
		UVChannelData = fallback.GetOrDefault<FMeshUVChannelInfo>("UVChannelData");
	}
}
