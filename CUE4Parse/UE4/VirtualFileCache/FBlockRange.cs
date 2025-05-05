namespace CUE4Parse.UE4.VirtualFileCache;

public readonly struct FBlockRange
{
	public readonly int StartIndex;

	public readonly int NumBlocks;

	public override string ToString()
	{
		return $"Start: {StartIndex} | Blocks: x{NumBlocks}";
	}
}
