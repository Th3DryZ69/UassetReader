using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CUE4Parse.Encryption.Aes;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.IO;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.UE4.VirtualFileCache;
using CUE4Parse.UE4.VirtualFileCache.Manifest;
using CUE4Parse.UE4.VirtualFileSystem;
using CUE4Parse.Utils;

namespace CUE4Parse.FileProvider.Vfs;

public abstract class AbstractVfsFileProvider : AbstractFileProvider, IVfsFileProvider, IFileProvider, IDisposable
{
	protected FileProviderDictionary _files;

	protected readonly ConcurrentDictionary<IAesVfsReader, object?> _unloadedVfs = new ConcurrentDictionary<IAesVfsReader, object>();

	private readonly ConcurrentDictionary<IAesVfsReader, object?> _mountedVfs = new ConcurrentDictionary<IAesVfsReader, object>();

	private readonly ConcurrentDictionary<FGuid, FAesKey> _keys = new ConcurrentDictionary<FGuid, FAesKey>();

	protected readonly ConcurrentDictionary<FGuid, object?> _requiredKeys = new ConcurrentDictionary<FGuid, object>();

	public override IReadOnlyDictionary<string, GameFile> Files => _files;

	public override IReadOnlyDictionary<FPackageId, GameFile> FilesById => _files.byId;

	public IReadOnlyCollection<IAesVfsReader> UnloadedVfs => (IReadOnlyCollection<IAesVfsReader>)_unloadedVfs.Keys;

	public IReadOnlyCollection<IAesVfsReader> MountedVfs => (IReadOnlyCollection<IAesVfsReader>)_mountedVfs.Keys;

	public IReadOnlyDictionary<FGuid, FAesKey> Keys => _keys;

	public IReadOnlyCollection<FGuid> RequiredKeys => (IReadOnlyCollection<FGuid>)_requiredKeys.Keys;

	public IoGlobalData? GlobalData { get; private set; }

	public IAesVfsReader.CustomEncryptionDelegate? CustomEncryption { get; set; }

	protected AbstractVfsFileProvider(bool isCaseInsensitive = false, VersionContainer? versions = null)
		: base(isCaseInsensitive, versions)
	{
		_files = new FileProviderDictionary(IsCaseInsensitive);
	}

	public IEnumerable<IAesVfsReader> UnloadedVfsByGuid(FGuid guid)
	{
		return _unloadedVfs.Keys.Where((IAesVfsReader it) => it.EncryptionKeyGuid == guid);
	}

	public void UnloadAllVfs()
	{
		_files.Clear();
		foreach (IAesVfsReader key in _mountedVfs.Keys)
		{
			_keys.TryRemove(key.EncryptionKeyGuid, out FAesKey _);
			_requiredKeys[key.EncryptionKeyGuid] = null;
			_mountedVfs.TryRemove(key, out object _);
			_unloadedVfs[key] = null;
		}
	}

	public void UnloadNonStreamedVfs()
	{
		Dictionary<string, GameFile> dictionary = new Dictionary<string, GameFile>();
		foreach (var (key, gameFile2) in _files)
		{
			if (gameFile2 is StreamedGameFile)
			{
				dictionary[key] = gameFile2;
			}
		}
		UnloadAllVfs();
		_files.AddFiles(dictionary);
	}

	public int Mount()
	{
		return MountAsync().Result;
	}

	public async Task<int> MountAsync()
	{
		int countNewMounts = 0;
		LinkedList<Task> linkedList = new LinkedList<Task>();
		foreach (IAesVfsReader reader in _unloadedVfs.Keys)
		{
			if (GlobalData == null && reader is IoStoreReader globalReader && (reader.Name.Equals("global.utoc", StringComparison.OrdinalIgnoreCase) || reader.Name.Equals("global_console_win.utoc", StringComparison.OrdinalIgnoreCase)))
			{
				GlobalData = new IoGlobalData(globalReader);
			}
			if ((reader.IsEncrypted && CustomEncryption == null) || !reader.HasDirectoryIndex)
			{
				continue;
			}
			linkedList.AddLast(Task.Run(delegate
			{
				try
				{
					reader.CustomEncryption = CustomEncryption;
					reader.MountTo(_files, IsCaseInsensitive);
					_unloadedVfs.TryRemove(reader, out object _);
					_mountedVfs[reader] = null;
					Interlocked.Increment(ref countNewMounts);
					return reader;
				}
				catch (InvalidAesKeyException)
				{
				}
				catch (Exception exception)
				{
					AbstractFileProvider.Log.Warning(exception, "Uncaught exception while loading file " + reader.Path.SubstringAfterLast('/'));
				}
				return (IAesVfsReader)null;
			}));
		}
		await Task.WhenAll(linkedList).ConfigureAwait(continueOnCapturedContext: false);
		return countNewMounts;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int SubmitKey(FGuid guid, FAesKey key)
	{
		return SubmitKeys(new Dictionary<FGuid, FAesKey> { { guid, key } });
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int SubmitKeys(IEnumerable<KeyValuePair<FGuid, FAesKey>> keys)
	{
		return SubmitKeysAsync(keys).Result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<int> SubmitKeyAsync(FGuid guid, FAesKey key)
	{
		return await SubmitKeysAsync(new Dictionary<FGuid, FAesKey> { { guid, key } }).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<int> SubmitKeysAsync(IEnumerable<KeyValuePair<FGuid, FAesKey>> keys)
	{
		int countNewMounts = 0;
		LinkedList<Task<IAesVfsReader>> linkedList = new LinkedList<Task<IAesVfsReader>>();
		foreach (KeyValuePair<FGuid, FAesKey> key3 in keys)
		{
			FGuid key = key3.Key;
			FAesKey key2 = key3.Value;
			foreach (IAesVfsReader reader in UnloadedVfsByGuid(key))
			{
				if (GlobalData == null && reader is IoStoreReader globalReader && (reader.Name.Equals("global.utoc", StringComparison.OrdinalIgnoreCase) || reader.Name.Equals("global_console_win.utoc", StringComparison.OrdinalIgnoreCase)))
				{
					GlobalData = new IoGlobalData(globalReader);
				}
				if (!reader.HasDirectoryIndex)
				{
					continue;
				}
				linkedList.AddLast(Task.Run(delegate
				{
					try
					{
						reader.MountTo(_files, IsCaseInsensitive, key2);
						_unloadedVfs.TryRemove(reader, out object _);
						_mountedVfs[reader] = null;
						Interlocked.Increment(ref countNewMounts);
						return reader;
					}
					catch (InvalidAesKeyException)
					{
					}
					catch (Exception exception)
					{
						AbstractFileProvider.Log.Warning(exception, "Uncaught exception while loading pak file " + reader.Path.SubstringAfterLast('/'));
					}
					return (IAesVfsReader)null;
				}));
			}
		}
		IAesVfsReader[] array = await Task.WhenAll(linkedList).ConfigureAwait(continueOnCapturedContext: false);
		foreach (IAesVfsReader aesVfsReader in array)
		{
			FAesKey fAesKey = aesVfsReader?.AesKey;
			if (aesVfsReader != null && fAesKey != null)
			{
				_requiredKeys.TryRemove(aesVfsReader.EncryptionKeyGuid, out object _);
				_keys.TryAdd(aesVfsReader.EncryptionKeyGuid, fAesKey);
			}
		}
		return countNewMounts;
	}

	public int LoadVirtualCache()
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), GameName, "Saved/PersistentDownloadDir");
		if (!Directory.Exists(text))
		{
			return 0;
		}
		string path = Path.Combine(text, "VFC", "vfc.meta");
		DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, "ManifestCache"));
		if (!File.Exists(path) || !directoryInfo.Exists)
		{
			return 0;
		}
		FileInfo[] files = directoryInfo.GetFiles("*.manifest");
		if (files.Length == 0)
		{
			return 0;
		}
		FFileTable fFileTable = new FFileTable(new FByteArchive("vfc.meta", File.ReadAllBytes(path)));
		OptimizedContentBuildManifest optimizedContentBuildManifest = new OptimizedContentBuildManifest(File.ReadAllBytes(files.OrderBy((FileInfo f) => f.LastWriteTime).Last().FullName));
		Dictionary<string, GameFile> dictionary = new Dictionary<string, GameFile>();
		foreach (var (fSHAHash2, dataReference) in fFileTable.FileMap)
		{
			if (optimizedContentBuildManifest.HashNameMap.TryGetValue(fSHAHash2.ToString(), out string value))
			{
				VfcGameFile vfcGameFile = new VfcGameFile(fFileTable.BlockFiles, dataReference, text, value, Versions);
				if (IsCaseInsensitive)
				{
					dictionary[vfcGameFile.Path.ToLowerInvariant()] = vfcGameFile;
				}
				else
				{
					dictionary[vfcGameFile.Path] = vfcGameFile;
				}
			}
		}
		_files.AddFiles(dictionary);
		return dictionary.Count;
	}

	public void Dispose()
	{
		_files = new FileProviderDictionary(IsCaseInsensitive);
		foreach (IAesVfsReader unloadedVf in UnloadedVfs)
		{
			unloadedVf.Dispose();
		}
		_unloadedVfs.Clear();
		foreach (IAesVfsReader mountedVf in MountedVfs)
		{
			mountedVf.Dispose();
		}
		_mountedVfs.Clear();
		_keys.Clear();
		_requiredKeys.Clear();
		GlobalData = null;
	}
}
