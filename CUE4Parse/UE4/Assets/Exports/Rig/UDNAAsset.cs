using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Rig;

public class UDNAAsset : UObject
{
	public string DNAFileName { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		DNAFileName = GetOrDefault<string>("DNAFileName");
		FDNAAssetCustomVersion.Get(Ar);
		_ = 0;
	}
}
