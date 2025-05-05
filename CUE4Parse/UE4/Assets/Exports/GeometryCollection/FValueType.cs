using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.GeometryCollection;

public readonly struct FValueType : IUStruct
{
	public readonly EManagedArrayType ArrayType;

	public readonly FName GroupIndexDependency;

	public readonly bool Saved;

	public FValueType(FAssetArchive Ar, int version)
	{
		int num = Ar.Read<int>();
		ArrayType = (EManagedArrayType)num;
		if (version < 4)
		{
			Ar.Read<int>();
		}
		if (version >= 2)
		{
			GroupIndexDependency = Ar.ReadFName();
			Saved = Ar.ReadBoolean();
		}
		else
		{
			GroupIndexDependency = default(FName);
			Saved = false;
		}
	}
}
