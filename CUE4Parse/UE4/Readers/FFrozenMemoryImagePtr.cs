using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Readers;

public struct FFrozenMemoryImagePtr
{
	private const ulong TypeIndexMask = 16777214uL;

	public readonly ulong _packed;

	public readonly bool IsFrozen;

	public readonly long OffsetFromThis;

	public readonly int TypeIndex;

	public FFrozenMemoryImagePtr(FMemoryImageArchive Ar)
	{
		TypeIndex = -1;
		_packed = Ar.Read<ulong>();
		IsFrozen = (_packed & 1) != 0;
		if (Ar.Game >= EGame.GAME_UE5_0)
		{
			OffsetFromThis = (long)_packed >> 24;
			TypeIndex = (int)((_packed & 0xFFFFFE) >> 1) - 1;
		}
		else
		{
			OffsetFromThis = (long)_packed >> 1;
		}
	}
}
