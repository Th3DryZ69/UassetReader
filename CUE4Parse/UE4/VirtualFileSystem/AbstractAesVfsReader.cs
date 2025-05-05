using System;
using System.Runtime.CompilerServices;
using CUE4Parse.Encryption.Aes;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.VirtualFileSystem;

public abstract class AbstractAesVfsReader : AbstractVfsReader, IAesVfsReader, IVfsReader, IDisposable
{
	private static EGame _game;

	public abstract FGuid EncryptionKeyGuid { get; }

	public abstract long Length { get; set; }

	public IAesVfsReader.CustomEncryptionDelegate? CustomEncryption { get; set; }

	public FAesKey? AesKey { get; set; }

	public abstract bool IsEncrypted { get; }

	public int EncryptedFileCount { get; protected set; }

	protected AbstractAesVfsReader(string path, VersionContainer versions)
		: base(path, versions)
	{
		_game = base.Game;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TestAesKey(FAesKey key)
	{
		if (IsEncrypted)
		{
			return TestAesKey(MountPointCheckBytes(), key);
		}
		return true;
	}

	public abstract byte[] MountPointCheckBytes();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TestAesKey(byte[] bytes, FAesKey key)
	{
		return AbstractVfsReader.IsValidIndex((_game == EGame.GAME_ApexLegendsMobile) ? bytes.DecryptApexMobile(key) : bytes.Decrypt(key));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected byte[] DecryptIfEncrypted(byte[] bytes)
	{
		return DecryptIfEncrypted(bytes, IsEncrypted);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected byte[] DecryptIfEncrypted(byte[] bytes, int beginOffset, int count)
	{
		return DecryptIfEncrypted(bytes, beginOffset, count, IsEncrypted);
	}

	protected byte[] DecryptIfEncrypted(byte[] bytes, bool isEncrypted)
	{
		if (!isEncrypted)
		{
			return bytes;
		}
		if (CustomEncryption != null)
		{
			return CustomEncryption(bytes, 0, bytes.Length, this);
		}
		if (AesKey != null && TestAesKey(AesKey))
		{
			if (_game != EGame.GAME_ApexLegendsMobile)
			{
				return bytes.Decrypt(AesKey);
			}
			return bytes.DecryptApexMobile(AesKey);
		}
		throw new InvalidAesKeyException("Reading encrypted data requires a valid aes key");
	}

	protected byte[] DecryptIfEncrypted(byte[] bytes, int beginOffset, int count, bool isEncrypted)
	{
		if (!isEncrypted)
		{
			return bytes;
		}
		if (CustomEncryption != null)
		{
			return CustomEncryption(bytes, beginOffset, count, this);
		}
		if (AesKey != null)
		{
			if (_game != EGame.GAME_ApexLegendsMobile)
			{
				return bytes.Decrypt(AesKey);
			}
			return bytes.DecryptApexMobile(AesKey);
		}
		throw new InvalidAesKeyException("Reading encrypted data requires a valid aes key");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected abstract byte[] ReadAndDecrypt(int length);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected byte[] ReadAndDecrypt(int length, FArchive reader, bool isEncrypted)
	{
		return DecryptIfEncrypted(reader.ReadBytes(length), isEncrypted);
	}

	public void MountTo(FileProviderDictionary files, bool caseInsensitive, FAesKey? key)
	{
		AesKey = key;
		MountTo(files, caseInsensitive);
	}
}
