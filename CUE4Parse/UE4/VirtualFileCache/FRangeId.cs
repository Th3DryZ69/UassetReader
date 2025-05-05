using System.IO;

namespace CUE4Parse.UE4.VirtualFileCache;

public readonly struct FRangeId
{
	public readonly int FileId;

	public readonly FBlockRange Range;

	public string GetFileName()
	{
		return $"vfc_{FileId}.data";
	}

	public string GetPersistentDownloadPath()
	{
		return Path.Combine("VFC", GetFileName());
	}

	public override string ToString()
	{
		return $"{GetFileName()}: {Range}";
	}
}
