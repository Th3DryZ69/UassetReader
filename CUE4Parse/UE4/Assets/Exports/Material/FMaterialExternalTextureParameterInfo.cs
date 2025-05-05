using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialExternalTextureParameterInfo
{
	public FName ParameterName;

	public FGuid ExternalTextureGuid;

	public int SourceTextureIndex;

	public FMaterialExternalTextureParameterInfo(FMemoryImageArchive Ar)
	{
		ParameterName = Ar.ReadFName();
		ExternalTextureGuid = Ar.Read<FGuid>();
		SourceTextureIndex = Ar.Read<int>();
	}
}
