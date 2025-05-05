#define TRACE
using System;
using System.Diagnostics;
using System.IO;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Readers;

public class FArchiveLoadCompressedProxy : FArchive
{
	private readonly byte[] _compressedData;

	private int _currentIndex;

	private readonly byte[] _tmpData;

	private int _tmpDataPos;

	private int _tmpDataSize;

	private bool _shouldSerializeFromArray;

	private long _rawBytesSerialized;

	private readonly string _compressionFormat;

	private readonly ECompressionFlags _compressionFlags;

	public override string Name { get; }

	public override bool CanSeek => true;

	public override long Length
	{
		get
		{
			throw new InvalidOperationException();
		}
	}

	public override long Position
	{
		get
		{
			return _rawBytesSerialized;
		}
		set
		{
			Seek(value, SeekOrigin.Begin);
		}
	}

	public FArchiveLoadCompressedProxy(string name, byte[] compressedData, string compressionFormat, ECompressionFlags flags = ECompressionFlags.COMPRESS_None, VersionContainer? versions = null)
		: base(versions)
	{
		Name = name;
		_compressedData = compressedData;
		_compressionFormat = compressionFormat;
		_compressionFlags = flags;
		_tmpData = new byte[131072];
		_tmpDataPos = 131072;
		_tmpDataSize = 131072;
	}

	public override object Clone()
	{
		return new FArchiveLoadCompressedProxy(Name, _compressedData, _compressionFormat, _compressionFlags, Versions);
	}

	public override int Read(byte[]? dstData, int offset, int count)
	{
		if (_shouldSerializeFromArray)
		{
			Trace.Assert(_currentIndex + count <= _compressedData.Length);
			Buffer.BlockCopy(_compressedData, _currentIndex, dstData, 0, count);
			_currentIndex += count;
			return count;
		}
		int num = 0;
		while (count > 0)
		{
			int num2 = Math.Min(count, _tmpDataSize - _tmpDataPos);
			if (num2 > 0)
			{
				if (dstData != null)
				{
					Buffer.BlockCopy(_tmpData, _tmpDataPos, dstData, num, num2);
					num += num2;
				}
				count -= num2;
				_tmpDataPos += num2;
				_rawBytesSerialized += num2;
			}
			else
			{
				DecompressMoreData();
				if (_tmpDataSize == 0)
				{
					throw new ParserException();
				}
			}
		}
		return num;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		Trace.Assert(origin == SeekOrigin.Begin);
		long position = Position;
		long num = offset - position;
		Trace.Assert(num >= 0);
		Read(null, 0, (int)num);
		return Position;
	}

	private void DecompressMoreData()
	{
		_shouldSerializeFromArray = true;
		SerializeCompressedNew(_tmpData, 131072, _compressionFormat, _compressionFlags, bTreatBufferAsFileReader: false, out var outPartialReadLength);
		Trace.Assert(outPartialReadLength <= 131072);
		_shouldSerializeFromArray = false;
		_tmpDataPos = 0;
		_tmpDataSize = (int)outPartialReadLength;
	}
}
