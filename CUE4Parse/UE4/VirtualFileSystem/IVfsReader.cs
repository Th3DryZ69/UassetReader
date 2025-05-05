using System;
using System.Collections.Generic;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.VirtualFileSystem;

public interface IVfsReader : IDisposable
{
	string Path { get; }

	string Name { get; }

	IReadOnlyDictionary<string, GameFile> Files { get; }

	int FileCount { get; }

	bool HasDirectoryIndex { get; }

	string MountPoint { get; }

	bool IsConcurrent { get; set; }

	bool IsMounted { get; }

	VersionContainer Versions { get; set; }

	EGame Game { get; set; }

	FPackageFileVersion Ver { get; set; }

	IReadOnlyDictionary<string, GameFile> Mount(bool caseInsensitive = false);

	void MountTo(FileProviderDictionary files, bool caseInsensitive);

	byte[] Extract(VfsEntry entry);
}
