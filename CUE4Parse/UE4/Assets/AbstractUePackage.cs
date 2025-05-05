using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CUE4Parse.FileProvider;
using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.UObject;
using Serilog;

namespace CUE4Parse.UE4.Assets;

public abstract class AbstractUePackage : UObject, IPackage
{
	public IFileProvider? Provider { get; }

	public TypeMappings? Mappings { get; }

	public abstract FPackageFileSummary Summary { get; }

	public abstract FNameEntrySerialized[] NameMap { get; }

	public abstract Lazy<UObject>[] ExportsLazy { get; }

	public abstract bool IsFullyLoaded { get; }

	public override bool IsNameStableForNetworking()
	{
		return true;
	}

	public AbstractUePackage(string name, IFileProvider? provider, TypeMappings? mappings)
	{
		base.Name = name;
		Provider = provider;
		Mappings = mappings;
		Flags |= EObjectFlags.RF_WasLoaded;
	}

	protected static UObject ConstructObject(UStruct? struc)
	{
		UObject uObject = null;
		for (UStruct uStruct = struc; uStruct != null; uStruct = uStruct.SuperStruct?.Load<UStruct>())
		{
			if (uStruct is UClass uClass)
			{
				uObject = uClass.ConstructObject();
				if (uObject != null)
				{
					break;
				}
			}
		}
		if (uObject == null)
		{
			uObject = new UObject();
		}
		uObject.Class = struc;
		uObject.Flags |= EObjectFlags.RF_WasLoaded;
		return uObject;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void DeserializeObject(UObject obj, FAssetArchive Ar, long serialSize)
	{
		long validPos = Ar.Position + serialSize;
		try
		{
			obj.Deserialize(Ar, validPos);
		}
		catch (Exception ex)
		{
			if (Globals.FatalObjectSerializationErrors)
			{
				throw new ParserException("Could not read " + obj.ExportType + " correctly", ex);
			}
			Log.Error(ex, "Could not read {0} correctly", obj.ExportType);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasFlags(EPackageFlags flags)
	{
		return Summary.PackageFlags.HasFlag(flags);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public abstract UObject? GetExportOrNull(string name, StringComparison comparisonType = StringComparison.Ordinal);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T? GetExportOrNull<T>(string name, StringComparison comparisonType = StringComparison.Ordinal) where T : UObject
	{
		return GetExportOrNull(name, comparisonType) as T;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public UObject GetExport(string name, StringComparison comparisonType = StringComparison.Ordinal)
	{
		return GetExportOrNull(name, comparisonType) ?? throw new NullReferenceException($"Package '{base.Name}' does not have an export with the name '{name}'");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetExport<T>(string name, StringComparison comparisonType = StringComparison.Ordinal) where T : UObject
	{
		return GetExportOrNull<T>(name, comparisonType) ?? throw new NullReferenceException($"Package '{base.Name}' does not have an export with the name '{name} and type {typeof(T).Name}'");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public UObject? GetExport(int index)
	{
		if (index >= ExportsLazy.Length)
		{
			return null;
		}
		return ExportsLazy[index].Value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public IEnumerable<UObject> GetExports()
	{
		return ExportsLazy.Select((Lazy<UObject> x) => x.Value);
	}

	public Lazy<UObject>? FindObject(FPackageIndex? index)
	{
		if (index == null || index.IsNull)
		{
			return null;
		}
		if (index.IsImport)
		{
			return ResolvePackageIndex(index)?.Object;
		}
		return ExportsLazy[index.Index - 1];
	}

	public abstract ResolvedObject? ResolvePackageIndex(FPackageIndex? index);

	public override string ToString()
	{
		return base.Name;
	}
}
