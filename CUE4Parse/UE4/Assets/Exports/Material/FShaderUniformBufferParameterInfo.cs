using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FShaderUniformBufferParameterInfo : FShaderParameterInfo
{
	public FShaderUniformBufferParameterInfo(FMemoryImageArchive Ar)
	{
		BaseIndex = Ar.Read<ushort>();
	}
}
