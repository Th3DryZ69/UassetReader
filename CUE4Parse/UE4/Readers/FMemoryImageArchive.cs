using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Readers;

public class FMemoryImageArchive : FArchive
{
	protected readonly FArchive InnerArchive;

	public IReadOnlyDictionary<int, FName>? Names;

	public override bool CanSeek => InnerArchive.CanSeek;

	public override long Length => InnerArchive.Length;

	public override long Position
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return InnerArchive.Position;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			InnerArchive.Position = value;
		}
	}

	public override string Name => InnerArchive.Name;

	public FMemoryImageArchive(FArchive Ar)
		: base(Ar.Versions)
	{
		InnerArchive = Ar;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return InnerArchive.Read(buffer, offset, count);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return InnerArchive.Seek(offset, origin);
	}

	public override T Read<T>()
	{
		return InnerArchive.Read<T>();
	}

	public override byte[] ReadBytes(int length)
	{
		return InnerArchive.ReadBytes(length);
	}

	public unsafe override void Serialize(byte* ptr, int length)
	{
		InnerArchive.Serialize(ptr, length);
	}

	public override T[] ReadArray<T>(int length)
	{
		return InnerArchive.ReadArray<T>(length);
	}

	public override void ReadArray<T>(T[] array)
	{
		InnerArchive.ReadArray(array);
	}

	public override T[] ReadArray<T>()
	{
		long position = Position;
		FFrozenMemoryImagePtr fFrozenMemoryImagePtr = new FFrozenMemoryImagePtr(this);
		int num = Read<int>();
		int num2 = Read<int>();
		if (num != num2)
		{
			throw new ParserException(this, $"Num ({num}) != Max ({num2})");
		}
		if (num == 0)
		{
			return Array.Empty<T>();
		}
		long position2 = Position;
		Position = position + fFrozenMemoryImagePtr.OffsetFromThis;
		T[] result = InnerArchive.ReadArray<T>(num);
		Position = position2;
		return result;
	}

	public override T[] ReadArray<T>(Func<T> getter)
	{
		long position = Position;
		FFrozenMemoryImagePtr fFrozenMemoryImagePtr = new FFrozenMemoryImagePtr(this);
		int num = Read<int>();
		int num2 = Read<int>();
		if (num != num2)
		{
			throw new ParserException(this, $"Num ({num}) != Max ({num2})");
		}
		if (num == 0)
		{
			return Array.Empty<T>();
		}
		long position2 = Position;
		Position = position + fFrozenMemoryImagePtr.OffsetFromThis;
		T[] array = new T[num];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = getter();
			Position = Position.Align(8);
		}
		Position = position2;
		return array;
	}

	public T[] ReadArrayOfPtrs<T>(Func<T> getter)
	{
		long position = Position;
		FFrozenMemoryImagePtr fFrozenMemoryImagePtr = new FFrozenMemoryImagePtr(this);
		int num = Read<int>();
		int num2 = Read<int>();
		if (num != num2)
		{
			throw new ParserException(this, $"Num ({num}) != Max ({num2})");
		}
		if (num == 0)
		{
			return Array.Empty<T>();
		}
		long position2 = Position;
		Position = position + fFrozenMemoryImagePtr.OffsetFromThis;
		T[] array = new T[num];
		for (int i = 0; i < array.Length; i++)
		{
			long position3 = Position;
			Position = position3 + new FFrozenMemoryImagePtr(this).OffsetFromThis;
			array[i] = getter();
			Position = (position3 + 8).Align(8);
		}
		Position = position2;
		return array;
	}

	public int[] ReadHashTable()
	{
		_ = Position;
		new FFrozenMemoryImagePtr(this);
		new FFrozenMemoryImagePtr(this);
		Read<uint>();
		Read<uint>();
		return Array.Empty<int>();
	}

	public override string ReadFString()
	{
		long position = Position;
		FFrozenMemoryImagePtr fFrozenMemoryImagePtr = new FFrozenMemoryImagePtr(this);
		int num = Read<int>();
		int num2 = Read<int>();
		if (num != num2)
		{
			throw new ParserException(this, $"Num ({num}) != Max ({num2})");
		}
		if (num <= 1)
		{
			return string.Empty;
		}
		long position2 = Position;
		Position = position + fFrozenMemoryImagePtr.OffsetFromThis;
		byte[] array = ReadBytes(num * 2);
		Position = position2;
		if (array[^1] != 0 || array[^2] != 0)
		{
			throw new ParserException(this, "Serialized FString is not null terminated");
		}
		return Encoding.Unicode.GetString(array, 0, array.Length - 2);
	}

	public override object Clone()
	{
		return new FMemoryImageArchive((FArchive)InnerArchive.Clone());
	}

	public IEnumerable<(KeyType, ValueType)> ReadTMap<KeyType, ValueType>(Func<KeyType> keyGetter, Func<ValueType> valueGetter, int keyStructSize, int valueStructSize) where KeyType : notnull
	{
		return ReadTSet(() => (keyGetter(), valueGetter()), (keyStructSize + valueStructSize).Align(8));
	}

	public IEnumerable<ElementType> ReadTSet<ElementType>(Func<ElementType> elementGetter, int elementStructSize)
	{
		IEnumerable<ElementType> result = ReadTSparseArray(elementGetter, elementStructSize + 4 + 4);
		Position += 12L;
		return result;
	}

	public IEnumerable<ElementType> ReadTSparseArray<ElementType>(Func<ElementType> elementGetter, int elementStructSize)
	{
		long position = Position;
		FFrozenMemoryImagePtr fFrozenMemoryImagePtr = new FFrozenMemoryImagePtr(this);
		int num = Read<int>();
		Read<int>();
		BitArray bitArray = ReadTBitArray();
		Position += 8L;
		if (num == 0)
		{
			return new List<ElementType>();
		}
		long position2 = Position;
		Position = position + fFrozenMemoryImagePtr.OffsetFromThis;
		List<ElementType> list = new List<ElementType>(num);
		for (int i = 0; i < num; i++)
		{
			long position3 = Position;
			if (bitArray[i])
			{
				list.Add(elementGetter());
			}
			Position = position3 + elementStructSize;
		}
		Position = position2;
		return list;
	}

	public BitArray ReadTBitArray()
	{
		long position = Position;
		FFrozenMemoryImagePtr fFrozenMemoryImagePtr = new FFrozenMemoryImagePtr(this);
		int num = Read<int>();
		Read<int>();
		if (num == 0)
		{
			return new BitArray(0);
		}
		long position2 = Position;
		Position = position + fFrozenMemoryImagePtr.OffsetFromThis;
		int[] values = InnerArchive.ReadArray<int>(num.DivideAndRoundUp(32));
		Position = position2;
		return new BitArray(values)
		{
			Length = num
		};
	}

	public override FName ReadFName()
	{
		Position += 12L;
		if (Names != null && Names.TryGetValue((int)Position, out var value))
		{
			return value;
		}
		return default(FName);
	}
}
