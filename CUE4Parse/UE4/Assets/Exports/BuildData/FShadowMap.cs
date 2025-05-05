using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class FShadowMap
{
	public readonly FGuid[] LightGuids;

	public FShadowMap(FAssetArchive Ar)
	{
		LightGuids = Ar.ReadArray<FGuid>();
	}
}
