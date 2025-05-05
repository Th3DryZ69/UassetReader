using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(FInstancedStructConverter))]
public class FInstancedStruct : IUStruct
{
	public readonly FStructFallback NonConstStruct;

	public FInstancedStruct(FAssetArchive Ar)
	{
		Ar.Read<byte>();
		FPackageIndex fPackageIndex = new FPackageIndex(Ar);
		int num = Ar.Read<int>();
		if (fPackageIndex.IsNull && num > 0)
		{
			Ar.Position += num;
		}
		else
		{
			NonConstStruct = new FStructFallback(Ar, fPackageIndex.Name);
		}
	}
}
