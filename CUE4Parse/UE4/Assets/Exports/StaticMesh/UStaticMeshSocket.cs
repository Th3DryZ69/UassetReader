using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class UStaticMeshSocket : UObject
{
	public FName SocketName { get; private set; }

	public FVector RelativeLocation { get; private set; }

	public FRotator RelativeRotation { get; private set; }

	public FVector RelativeScale { get; private set; }

	public string Tag { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		SocketName = GetOrDefault<FName>("SocketName");
		RelativeLocation = GetOrDefault("RelativeLocation", FVector.ZeroVector);
		RelativeRotation = GetOrDefault("RelativeRotation", FRotator.ZeroRotator);
		RelativeScale = GetOrDefault("RelativeScale", FVector.OneVector);
		Tag = GetOrDefault<string>("Tag");
	}
}
