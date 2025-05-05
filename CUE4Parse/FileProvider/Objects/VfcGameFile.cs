using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using CUE4Parse.Compression;
using CUE4Parse.UE4.Versions;
using CUE4Parse.UE4.VirtualFileCache;

namespace CUE4Parse.FileProvider.Objects;

public class VfcGameFile : VersionedGameFile
{
	public readonly FBlockFile[] BlockFiles;

	public readonly FRangeId[] Ranges;

	private readonly string _persistentDownloadDir;

	public override bool IsEncrypted => false;

	public override CompressionMethod CompressionMethod => CompressionMethod.None;

	public VfcGameFile(FBlockFile[] blockFiles, FDataReference dataReference, string persistentDownloadDir, string path, VersionContainer versions)
		: base(path, dataReference.TotalSize, versions)
	{
		BlockFiles = blockFiles;
		Ranges = dataReference.Ranges;
		_persistentDownloadDir = persistentDownloadDir;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override byte[] Read()
	{
		int num = 0;
		byte[] array = new byte[base.Size];
		FRangeId[] ranges = Ranges;
		for (int i = 0; i < ranges.Length; i++)
		{
			FRangeId r = ranges[i];
			int blockSize = BlockFiles.First((FBlockFile x) => x.FileId == r.FileId).BlockSize;
			using FileStream fileStream = new FileStream(System.IO.Path.Combine(_persistentDownloadDir, r.GetPersistentDownloadPath()), FileMode.Open, FileAccess.Read, FileShare.Read);
			fileStream.Seek(r.Range.StartIndex * blockSize, SeekOrigin.Begin);
			num += fileStream.Read(array, num, r.Range.NumBlocks * blockSize);
		}
		return array;
	}
}
