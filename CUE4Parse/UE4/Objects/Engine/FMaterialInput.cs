using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine;

[StructFallback]
public class FMaterialInput<T> : FExpressionInput where T : struct
{
	public bool UseConstant { get; protected set; }

	public T Constant { get; protected set; }

	public FMaterialInput()
	{
		UseConstant = false;
		Constant = new T();
	}

	public FMaterialInput(FStructFallback fallback)
		: base(fallback)
	{
		UseConstant = fallback.GetOrDefault("UseConstant", defaultValue: false);
		Constant = fallback.GetOrDefault("Constant", new T());
	}

	public FMaterialInput(FAssetArchive Ar)
		: base(Ar)
	{
		if (FCoreObjectVersion.Get(Ar) < FCoreObjectVersion.Type.MaterialInputNativeSerialize)
		{
			FMaterialInput<T> fMaterialInput = new FMaterialInput<T>(new FStructFallback(Ar, "MaterialInput"));
			UseConstant = fMaterialInput.UseConstant;
			Constant = fMaterialInput.Constant;
		}
		else
		{
			UseConstant = Ar.ReadBoolean();
			Constant = Ar.Read<T>();
		}
	}
}
