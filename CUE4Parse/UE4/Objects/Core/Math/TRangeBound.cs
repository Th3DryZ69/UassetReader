using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.Objects.Core.Math;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct TRangeBound<T> : IUStruct
{
	public readonly ERangeBoundTypes Type;

	public readonly T Value;

	public override string ToString()
	{
		T value = Value;
		return ((value != null) ? value.ToString() : null) ?? string.Empty;
	}
}
