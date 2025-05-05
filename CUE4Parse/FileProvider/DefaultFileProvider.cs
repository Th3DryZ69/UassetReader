using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.IO;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Pak;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;
using Ionic.Zip;

namespace CUE4Parse.FileProvider;

public class DefaultFileProvider : AbstractVfsFileProvider
{
	private DirectoryInfo _workingDirectory;

	private readonly SearchOption _searchOption;

	private readonly List<DirectoryInfo> _extraDirectories;

	public DefaultFileProvider(string directory, SearchOption searchOption, bool isCaseInsensitive = false, VersionContainer? versions = null)
		: this(new DirectoryInfo(directory), searchOption, isCaseInsensitive, versions)
	{
	}

	public DefaultFileProvider(DirectoryInfo directory, SearchOption searchOption, bool isCaseInsensitive = false, VersionContainer? versions = null)
		: base(isCaseInsensitive, versions)
	{
		_workingDirectory = directory;
		_searchOption = searchOption;
	}

	public DefaultFileProvider(DirectoryInfo mainDirectory, List<DirectoryInfo> extraDirectories, SearchOption searchOption, bool isCaseInsensitive = false, VersionContainer? versions = null)
		: base(isCaseInsensitive, versions)
	{
		_workingDirectory = mainDirectory;
		_extraDirectories = extraDirectories;
		_searchOption = searchOption;
	}

	public void Initialize()
	{
		if (!_workingDirectory.Exists)
		{
			throw new ArgumentException("Given directory must exist", "_workingDirectory");
		}
		List<Dictionary<string, GameFile>> list = new List<Dictionary<string, GameFile>> { IterateFiles(_workingDirectory, _searchOption) };
		List<DirectoryInfo> extraDirectories = _extraDirectories;
		if (extraDirectories != null && extraDirectories.Count > 0)
		{
			list.AddRange(_extraDirectories.Select((DirectoryInfo directory) => IterateFiles(directory, _searchOption)));
		}
		foreach (Dictionary<string, GameFile> item in list)
		{
			_files.AddFiles(item);
		}
	}

	private void RegisterFile(string file, Stream[] stream = null, Func<string, FArchive>? openContainerStreamFunc = null)
	{
		string text = file.SubstringAfterLast('.');
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

	private void RegisterFile(FileInfo file)
	{
		string text = file.FullName.SubstringAfterLast('.');
		if (text.Equals("pak", StringComparison.OrdinalIgnoreCase))
		{
			RegisterFile(file.FullName, new Stream[1] { file.OpenRead() });
		}
		else if (text.Equals("utoc", StringComparison.OrdinalIgnoreCase))
		{
			RegisterFile(file.FullName, new Stream[1] { file.OpenRead() }, (string it) => new FStreamArchive(it, File.OpenRead(it), Versions));
		}
		else
		{
			if (!text.Equals("apk", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			ZipFile zipFile = new ZipFile(file.FullName);
			MemoryStream memoryStream = new MemoryStream();
			foreach (ZipEntry entry in zipFile.Entries)
			{
				if (!entry.FileName.EndsWith("main.obb.png", StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				entry.Extract(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
				ZipFile zipFile2 = ZipFile.Read(memoryStream);
				foreach (ZipEntry entry2 in zipFile2.Entries)
				{
					Stream[] array = new Stream[2];
					if (entry2.FileName.EndsWith(".pak"))
					{
						try
						{
							array[0] = new MemoryStream();
							entry2.Extract(array[0]);
							array[0].Seek(0L, SeekOrigin.Begin);
						}
						catch (Exception ex)
						{
							AbstractFileProvider.Log.Warning(ex.ToString());
						}
					}
					else
					{
						if (!entry2.FileName.EndsWith(".utoc"))
						{
							continue;
						}
						try
						{
							array[0] = new MemoryStream();
							entry2.Extract(array[0]);
							array[0].Seek(0L, SeekOrigin.Begin);
							foreach (ZipEntry entry3 in zipFile2.Entries)
							{
								if (entry3.FileName.Equals(entry2.FileName.SubstringBeforeLast('.') + ".ucas"))
								{
									array[1] = new MemoryStream();
									entry3.Extract(array[1]);
									array[1].Seek(0L, SeekOrigin.Begin);
									break;
								}
							}
							if (array[1] == null)
							{
								continue;
							}
						}
						catch (Exception ex2)
						{
							AbstractFileProvider.Log.Warning(ex2.ToString());
						}
					}
					RegisterFile(entry2.FileName, array);
				}
			}
		}
	}

	private Dictionary<string, GameFile> IterateFiles(DirectoryInfo directory, SearchOption option)
	{
		Dictionary<string, GameFile> dictionary = new Dictionary<string, GameFile>();
		if (!directory.Exists)
		{
			return dictionary;
		}
		FileInfo fileInfo = directory.GetFiles("*.uproject", SearchOption.TopDirectoryOnly).FirstOrDefault();
		string mountPoint = ((fileInfo == null) ? (directory.Name + "/") : (fileInfo.Name.SubstringBeforeLast('.') + "/"));
		option = ((fileInfo != null) ? SearchOption.AllDirectories : option);
		foreach (FileInfo item in directory.EnumerateFiles("*.*", option))
		{
			string value = item.Extension.SubstringAfter('.');
			if (!item.Exists || string.IsNullOrEmpty(value))
			{
				continue;
			}
			if (item.Name.Contains("ProSwapper") || item.Name.Contains("Saturn"))
			{
				AbstractFileProvider.Log.Warning(item.Name + " was blocked. Other swapper files are not allowed!");
				continue;
			}
			if (item.Name.Contains(".o"))
			{
				AbstractFileProvider.Log.Warning(item.Name + " was blocked. UEFN files are not allowed!");
				continue;
			}
			if (fileInfo == null)
			{
				RegisterFile(item);
			}
			if (GameFile.Ue4KnownExtensions.Contains<string>(value, StringComparer.OrdinalIgnoreCase))
			{
				OsGameFile osGameFile = new OsGameFile(_workingDirectory, item, mountPoint, Versions);
				if (IsCaseInsensitive)
				{
					dictionary[osGameFile.Path.ToLowerInvariant()] = osGameFile;
				}
				else
				{
					dictionary[osGameFile.Path] = osGameFile;
				}
			}
		}
		return dictionary;
	}
}
