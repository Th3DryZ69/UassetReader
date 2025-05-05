using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class UMaterialExpressionParameter : UMaterialExpression
{
	public FName ParameterName { get; private set; }

	public FGuid ExpressionGUID { get; private set; }

	public FName Group { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		ParameterName = GetOrDefault<FName>("ParameterName");
		ExpressionGUID = GetOrDefault<FGuid>("ExpressionGUID");
		Group = GetOrDefault<FName>("Group");
	}
}
