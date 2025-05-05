using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine;

[StructFallback]
public class FMaterialInputVector2D : FExpressionInput
{
	public bool UseConstant { get; protected set; }

	public FVector2D Constant { get; protected set; }

	public FMaterialInputVector2D()
	{
		UseConstant = false;
		Constant = FVector2D.ZeroVector;
	}

	public FMaterialInputVector2D(FStructFallback fallback)
	{
		UseConstant = fallback.GetOrDefault("UseConstant", defaultValue: false);
		Constant = fallback.GetOrDefault("Constant", FVector2D.ZeroVector);
	}

	public FMaterialInputVector2D(FAssetArchive Ar)
		: base(Ar)
	{
		if (FCoreObjectVersion.Get(Ar) < FCoreObjectVersion.Type.MaterialInputNativeSerialize)
		{
			FMaterialInputVector2D fMaterialInputVector2D = new FMaterialInputVector2D(new FStructFallback(Ar, "MaterialInputVector2D"));
			UseConstant = fMaterialInputVector2D.UseConstant;
			Constant = fMaterialInputVector2D.Constant;
		}
		else
		{
			UseConstant = Ar.ReadBoolean();
			Constant = Ar.Read<FVector2D>();
		}
	}
}
