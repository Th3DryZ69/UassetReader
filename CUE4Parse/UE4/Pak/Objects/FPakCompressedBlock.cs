namespace CUE4Parse.UE4.Pak.Objects;

public struct FPakCompressedBlock
{
	public long CompressedStart;

	public long CompressedEnd;

	public long Size => CompressedEnd - CompressedStart;
}
