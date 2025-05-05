using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Uasset_Reader.Workspace.Swapping.Utilities;

public class Reader : BinaryReader
{
	public long Length => base.BaseStream.Length;

	public long Position
	{
		get
		{
			return base.BaseStream.Position;
		}
		set
		{
			base.BaseStream.Position = value;
		}
	}

	public Reader(string file)
		: base(File.OpenRead(file))
	{
	}

	public Reader(byte[] data)
		: base(new MemoryStream(data))
	{
	}

	public new byte[] ReadBytes(int length)
	{
		byte[] array = new byte[length];
		Read(array, 0, length);
		return array;
	}

	public T Read<T>()
	{
		if (typeof(T) == typeof(string) || typeof(T) == typeof(object))
		{
			int length = Read<int>();
			return (T)(object)Encoding.ASCII.GetString(ReadBytes(length));
		}
		return Unsafe.ReadUnaligned<T>(ref ReadBytes(Unsafe.SizeOf<T>())[0]);
	}

	public T[] ReadArray<T>()
	{
		T[] array = new T[Read<int>()];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Read<T>();
		}
		return array;
	}

	public T[] ReadArray<T>(int length)
	{
		T[] array = new T[length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Read<T>();
		}
		return array;
	}

	public Dictionary<TKey, TValue> ReadMap<TKey, TValue>()
	{
		Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
		int num = Read<int>();
		for (int i = 0; i < num; i++)
		{
			TKey key = Read<TKey>();
			bool flag = Read<bool>();
			dictionary.Add(key, flag ? ((TValue)(object)ReadArray<TValue>()) : Read<TValue>());
		}
		return dictionary;
	}
}
