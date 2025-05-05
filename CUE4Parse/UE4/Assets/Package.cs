#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CUE4Parse.FileProvider;
using CUE4Parse.GameTypes.ACE7.Encryption;
using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;
using Serilog;

namespace CUE4Parse.UE4.Assets;

[SkipObjectRegistration]
public sealed class Package : AbstractUePackage
{
	private class ResolvedExportObject : ResolvedObject
	{
		private readonly FObjectExport _export;

		public override FName Name => _export?.ObjectName ?? ((FName)"None");

		public override ResolvedObject Outer => Package.ResolvePackageIndex(_export.OuterIndex) ?? new ResolvedLoadedObject((UObject)Package);

		public override ResolvedObject? Class => Package.ResolvePackageIndex(_export.ClassIndex);

		public override ResolvedObject? Super => Package.ResolvePackageIndex(_export.SuperIndex);

		public override Lazy<UObject> Object => _export.ExportObject;

		public ResolvedExportObject(int exportIndex, Package package)
			: base(package, exportIndex)
		{
			_export = package.ExportMap[exportIndex];
		}
	}

	private class ResolvedImportObject : ResolvedObject
	{
		private readonly FObjectImport _import;

		public override FName Name => _import.ObjectName;

		public override ResolvedObject? Outer => Package.ResolvePackageIndex(_import.OuterIndex);

		public override ResolvedObject Class => new ResolvedLoadedObject(new UScriptClass(_import.ClassName.Text));

		public override Lazy<UObject>? Object
		{
			get
			{
				if (!(_import.ClassName.Text == "Class"))
				{
					return null;
				}
				return new Lazy<UObject>(() => new UScriptClass(Name.Text));
			}
		}

		public ResolvedImportObject(FObjectImport import, Package package)
			: base(package)
		{
			_import = import;
		}
	}

	private class ExportLoader
	{
		private Package _package;

		private FObjectExport _export;

		private FAssetArchive _archive;

		private UObject _object;

		private List<LoadDependency>? _dependencies;

		private LoadPhase _phase;

		public Lazy<UObject> Lazy;

		public ExportLoader(Package package, FObjectExport export, FAssetArchive archive)
		{
			_package = package;
			_export = export;
			_archive = archive;
			Lazy = new Lazy<UObject>(delegate
			{
				Fire(LoadPhase.Serialize);
				return _object;
			});
			export.ExportObject = Lazy;
		}

		private void EnsureDependencies()
		{
			if (_dependencies != null)
			{
				return;
			}
			_dependencies = new List<LoadDependency>();
			int firstExportDependency = _export.FirstExportDependency;
			if (firstExportDependency >= 0)
			{
				for (int num = _export.SerializationBeforeSerializationDependencies; num > 0; num--)
				{
					FPackageIndex index = _package.PreloadDependencies[firstExportDependency++];
					_dependencies.Add(new LoadDependency(LoadPhase.Serialize, LoadPhase.Serialize, ResolveLoader(index)));
				}
				for (int num2 = _export.CreateBeforeSerializationDependencies; num2 > 0; num2--)
				{
					FPackageIndex index2 = _package.PreloadDependencies[firstExportDependency++];
					_dependencies.Add(new LoadDependency(LoadPhase.Serialize, LoadPhase.Create, ResolveLoader(index2)));
				}
				for (int num3 = _export.SerializationBeforeCreateDependencies; num3 > 0; num3--)
				{
					FPackageIndex index3 = _package.PreloadDependencies[firstExportDependency++];
					_dependencies.Add(new LoadDependency(LoadPhase.Create, LoadPhase.Serialize, ResolveLoader(index3)));
				}
				for (int num4 = _export.CreateBeforeCreateDependencies; num4 > 0; num4--)
				{
					FPackageIndex index4 = _package.PreloadDependencies[firstExportDependency++];
					_dependencies.Add(new LoadDependency(LoadPhase.Create, LoadPhase.Create, ResolveLoader(index4)));
				}
			}
			else
			{
				_dependencies.Add(new LoadDependency(LoadPhase.Create, LoadPhase.Create, ResolveLoader(_export.OuterIndex)));
			}
		}

		private ExportLoader? ResolveLoader(FPackageIndex index)
		{
			if (index.IsExport)
			{
				return _package._exportLoaders[index.Index - 1];
			}
			return null;
		}

		private void Fire(LoadPhase untilPhase)
		{
			if (untilPhase >= LoadPhase.Create && _phase <= LoadPhase.Create)
			{
				FireDependencies(LoadPhase.Create);
				Create();
			}
			if (untilPhase >= LoadPhase.Serialize && _phase <= LoadPhase.Serialize)
			{
				FireDependencies(LoadPhase.Serialize);
				Serialize();
			}
		}

		private void FireDependencies(LoadPhase phase)
		{
			EnsureDependencies();
			foreach (LoadDependency dependency in _dependencies)
			{
				if (dependency.FromPhase == phase)
				{
					dependency.Target?.Fire(dependency.ToPhase);
				}
			}
		}

		private void Create()
		{
			Trace.Assert(_phase == LoadPhase.Create);
			_phase = LoadPhase.Serialize;
			_object = AbstractUePackage.ConstructObject(_package.ResolvePackageIndex(_export.ClassIndex)?.Object?.Value as UStruct);
			_object.Name = _export.ObjectName.Text;
			if (!_export.OuterIndex.IsNull)
			{
				Trace.Assert(_export.OuterIndex.IsExport, "Outer imports are not yet supported");
				_object.Outer = _package._exportLoaders[_export.OuterIndex.Index - 1]._object;
			}
			else
			{
				_object.Outer = _package;
			}
			_object.Super = _package.ResolvePackageIndex(_export.SuperIndex) as ResolvedExportObject;
			_object.Template = _package.ResolvePackageIndex(_export.TemplateIndex) as ResolvedExportObject;
			_object.Flags |= (EObjectFlags)_export.ObjectFlags;
		}

		private void Serialize()
		{
			Trace.Assert(_phase == LoadPhase.Serialize);
			_phase = LoadPhase.Complete;
			FAssetArchive fAssetArchive = (FAssetArchive)_archive.Clone();
			fAssetArchive.SeekAbsolute(_export.SerialOffset, SeekOrigin.Begin);
			AbstractUePackage.DeserializeObject(_object, fAssetArchive, _export.SerialSize);
			_object.Flags |= EObjectFlags.RF_LoadCompleted;
			_object.PostLoad();
		}
	}

	private class LoadDependency
	{
		public LoadPhase FromPhase;

		public LoadPhase ToPhase;

		public ExportLoader? Target;

		public LoadDependency(LoadPhase fromPhase, LoadPhase toPhase, ExportLoader? target)
		{
			FromPhase = fromPhase;
			ToPhase = toPhase;
			Target = target;
		}
	}

	private enum LoadPhase
	{
		Create,
		Serialize,
		Complete
	}

	private ExportLoader[] _exportLoaders;

	public override FPackageFileSummary Summary { get; }

	public override FNameEntrySerialized[] NameMap { get; }

	public FObjectImport[] ImportMap { get; }

	public FObjectExport[] ExportMap { get; }

	public FPackageIndex[][]? DependsMap { get; }

	public FPackageIndex[]? PreloadDependencies { get; }

	public override Lazy<UObject>[] ExportsLazy => ExportMap.Select((FObjectExport it) => it.ExportObject).ToArray();

	public override bool IsFullyLoaded { get; }

	public Package(FArchive uasset, FArchive? uexp, Lazy<FArchive?>? ubulk = null, Lazy<FArchive?>? uptnl = null, IFileProvider? provider = null, TypeMappings? mappings = null, bool useLazySerialization = true)
		: base(uasset.Name.SubstringBeforeLast('.'), provider, mappings)
	{
		Package package = this;
		uasset.Versions = (VersionContainer)uasset.Versions.Clone();
		ACE7XORKey key = null;
		ACE7Decrypt aCE7Decrypt = null;
		FAssetArchive uassetAr;
		if (uasset.Game == EGame.GAME_AceCombat7)
		{
			aCE7Decrypt = new ACE7Decrypt();
			uassetAr = new FAssetArchive(aCE7Decrypt.DecryptUassetArchive(uasset, out key), this);
		}
		else
		{
			uassetAr = new FAssetArchive(uasset, this);
		}
		Summary = new FPackageFileSummary(uassetAr);
		uassetAr.SeekAbsolute(Summary.NameOffset, SeekOrigin.Begin);
		NameMap = new FNameEntrySerialized[Summary.NameCount];
		uassetAr.ReadArray(NameMap, () => new FNameEntrySerialized(uassetAr));
		uassetAr.SeekAbsolute(Summary.ImportOffset, SeekOrigin.Begin);
		ImportMap = new FObjectImport[Summary.ImportCount];
		uassetAr.ReadArray(ImportMap, () => new FObjectImport(uassetAr));
		uassetAr.SeekAbsolute(Summary.ExportOffset, SeekOrigin.Begin);
		ExportMap = new FObjectExport[Summary.ExportCount];
		uassetAr.ReadArray(ExportMap, () => new FObjectExport(uassetAr));
		if (!useLazySerialization && Summary.DependsOffset > 0 && Summary.ExportCount > 0)
		{
			uassetAr.SeekAbsolute(Summary.DependsOffset, SeekOrigin.Begin);
			DependsMap = uassetAr.ReadArray(Summary.ExportCount, () => uassetAr.ReadArray(() => new FPackageIndex(uassetAr)));
		}
		if (!useLazySerialization && Summary.PreloadDependencyCount > 0 && Summary.PreloadDependencyOffset > 0)
		{
			uassetAr.SeekAbsolute(Summary.PreloadDependencyOffset, SeekOrigin.Begin);
			PreloadDependencies = uassetAr.ReadArray(Summary.PreloadDependencyCount, () => new FPackageIndex(uassetAr));
		}
		FAssetArchive uexpAr;
		if (uexp != null)
		{
			if (uasset.Game == EGame.GAME_AceCombat7 && aCE7Decrypt != null && key != null)
			{
				uexpAr = new FAssetArchive(aCE7Decrypt.DecryptUexpArchive(uexp, key), this, (int)uassetAr.Length);
			}
			else
			{
				uexpAr = new FAssetArchive(uexp, this, (int)uassetAr.Length);
			}
		}
		else
		{
			uexpAr = uassetAr;
		}
		if (ubulk != null)
		{
			int bulkDataStartOffset = Summary.BulkDataStartOffset;
			uexpAr.AddPayload(PayloadType.UBULK, bulkDataStartOffset, ubulk);
		}
		if (uptnl != null)
		{
			int bulkDataStartOffset2 = Summary.BulkDataStartOffset;
			uexpAr.AddPayload(PayloadType.UPTNL, bulkDataStartOffset2, uptnl);
		}
		if (HasFlags(EPackageFlags.PKG_UnversionedProperties) && mappings == null)
		{
			throw new ParserException("Package has unversioned properties but mapping file is missing, can't serialize");
		}
		if (useLazySerialization)
		{
			FObjectExport[] exportMap = ExportMap;
			foreach (FObjectExport export in exportMap)
			{
				export.ExportObject = new Lazy<UObject>(delegate
				{
					UObject uObject = AbstractUePackage.ConstructObject(package.ResolvePackageIndex(export.ClassIndex)?.Object?.Value as UStruct);
					uObject.Name = export.ObjectName.Text;
					uObject.Outer = (package.ResolvePackageIndex(export.OuterIndex) as ResolvedExportObject)?.Object.Value ?? package;
					uObject.Super = package.ResolvePackageIndex(export.SuperIndex) as ResolvedExportObject;
					uObject.Template = package.ResolvePackageIndex(export.TemplateIndex) as ResolvedExportObject;
					uObject.Flags |= (EObjectFlags)export.ObjectFlags;
					FAssetArchive fAssetArchive = (FAssetArchive)uexpAr.Clone();
					fAssetArchive.SeekAbsolute(export.SerialOffset, SeekOrigin.Begin);
					AbstractUePackage.DeserializeObject(uObject, fAssetArchive, export.SerialSize);
					uObject.Flags |= EObjectFlags.RF_LoadCompleted;
					uObject.PostLoad();
					return uObject;
				});
			}
		}
		else
		{
			_exportLoaders = new ExportLoader[ExportMap.Length];
			for (int num2 = 0; num2 < ExportMap.Length; num2++)
			{
				_exportLoaders[num2] = new ExportLoader(this, ExportMap[num2], uexpAr);
			}
		}
		IsFullyLoaded = true;
	}

	public Package(FArchive uasset, FArchive? uexp, FArchive? ubulk = null, FArchive? uptnl = null, IFileProvider? provider = null, TypeMappings? mappings = null, bool useLazySerialization = true)
		: this(uasset, uexp, (ubulk != null) ? new Lazy<FArchive>(() => ubulk) : null, (uptnl != null) ? new Lazy<FArchive>(() => uptnl) : null, provider, mappings, useLazySerialization)
	{
	}

	public Package(string name, byte[] uasset, byte[]? uexp, byte[]? ubulk = null, byte[]? uptnl = null, IFileProvider? provider = null, bool useLazySerialization = true)
		: this(new FByteArchive(name + ".uasset", uasset), (uexp != null) ? new FByteArchive(name + ".uexp", uexp) : null, (ubulk != null) ? new FByteArchive(name + ".ubulk", ubulk) : null, (uptnl != null) ? new FByteArchive(name + ".uptnl", uptnl) : null, provider, null, useLazySerialization)
	{
	}

	public override UObject? GetExportOrNull(string name, StringComparison comparisonType = StringComparison.Ordinal)
	{
		try
		{
			return ExportMap.FirstOrDefault((FObjectExport it) => it.ObjectName.Text.Equals(name, comparisonType))?.ExportObject.Value;
		}
		catch (Exception exception)
		{
			Log.Debug(exception, "Failed to get export object");
			return null;
		}
	}

	public override ResolvedObject? ResolvePackageIndex(FPackageIndex? index)
	{
		if (index == null || index.IsNull)
		{
			return null;
		}
		if (index.IsImport && -index.Index - 1 < ImportMap.Length)
		{
			return ResolveImport(index);
		}
		if (index.IsExport && index.Index - 1 < ExportMap.Length)
		{
			return new ResolvedExportObject(index.Index - 1, this);
		}
		return null;
	}

	private ResolvedObject? ResolveImport(FPackageIndex importIndex)
	{
		FObjectImport fObjectImport = ImportMap[-importIndex.Index - 1];
		FPackageIndex fPackageIndex = importIndex;
		FObjectImport fObjectImport2;
		while (true)
		{
			fObjectImport2 = ImportMap[-fPackageIndex.Index - 1];
			if (fObjectImport2.OuterIndex.IsNull)
			{
				break;
			}
			fPackageIndex = fObjectImport2.OuterIndex;
		}
		fObjectImport2 = ImportMap[-fPackageIndex.Index - 1];
		if (fObjectImport2.ObjectName.Text.StartsWith("/Script/"))
		{
			return new ResolvedImportObject(fObjectImport, this);
		}
		if (base.Provider == null)
		{
			return null;
		}
		Package package = null;
		if (base.Provider.TryLoadPackage(fObjectImport2.ObjectName.Text, out IPackage package2))
		{
			package = package2 as Package;
		}
		if (package == null)
		{
			return new ResolvedImportObject(fObjectImport, this);
		}
		string text = null;
		if (fPackageIndex != fObjectImport.OuterIndex && fObjectImport.OuterIndex.IsImport)
		{
			_ = ImportMap[-fObjectImport.OuterIndex.Index - 1];
			text = ResolveImport(fObjectImport.OuterIndex)?.GetPathName();
			if (text == null)
			{
				return new ResolvedImportObject(fObjectImport, this);
			}
		}
		for (int i = 0; i < package.ExportMap.Length; i++)
		{
			FObjectExport fObjectExport = package.ExportMap[i];
			if (!(fObjectExport.ObjectName.Text != fObjectImport.ObjectName.Text) && package.ResolvePackageIndex(fObjectExport.OuterIndex)?.GetPathName() == text)
			{
				return new ResolvedExportObject(i, package);
			}
		}
		return new ResolvedImportObject(fObjectImport, this);
	}
}
