using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Engine;

[StructFallback]
public class FExpressionInput : IUStruct
{
	public readonly FPackageIndex Expression;

	public readonly int OutputIndex;

	public readonly FName InputName;

	public readonly int Mask;

	public readonly int MaskR;

	public readonly int MaskG;

	public readonly int MaskB;

	public readonly int MaskA;

	public readonly FName ExpressionName;

	public FExpressionInput()
	{
	}

	public FExpressionInput(FStructFallback fallback)
	{
		Expression = fallback.GetOrDefault("Expression", new FPackageIndex());
		OutputIndex = fallback.GetOrDefault("OutputIndex", 0);
		InputName = fallback.GetOrDefault<FName>("InputName");
		Mask = fallback.GetOrDefault("Mask", 0);
		MaskR = fallback.GetOrDefault("MaskR", 0);
		MaskG = fallback.GetOrDefault("MaskG", 0);
		MaskB = fallback.GetOrDefault("MaskB", 0);
		MaskA = fallback.GetOrDefault("MaskA", 0);
		ExpressionName = fallback.GetOrDefault<FName>("ExpressionName");
	}

	public FExpressionInput(FAssetArchive Ar)
	{
		if (FCoreObjectVersion.Get(Ar) < FCoreObjectVersion.Type.MaterialInputNativeSerialize)
		{
			FExpressionInput fExpressionInput = new FExpressionInput(new FStructFallback(Ar, "ExpressionInput"));
			Expression = fExpressionInput.Expression;
			OutputIndex = fExpressionInput.OutputIndex;
			InputName = fExpressionInput.InputName;
			Mask = fExpressionInput.Mask;
			MaskR = fExpressionInput.MaskR;
			MaskG = fExpressionInput.MaskG;
			MaskB = fExpressionInput.MaskB;
			MaskA = fExpressionInput.MaskA;
			ExpressionName = fExpressionInput.ExpressionName;
			return;
		}
		if ((Ar != null && Ar.Game < EGame.GAME_UE5_1 && !Ar.IsFilterEditorOnly) || Ar.Game >= EGame.GAME_UE5_1)
		{
			Expression = new FPackageIndex(Ar);
		}
		OutputIndex = Ar.Read<int>();
		InputName = ((FFrameworkObjectVersion.Get(Ar) >= FFrameworkObjectVersion.Type.PinsStoreFName) ? Ar.ReadFName() : new FName(Ar.ReadFString()));
		Mask = Ar.Read<int>();
		MaskR = Ar.Read<int>();
		MaskG = Ar.Read<int>();
		MaskB = Ar.Read<int>();
		MaskA = Ar.Read<int>();
		ExpressionName = ((Ar != null && Ar.Game <= EGame.GAME_UE5_1 && Ar.IsFilterEditorOnly) ? Ar.ReadFName() : ((FName)(Expression ?? new FPackageIndex()).Name.SubstringAfterLast('/')));
	}
}
