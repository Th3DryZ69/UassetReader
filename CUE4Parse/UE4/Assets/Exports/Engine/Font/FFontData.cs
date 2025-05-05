using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Engine.Font;

[JsonConverter(typeof(FFontDataConverter))]
public class FFontData : IUStruct
{
	public FPackageIndex? LocalFontFaceAsset;

	public string? FontFilename;

	public EFontHinting Hinting;

	public EFontLoadingPolicy LoadingPolicy;

	public int SubFaceIndex;

	public FFontData(FAssetArchive Ar)
	{
		if (FEditorObjectVersion.Get(Ar) >= FEditorObjectVersion.Type.AddedFontFaceAssets && Ar.ReadBoolean())
		{
			LocalFontFaceAsset = new FPackageIndex(Ar);
			if (LocalFontFaceAsset == null)
			{
				FontFilename = Ar.ReadFString();
				Hinting = Ar.Read<EFontHinting>();
				LoadingPolicy = Ar.Read<EFontLoadingPolicy>();
			}
			SubFaceIndex = Ar.Read<int>();
		}
	}
}
