using System.Runtime.CompilerServices;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

public static class FPartialMapHandle
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FMapHandle MakeFullHandle(FStore store, ulong mapSize)
	{
		return new FMapHandle(mapSize >> 63 != 0, store, (ushort)(mapSize >> 32), (uint)mapSize);
	}
}
