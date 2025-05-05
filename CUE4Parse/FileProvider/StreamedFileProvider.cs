using System;
using System.IO;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.IO;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Pak;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;

namespace CUE4Parse.FileProvider;

public class StreamedFileProvider : AbstractVfsFileProvider
{
	public string LiveGame { get; }

	public StreamedFileProvider(string liveGame, bool isCaseInsensitive = false, VersionContainer? versions = null)
		: base(isCaseInsensitive, versions)
	{
		LiveGame = liveGame;
	}

	public void Initialize(string file = "", Stream[] stream = null, Func<string, FArchive>? openContainerStreamFunc = null)
	{
		string text = file.SubstringAfter('.');
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (text.Equals("pak", StringComparison.OrdinalIgnoreCase))
		{
			try
			{
				PakFileReader pakFileReader = new PakFileReader(file, stream[0], Versions)
				{
					IsConcurrent = true,
					CustomEncryption = base.CustomEncryption
				};
				if (pakFileReader.IsEncrypted && !_requiredKeys.ContainsKey(pakFileReader.Info.EncryptionKeyGuid))
				{
					_requiredKeys[pakFileReader.Info.EncryptionKeyGuid] = null;
				}
				_unloadedVfs[pakFileReader] = null;
				return;
			}
			catch (Exception ex)
			{
				AbstractFileProvider.Log.Warning(ex.ToString());
				return;
			}
		}
		if (!text.Equals("utoc", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}
		if (openContainerStreamFunc == null)
		{
			openContainerStreamFunc = (string it) => new FStreamArchive(it, stream[1], Versions);
		}
		try
		{
			IoStoreReader ioStoreReader = new IoStoreReader(file, stream[0], openContainerStreamFunc, EIoStoreTocReadOptions.ReadDirectoryIndex, Versions)
			{
				IsConcurrent = true,
				CustomEncryption = base.CustomEncryption
			};
			if (ioStoreReader.IsEncrypted && !_requiredKeys.ContainsKey(ioStoreReader.Info.EncryptionKeyGuid))
			{
				_requiredKeys[ioStoreReader.Info.EncryptionKeyGuid] = null;
			}
			_unloadedVfs[ioStoreReader] = null;
		}
		catch (Exception ex2)
		{
			AbstractFileProvider.Log.Warning(ex2.ToString());
		}
	}
}
