using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Objects.Engine;

public class UMaterialExpression : CUE4Parse.UE4.Assets.Exports.UObject
{
	public FPackageIndex Material { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		Material = GetOrDefault<FPackageIndex>("Material");
	}
}
