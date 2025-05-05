using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.Assets.Exports.Material;

[StructLayout(LayoutKind.Sequential, Size = 4)]
public struct FRHIUniformBufferResource
{
	public ushort MemberOffset;

	public EUniformBufferBaseType MemberType;
}
