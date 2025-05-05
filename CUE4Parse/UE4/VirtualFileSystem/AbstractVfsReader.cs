using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;
using Serilog;

namespace CUE4Parse.UE4.VirtualFileSystem;

public abstract class AbstractVfsReader : IVfsReader, IDisposable
{
	protected static readonly ILogger log = Log.ForContext<AbstractVfsReader>();

	protected const int MAX_MOUNTPOINT_TEST_LENGTH = 128;

	public string Path { get; }

	public string Name { get; }

	public IReadOnlyDictionary<string, GameFile> Files { get; protected set; }

	public virtual int FileCount => Files.Count;

	public abstract bool HasDirectoryIndex { get; }

	public abstract string MountPoint { get; protected set; }

	public bool IsConcurrent { get; set; }

	public bool IsMounted { get; }

	public VersionContainer Versions { get; set; }

	public EGame Game
	{
		get
		{
			return Versions.Game;
		}
		set
		{
			Versions.Game = value;
		}
	}

	public FPackageFileVersion Ver
	{
		get
		{
			return Versions.Ver;
		}
		set
		{
			Versions.Ver = value;
		}
	}

	protected AbstractVfsReader(string path, VersionContainer versions)
	{
		Path = path;
		Name = path.Replace('\\', '/').SubstringAfterLast('/');
		Versions = versions;
		Files = new Dictionary<string, GameFile>();
	}

	public abstract IReadOnlyDictionary<string, GameFile> Mount(bool caseInsensitive = false);

	public abstract byte[] Extract(VfsEntry entry);

	protected void ValidateMountPoint(ref string mountPoint)
	{
		bool flag = !mountPoint.StartsWith("../../..");
		mountPoint = mountPoint.SubstringAfter("../../..");
		if (mountPoint[0] != '/' || (mountPoint.Length > 1 && mountPoint[1] == '.'))
		{
			flag = true;
		}
		if (flag)
		{
			if (Globals.LogVfsMounts)
			{
				log.Warning($"\"{Name}\" has strange mount point \"{mountPoint}\", mounting to root");
			}
			mountPoint = "/";
		}
		mountPoint = mountPoint.Substring(1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsValidIndex(byte[] testBytes)
	{
		return IsValidIndex(new FByteArchive(string.Empty, testBytes));
	}

	public static bool IsValidIndex(FArchive reader)
	{
		int num = reader.Read<int>();
		if (num > 128 || num < -128)
		{
			return false;
		}
		if (num == 0)
		{
			return reader.Read<byte>() == 0;
		}
		if (num < 0)
		{
			reader.Seek(-(num - 1) * 2, SeekOrigin.Current);
			return reader.Read<short>() == 0;
		}
		reader.Seek(num - 1, SeekOrigin.Current);
		return reader.Read<byte>() == 0;
	}

	public abstract void Dispose();

	public override string ToString()
	{
		return Path;
	}

	public void MountTo(FileProviderDictionary files, bool caseInsensitive)
	{
		files.AddFiles(Mount(caseInsensitive));
	}
}
