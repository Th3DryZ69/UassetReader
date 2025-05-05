using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine;

[StructFallback]
public class FMaterialInputVector : FExpressionInput
{
	public bool UseConstant { get; protected set; }

	public FVector Constant { get; protected set; }

	public FMaterialInputVector()
	{
		UseConstant = false;
		Constant = FVector.ZeroVector;
	}

	public FMaterialInputVector(FStructFallback fallback)
	{
		UseConstant = fallback.GetOrDefault("UseConstant", defaultValue: false);
		Constant = fallback.GetOrDefault("Constant", FVector.ZeroVector);
	}

	public FMaterialInputVector(FAssetArchive Ar)
		: base(Ar)
	{
		if (FCoreObjectVersion.Get(Ar) < FCoreObjectVersion.Type.MaterialInputNativeSerialize)
		{
			FMaterialInputVector fMaterialInputVector = new FMaterialInputVector(new FStructFallback(Ar, "MaterialMaterialInput"));
			UseConstant = fMaterialInputVector.UseConstant;
			Constant = fMaterialInputVector.Constant;
		}
		else
		{
			UseConstant = Ar.ReadBoolean();
			Constant = Ar.Read<FVector>();
		}
	}
}
