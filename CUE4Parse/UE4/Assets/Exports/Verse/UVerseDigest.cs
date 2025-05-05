using System.Text;
using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Verse;

public class UVerseDigest : UObject
{
	public string ProjectName { get; private set; }

	public EVerseDigestVariant Variant { get; private set; }

	public string ReadableCode { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		ProjectName = GetOrDefault<string>("ProjectName");
		Variant = GetOrDefault("Variant", EVerseDigestVariant.PublicOnly);
		ReadableCode = Encoding.UTF8.GetString(GetOrDefault<byte[]>("DigestCode"));
	}
}
