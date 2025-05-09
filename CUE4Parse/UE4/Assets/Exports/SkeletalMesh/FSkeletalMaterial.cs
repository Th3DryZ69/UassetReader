using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Meshes;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

[JsonConverter(typeof(FSkeletalMaterialConverter))]
public class FSkeletalMaterial
{
	public ResolvedObject? Material;

	public FName MaterialSlotName;

	public FName? ImportedMaterialSlotName;

	public FMeshUVChannelInfo? UVChannelData;

	public FSkeletalMaterial(FAssetArchive Ar)
	{
		Material = new FPackageIndex(Ar).ResolvedObject;
		if (FEditorObjectVersion.Get(Ar) >= FEditorObjectVersion.Type.RefactorMeshEditorMaterials)
		{
			MaterialSlotName = Ar.ReadFName();
			bool flag = !Ar.Owner.HasFlags(EPackageFlags.PKG_FilterEditorOnly);
			if (FCoreObjectVersion.Get(Ar) >= FCoreObjectVersion.Type.SkeletalMaterialEditorDataStripping)
			{
				flag = Ar.ReadBoolean();
			}
			if (flag)
			{
				ImportedMaterialSlotName = Ar.ReadFName();
			}
		}
		else
		{
			if (Ar.Ver >= EUnrealEngineObjectUE4Version.MOVE_SKELETALMESH_SHADOWCASTING)
			{
				Ar.Position += 4L;
			}
			if (FRecomputeTangentCustomVersion.Get(Ar) >= FRecomputeTangentCustomVersion.Type.RuntimeRecomputeTangent)
			{
				Ar.ReadBoolean();
			}
		}
		if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.TextureStreamingMeshUVChannelData)
		{
			UVChannelData = new FMeshUVChannelInfo(Ar);
		}
	}
}
