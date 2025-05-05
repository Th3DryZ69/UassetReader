using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CUE4Parse.Encryption.Aes;
using CUE4Parse.UE4.IO;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.VirtualFileSystem;

namespace CUE4Parse.FileProvider.Vfs;

public interface IVfsFileProvider : IFileProvider, IDisposable
{
	IReadOnlyCollection<IAesVfsReader> UnloadedVfs { get; }

	IReadOnlyCollection<IAesVfsReader> MountedVfs { get; }

	IoGlobalData? GlobalData { get; }

	IAesVfsReader.CustomEncryptionDelegate? CustomEncryption { get; set; }

	IReadOnlyDictionary<FGuid, FAesKey> Keys { get; }

	IReadOnlyCollection<FGuid> RequiredKeys { get; }

	int Mount();

	Task<int> MountAsync();

	int SubmitKey(FGuid guid, FAesKey key);

	Task<int> SubmitKeyAsync(FGuid guid, FAesKey key);

	int SubmitKeys(IEnumerable<KeyValuePair<FGuid, FAesKey>> keys);

	Task<int> SubmitKeysAsync(IEnumerable<KeyValuePair<FGuid, FAesKey>> keys);
}
