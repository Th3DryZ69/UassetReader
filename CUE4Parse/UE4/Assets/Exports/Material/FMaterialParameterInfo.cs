using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Material;

[StructFallback]
[JsonConverter(typeof(FMaterialParameterInfoConverter))]
public class FMaterialParameterInfo
{
	public FName Name;

	public EMaterialParameterAssociation Association;

	public int Index;

	public FMaterialParameterInfo(FStructFallback fallback)
	{
		Name = fallback.GetOrDefault<FName>("Name");
		Association = fallback.GetOrDefault("Association", EMaterialParameterAssociation.LayerParameter);
		Index = fallback.GetOrDefault("Index", 0);
	}

	public FMaterialParameterInfo(FArchive Ar)
	{
		Name = Ar.ReadFName();
		Association = Ar.Read<EMaterialParameterAssociation>();
		Index = Ar.Read<int>();
	}

	public FMaterialParameterInfo()
	{
		Name = default(FName);
		Association = EMaterialParameterAssociation.LayerParameter;
		Index = 0;
	}
}
