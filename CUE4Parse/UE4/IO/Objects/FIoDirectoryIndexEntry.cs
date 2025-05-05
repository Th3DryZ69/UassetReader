namespace CUE4Parse.UE4.IO.Objects;

public readonly struct FIoDirectoryIndexEntry
{
	public readonly uint Name;

	public readonly uint FirstChildEntry;

	public readonly uint NextSiblingEntry;

	public readonly uint FirstFileEntry;
}
