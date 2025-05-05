using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FShaderLooseParameterBufferInfo
{
	public ushort BaseIndex;

	public ushort Size;

	public FShaderLooseParameterInfo[] Parameters;

	public FShaderLooseParameterBufferInfo(FMemoryImageArchive Ar)
	{
		BaseIndex = Ar.Read<ushort>();
		Size = Ar.Read<ushort>();
		Ar.Position += 4L;
		Parameters = Ar.ReadArray<FShaderLooseParameterInfo>();
	}
}
