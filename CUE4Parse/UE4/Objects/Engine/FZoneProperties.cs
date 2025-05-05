using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Objects.Engine;

public readonly struct FZoneProperties : IUStruct
{
	public readonly FPackageIndex ZoneActor;

	public readonly float LastRenderTime;

	public readonly FZoneSet Connectivity;

	public readonly FZoneSet Visibility;

	public FZoneProperties(FAssetArchive Ar)
	{
		ZoneActor = new FPackageIndex(Ar);
		Connectivity = Ar.Read<FZoneSet>();
		Visibility = Ar.Read<FZoneSet>();
		LastRenderTime = Ar.Read<float>();
	}
}
