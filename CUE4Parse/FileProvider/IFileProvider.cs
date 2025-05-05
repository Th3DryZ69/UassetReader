using System.Collections.Generic;
using System.Threading.Tasks;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.FileProvider;

public interface IFileProvider
{
	VersionContainer Versions { get; set; }

	ITypeMappingsProvider? MappingsContainer { get; set; }

	TypeMappings? MappingsForGame { get; }

	IDictionary<string, IDictionary<string, string>> LocalizedResources { get; }

	IReadOnlyDictionary<string, GameFile> Files { get; }

	IReadOnlyDictionary<FPackageId, GameFile> FilesById { get; }

	bool IsCaseInsensitive { get; }

	bool ReadScriptData { get; set; }

	string GameName { get; }

	GameFile this[string path] { get; }

	string GetLocalizedString(string namespacee, string key, string defaultValue);

	bool TryFindGameFile(string path, out GameFile file);

	string FixPath(string path);

	byte[] SaveAsset(string path);

	bool TrySaveAsset(string path, out byte[] data);

	Task<byte[]> SaveAssetAsync(string path);

	Task<byte[]?> TrySaveAssetAsync(string path);

	FArchive CreateReader(string path);

	bool TryCreateReader(string path, out FArchive reader);

	Task<FArchive> CreateReaderAsync(string path);

	Task<FArchive?> TryCreateReaderAsync(string path);

	IPackage LoadPackage(string path);

	IPackage LoadPackage(GameFile file);

	IoPackage LoadPackage(FPackageId id);

	bool TryLoadPackage(string path, out IPackage package);

	bool TryLoadPackage(GameFile file, out IPackage package);

	bool TryLoadPackage(FPackageId id, out IoPackage package);

	Task<IPackage> LoadPackageAsync(string path);

	Task<IPackage> LoadPackageAsync(GameFile file);

	Task<IPackage?> TryLoadPackageAsync(string path);

	Task<IPackage?> TryLoadPackageAsync(GameFile file);

	IReadOnlyDictionary<string, byte[]> SavePackage(string path);

	IReadOnlyDictionary<string, byte[]> SavePackage(GameFile file);

	bool TrySavePackage(string path, out IReadOnlyDictionary<string, byte[]> package);

	bool TrySavePackage(GameFile file, out IReadOnlyDictionary<string, byte[]> package);

	Task<IReadOnlyDictionary<string, byte[]>> SavePackageAsync(string path);

	Task<IReadOnlyDictionary<string, byte[]>> SavePackageAsync(GameFile file);

	Task<IReadOnlyDictionary<string, byte[]>?> TrySavePackageAsync(string path);

	Task<IReadOnlyDictionary<string, byte[]>?> TrySavePackageAsync(GameFile file);

	UObject LoadObject(string? objectPath);

	bool TryLoadObject(string? objectPath, out UObject export);

	T LoadObject<T>(string? objectPath) where T : UObject;

	bool TryLoadObject<T>(string? objectPath, out T export) where T : UObject;

	Task<UObject> LoadObjectAsync(string? objectPath);

	Task<UObject?> TryLoadObjectAsync(string? objectPath);

	Task<T> LoadObjectAsync<T>(string? objectPath) where T : UObject;

	Task<T?> TryLoadObjectAsync<T>(string? objectPath) where T : UObject;

	IEnumerable<UObject> LoadObjectExports(string? objectPath);
}
