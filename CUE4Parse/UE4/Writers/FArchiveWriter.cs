using System.IO;

namespace CUE4Parse.UE4.Writers;

public class FArchiveWriter : BinaryWriter
{
	private readonly MemoryStream _memoryData;

	public long Length => _memoryData.Length;

	public long Position => _memoryData.Position;

	public FArchiveWriter()
	{
		_memoryData = new MemoryStream
		{
			Position = 0L
		};
		OutStream = _memoryData;
	}

	public byte[] GetBuffer()
	{
		return _memoryData.ToArray();
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		_memoryData.Dispose();
	}
}
