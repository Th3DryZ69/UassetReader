namespace CUE4Parse.UE4.Assets.Objects.Unversioned;

public readonly struct FFragment
{
	public const uint SkipMax = 127u;

	public const uint ValueMax = 127u;

	public const uint SkipNumMask = 127u;

	public const uint HasZeroMask = 128u;

	public const int ValueNumShift = 9;

	public const uint IsLastMask = 256u;

	public readonly byte SkipNum;

	public readonly bool HasAnyZeroes;

	public readonly byte ValueNum;

	public readonly bool IsLast;

	public FFragment(ushort packed)
	{
		SkipNum = (byte)(packed & 0x7F);
		HasAnyZeroes = (packed & 0x80) != 0;
		ValueNum = (byte)(packed >> 9);
		IsLast = (packed & 0x100) != 0;
	}
}
