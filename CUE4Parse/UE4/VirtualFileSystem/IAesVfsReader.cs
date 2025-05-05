using System;
using CUE4Parse.Encryption.Aes;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.Objects.Core.Misc;

namespace CUE4Parse.UE4.VirtualFileSystem;

public interface IAesVfsReader : IVfsReader, IDisposable
{
	public delegate byte[] CustomEncryptionDelegate(byte[] bytes, int beginOffset, int count, IAesVfsReader reader);

	FGuid EncryptionKeyGuid { get; }

	long Length { get; set; }

	CustomEncryptionDelegate? CustomEncryption { get; set; }

	FAesKey? AesKey { get; set; }

	bool IsEncrypted { get; }

	int EncryptedFileCount { get; }

	bool TestAesKey(FAesKey key);

	byte[] MountPointCheckBytes();

	void MountTo(FileProviderDictionary files, bool caseInsensitive, FAesKey? key);
}
