using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.GameTypes.FN.Assets.Exports;

public class FLevelStreamedDeleteActorRecord
{
	public FName ActorId;

	public FTransform Transform;

	public FSoftObjectPath ActorClass;

	public FSoftObjectPath OwningLevel;

	public FLevelStreamedDeleteActorRecord(FAssetArchive Ar)
	{
		ActorId = Ar.ReadFName();
		Transform = new FTransform(Ar);
		ActorClass = new FSoftObjectPath(Ar);
		OwningLevel = new FSoftObjectPath(Ar);
	}
}
