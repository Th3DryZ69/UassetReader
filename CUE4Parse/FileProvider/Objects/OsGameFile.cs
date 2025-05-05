using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.Compression;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.FileProvider.Objects;

public class OsGameFile : VersionedGameFile
{
	public readonly FileInfo ActualFile;

	public override bool IsEncrypted => false;

	public override CompressionMethod CompressionMethod => CompressionMethod.None;

	public OsGameFile(DirectoryInfo baseDir, FileInfo info, string mountPoint, VersionContainer versions)
		: base(mountPoint + info.FullName.Substring(baseDir.FullName.Length + 1).Replace('\\', '/'), info.Length, versions)
	{
		ActualFile = info;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override byte[] Read()
	{
		return File.ReadAllBytes(ActualFile.FullName);
	}
}
