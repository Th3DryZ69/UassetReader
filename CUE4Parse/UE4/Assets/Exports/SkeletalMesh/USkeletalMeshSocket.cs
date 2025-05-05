using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class USkeletalMeshSocket : UObject
{
	public FName SocketName { get; private set; }

	public FName BoneName { get; private set; }

	public FVector RelativeLocation { get; private set; }

	public FRotator RelativeRotation { get; private set; }

	public FVector RelativeScale { get; private set; }

	public bool bForceAlwaysAnimated { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		SocketName = GetOrDefault<FName>("SocketName");
		BoneName = GetOrDefault<FName>("BoneName");
		RelativeLocation = GetOrDefault("RelativeLocation", FVector.ZeroVector);
		RelativeRotation = GetOrDefault("RelativeRotation", FRotator.ZeroRotator);
		RelativeScale = GetOrDefault("RelativeScale", FVector.OneVector);
		bForceAlwaysAnimated = GetOrDefault("bForceAlwaysAnimated", defaultValue: false);
	}
}
