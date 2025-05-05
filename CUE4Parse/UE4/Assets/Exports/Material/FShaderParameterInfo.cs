using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FShaderParameterInfo
{
	public ushort BaseIndex;

	public ushort Size;

	public FShaderParameterInfo(FMemoryImageArchive Ar)
	{
		BaseIndex = Ar.Read<ushort>();
		Size = Ar.Read<ushort>();
	}

	public FShaderParameterInfo()
	{
	}
}
