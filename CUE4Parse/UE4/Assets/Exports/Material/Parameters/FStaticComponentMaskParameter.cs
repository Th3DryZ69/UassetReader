using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material.Parameters;

[StructFallback]
public class FStaticComponentMaskParameter : FStaticParameterBase
{
	public bool R;

	public bool G;

	public bool B;

	public bool A;

	public FStaticComponentMaskParameter(FStructFallback fallback)
		: base(fallback)
	{
		R = fallback.GetOrDefault("R", defaultValue: false);
		G = fallback.GetOrDefault("G", defaultValue: false);
		B = fallback.GetOrDefault("B", defaultValue: false);
		A = fallback.GetOrDefault("A", defaultValue: false);
	}

	public FStaticComponentMaskParameter(FArchive Ar)
		: base(Ar)
	{
		R = Ar.ReadBoolean();
		G = Ar.ReadBoolean();
		B = Ar.ReadBoolean();
		A = Ar.ReadBoolean();
		bOverride = Ar.ReadBoolean();
		ExpressionGuid = Ar.Read<FGuid>();
	}
}
