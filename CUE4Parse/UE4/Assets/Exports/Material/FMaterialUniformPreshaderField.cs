using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.Assets.Exports.Material;

[StructLayout(LayoutKind.Sequential, Size = 12)]
public struct FMaterialUniformPreshaderField
{
	public uint BufferOffset;

	public uint ComponentIndex;

	public EShaderValueType Type;
}
