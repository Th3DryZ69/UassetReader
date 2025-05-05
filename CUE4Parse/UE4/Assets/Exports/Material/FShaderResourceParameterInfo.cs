using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FShaderResourceParameterInfo : FShaderParameterInfo
{
	public byte BufferIndex;

	public byte Type;

	public FShaderResourceParameterInfo(FMemoryImageArchive Ar)
	{
		BaseIndex = Ar.Read<ushort>();
		BufferIndex = Ar.Read<byte>();
		Type = Ar.Read<byte>();
	}
}
