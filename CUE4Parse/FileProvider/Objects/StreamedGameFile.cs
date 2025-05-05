using System.IO;
using CUE4Parse.Compression;
using CUE4Parse.UE4.Versions;
using Ionic.Zip;

namespace CUE4Parse.FileProvider.Objects;

public class StreamedGameFile : VersionedGameFile
{
	private readonly Stream _baseStream;

	private readonly long _position;

	public override bool IsEncrypted => false;

	public override CUE4Parse.Compression.CompressionMethod CompressionMethod => CUE4Parse.Compression.CompressionMethod.None;

	public StreamedGameFile(string path, Stream stream, VersionContainer versions)
		: base(path, stream.Length, versions)
	{
		_baseStream = stream;
		_position = _baseStream.Position;
	}

	public override byte[] Read()
	{
		byte[] array = new byte[base.Size];
		_baseStream.Seek(_position, SeekOrigin.Begin);
		if (_baseStream.Read(array, 0, array.Length) != base.Size)
		{
			throw new BadReadException("Read operation mismatch: bytesRead ≠ Size");
		}
		return array;
	}
}
