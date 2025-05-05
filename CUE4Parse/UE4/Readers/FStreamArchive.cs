using System;
using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Readers;

public class FStreamArchive : FArchive
{
	private readonly Stream _baseStream;

	public override bool CanSeek => _baseStream.CanSeek;

	public override long Length => _baseStream.Length;

	public override long Position
	{
		get
		{
			return _baseStream.Position;
		}
		set
		{
			_baseStream.Position = value;
		}
	}

	public override string Name { get; }

	public FStreamArchive(string name, Stream baseStream, VersionContainer? versions = null)
		: base(versions)
	{
		_baseStream = baseStream;
		Name = name;
	}

	public override void Close()
	{
		_baseStream.Close();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int Read(byte[] buffer, int offset, int count)
	{
		return _baseStream.Read(buffer, offset, count);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override long Seek(long offset, SeekOrigin origin)
	{
		return _baseStream.Seek(offset, origin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override byte[] ReadBytes(int length)
	{
		byte[] array = new byte[length];
		_baseStream.Read(array, 0, length);
		return array;
	}

	public override object Clone()
	{
		Stream baseStream = _baseStream;
		if (!(baseStream is ICloneable cloneable))
		{
			if (baseStream is FileStream fileStream)
			{
				return new FStreamArchive(Name, File.Open(fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Versions)
				{
					Position = Position
				};
			}
			return new FStreamArchive(Name, _baseStream, Versions)
			{
				Position = Position
			};
		}
		return new FStreamArchive(Name, (Stream)cloneable.Clone(), Versions)
		{
			Position = Position
		};
	}
}
