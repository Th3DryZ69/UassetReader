using System;
using System.Collections.Generic;
using CUE4Parse.FileProvider;
using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets;

public interface IPackage
{
	string Name { get; set; }

	IFileProvider? Provider { get; }

	TypeMappings? Mappings { get; }

	FPackageFileSummary Summary { get; }

	FNameEntrySerialized[] NameMap { get; }

	Lazy<UObject>[] ExportsLazy { get; }

	bool IsFullyLoaded { get; }

	bool HasFlags(EPackageFlags flags);

	UObject? GetExportOrNull(string name, StringComparison comparisonType = StringComparison.Ordinal);

	T? GetExportOrNull<T>(string name, StringComparison comparisonType = StringComparison.Ordinal) where T : UObject;

	UObject GetExport(string name, StringComparison comparisonType = StringComparison.Ordinal);

	T GetExport<T>(string name, StringComparison comparisonType = StringComparison.Ordinal) where T : UObject;

	Lazy<UObject>? FindObject(FPackageIndex? index);

	ResolvedObject? ResolvePackageIndex(FPackageIndex? index);

	UObject? GetExport(int index);

	IEnumerable<UObject> GetExports();
}
