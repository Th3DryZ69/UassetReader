using System;
using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Readers;

public class FPointerArchive : FArchive
{
	private unsafe readonly byte* _ptr;

	public override bool CanSeek { get; } = true;

	public override long Length { get; }

	public override long Position { get; set; }

	public override string Name { get; }

	public unsafe FPointerArchive(string name, byte* ptr, long length, VersionContainer? versions = null)
		: base(versions)
	{
		_ptr = ptr;
		Name = name;
		Length = length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe override int Read(byte[] buffer, int offset, int count)
	{
		int num = (int)(Length - Position);
		if (num > count)
		{
			num = count;
		}
		if (num <= 0)
		{
			return 0;
		}
		if (num <= 8)
		{
			int num2 = num;
			while (--num2 >= 0)
			{
				buffer[offset + num2] = _ptr[Position + num2];
			}
		}
		else
		{
			Unsafe.CopyBlockUnaligned(ref buffer[offset], ref _ptr[Position], (uint)num);
		}
		Position += num;
		return num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override long Seek(long offset, SeekOrigin origin)
	{
		Position = origin switch
		{
			SeekOrigin.Begin => offset, 
			SeekOrigin.Current => Position + offset, 
			SeekOrigin.End => Length + offset, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		return Position;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe override T Read<T>()
	{
		int num = Unsafe.SizeOf<T>();
		T result = Unsafe.ReadUnaligned<T>(ref _ptr[Position]);
		Position += num;
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override byte[] ReadBytes(int length)
	{
		byte[] array = new byte[length];
		Read(array, 0, length);
		return array;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe override void Serialize(byte* ptr, int length)
	{
		Unsafe.CopyBlockUnaligned(ref *ptr, ref _ptr[Position], (uint)length);
		Position += length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe override T[] ReadArray<T>(int length)
	{
		int num = length * Unsafe.SizeOf<T>();
		T[] array = new T[length];
		if (length > 0)
		{
			Unsafe.CopyBlockUnaligned(ref Unsafe.As<T, byte>(ref array[0]), ref _ptr[Position], (uint)num);
		}
		Position += num;
		return array;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe override void ReadArray<T>(T[] array)
	{
		if (array.Length != 0)
		{
			int num = array.Length * Unsafe.SizeOf<T>();
			Unsafe.CopyBlockUnaligned(ref Unsafe.As<T, byte>(ref array[0]), ref _ptr[Position], (uint)num);
			Position += num;
		}
	}

	public unsafe override object Clone()
	{
		return new FPointerArchive(Name, _ptr, Length, Versions)
		{
			Position = Position
		};
	}
}
