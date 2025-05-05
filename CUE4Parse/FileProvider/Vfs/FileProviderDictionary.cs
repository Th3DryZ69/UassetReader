using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.Utils;

namespace CUE4Parse.FileProvider.Vfs;

public class FileProviderDictionary : IReadOnlyDictionary<string, GameFile>, IEnumerable<KeyValuePair<string, GameFile>>, IEnumerable, IReadOnlyCollection<KeyValuePair<string, GameFile>>
{
	private class KeyEnumerable : IEnumerable<string>, IEnumerable
	{
		private readonly FileProviderDictionary _orig;

		internal KeyEnumerable(FileProviderDictionary orig)
		{
			_orig = orig;
		}

		public IEnumerator<string> GetEnumerator()
		{
			foreach (IReadOnlyDictionary<string, GameFile> item in _orig._indicesBag)
			{
				foreach (string key in item.Keys)
				{
					yield return key;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private class ValueEnumerable : IEnumerable<GameFile>, IEnumerable
	{
		private readonly FileProviderDictionary _orig;

		internal ValueEnumerable(FileProviderDictionary orig)
		{
			_orig = orig;
		}

		public IEnumerator<GameFile> GetEnumerator()
		{
			foreach (IReadOnlyDictionary<string, GameFile> item in _orig._indicesBag)
			{
				foreach (GameFile value in item.Values)
				{
					yield return value;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private readonly ConcurrentDictionary<FPackageId, GameFile> _byId = new ConcurrentDictionary<FPackageId, GameFile>();

	private readonly KeyEnumerable _keys;

	private readonly ValueEnumerable _values;

	private readonly ConcurrentBag<IReadOnlyDictionary<string, GameFile>> _indicesBag = new ConcurrentBag<IReadOnlyDictionary<string, GameFile>>();

	public readonly bool IsCaseInsensitive;

	public IReadOnlyDictionary<FPackageId, GameFile> byId => _byId;

	public IEnumerable<string> Keys => _keys;

	public IEnumerable<GameFile> Values => _values;

	public GameFile this[string path]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (TryGetValue(path, out GameFile value))
			{
				return value;
			}
			if (TryGetValue(path.SubstringBeforeWithLast('.') + GameFile.Ue4PackageExtensions[1], out value))
			{
				return value;
			}
			throw new KeyNotFoundException("There is no game file with the path \"" + path + "\"");
		}
	}

	public int Count => _indicesBag.Sum((IReadOnlyDictionary<string, GameFile> it) => it.Count);

	public FileProviderDictionary(bool isCaseInsensitive)
	{
		IsCaseInsensitive = isCaseInsensitive;
		_keys = new KeyEnumerable(this);
		_values = new ValueEnumerable(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddFiles(IReadOnlyDictionary<string, GameFile> newFiles)
	{
		foreach (GameFile value in newFiles.Values)
		{
			if (value is FIoStoreEntry fIoStoreEntry && value.IsUE4Package)
			{
				_byId[fIoStoreEntry.ChunkId.AsPackageId()] = value;
			}
		}
		_indicesBag.Add(newFiles);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Clear()
	{
		_indicesBag.Clear();
		_byId.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool ContainsKey(string key)
	{
		if (IsCaseInsensitive)
		{
			key = key.ToLowerInvariant();
		}
		foreach (IReadOnlyDictionary<string, GameFile> item in _indicesBag)
		{
			if (item.ContainsKey(key))
			{
				return true;
			}
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryGetValue(string key, out GameFile value)
	{
		if (IsCaseInsensitive)
		{
			key = key.ToLowerInvariant();
		}
		foreach (IReadOnlyDictionary<string, GameFile> item in _indicesBag)
		{
			if (item.TryGetValue(key, out value))
			{
				return true;
			}
		}
		value = null;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public IEnumerator<KeyValuePair<string, GameFile>> GetEnumerator()
	{
		foreach (IReadOnlyDictionary<string, GameFile> item in _indicesBag)
		{
			foreach (KeyValuePair<string, GameFile> item2 in item)
			{
				yield return item2;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
