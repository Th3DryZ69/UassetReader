using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public abstract class FShaderMapBase
{
	public FShaderMapContent Content;

	public FSHAHash ResourceHash;

	public FShaderMapResourceCode Code;

	public void Deserialize(FMaterialResourceProxyReader Ar)
	{
		bool flag = Ar.Versions["ShaderMap.UseNewCookedFormat"];
		FMemoryImageResult fMemoryImageResult = new FMemoryImageResult(new FShaderMapPointerTable());
		fMemoryImageResult.LoadFromArchive(Ar);
		Content = ReadContent(new FMemoryImageArchive(new FByteArchive("FShaderMapContent", fMemoryImageResult.FrozenObject, Ar.Versions))
		{
			Names = fMemoryImageResult.GetNames()
		});
		bool num = Ar.ReadBoolean();
		if (flag)
		{
			if (Ar.Game < EGame.GAME_UE5_2)
			{
				Ar.Read<EShaderPlatform>().ToString();
			}
			else
			{
				Ar.ReadFString();
			}
		}
		if (num)
		{
			ResourceHash = new FSHAHash(Ar);
		}
		else
		{
			Code = new FShaderMapResourceCode(Ar);
		}
	}

	protected abstract FShaderMapContent ReadContent(FMemoryImageArchive Ar);
}
