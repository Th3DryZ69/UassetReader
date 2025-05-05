using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FShaderParameterMapInfo
{
	public FShaderParameterInfo[] UniformBuffers;

	public FShaderParameterInfo[] TextureSamplers;

	public FShaderParameterInfo[] SRVs;

	public FShaderLooseParameterBufferInfo[] LooseParameterBuffers;

	public ulong Hash;

	public FShaderParameterMapInfo(FMemoryImageArchive Ar)
	{
		if (Ar.Game >= EGame.GAME_UE5_1)
		{
			FShaderParameterInfo[] uniformBuffers = Ar.ReadArray(() => new FShaderUniformBufferParameterInfo(Ar));
			UniformBuffers = uniformBuffers;
			uniformBuffers = Ar.ReadArray(() => new FShaderResourceParameterInfo(Ar));
			TextureSamplers = uniformBuffers;
			uniformBuffers = Ar.ReadArray(() => new FShaderResourceParameterInfo(Ar));
			SRVs = uniformBuffers;
		}
		else
		{
			UniformBuffers = Ar.ReadArray(() => new FShaderParameterInfo(Ar));
			TextureSamplers = Ar.ReadArray(() => new FShaderParameterInfo(Ar));
			SRVs = Ar.ReadArray(() => new FShaderParameterInfo(Ar));
		}
		LooseParameterBuffers = Ar.ReadArray(() => new FShaderLooseParameterBufferInfo(Ar));
		Hash = ((Ar.Game >= EGame.GAME_UE4_26) ? Ar.Read<ulong>() : 0);
	}
}
