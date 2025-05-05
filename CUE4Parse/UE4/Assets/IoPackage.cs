using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CUE4Parse.FileProvider;
using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.IO;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;
using Serilog;

namespace CUE4Parse.UE4.Assets;

[SkipObjectRegistration]
public sealed class IoPackage : AbstractUePackage
{
	private class ResolvedExportObject : ResolvedObject
	{
		public FExportMapEntry ExportMapEntry;

		public Lazy<UObject> ExportObject;

		public override FName Name => ((IoPackage)Package).CreateFNameFromMappedName(ExportMapEntry.ObjectName);

		public override ResolvedObject Outer => ((IoPackage)Package).ResolveObjectIndex(ExportMapEntry.OuterIndex) ?? new ResolvedLoadedObject((UObject)Package);

		public override ResolvedObject? Class => ((IoPackage)Package).ResolveObjectIndex(ExportMapEntry.ClassIndex);

		public override ResolvedObject? Super => ((IoPackage)Package).ResolveObjectIndex(ExportMapEntry.SuperIndex);

		public override Lazy<UObject> Object => ExportObject;

		public ResolvedExportObject(int exportIndex, IoPackage package)
			: base(package, exportIndex)
		{
			if (exportIndex < package.ExportMap.Length)
			{
				ExportMapEntry = package.ExportMap[exportIndex];
				ExportObject = package.ExportsLazy[exportIndex];
			}
		}
	}

	private class ResolvedScriptObject : ResolvedObject
	{
		public FScriptObjectEntry ScriptImport;

		public override FName Name => ((IoPackage)Package).CreateFNameFromMappedName(ScriptImport.ObjectName);

		public override ResolvedObject? Outer => ((IoPackage)Package).ResolveObjectIndex(ScriptImport.OuterIndex);

		public override ResolvedObject Class => new ResolvedLoadedObject(new UScriptClass("Class"));

		public override Lazy<UObject> Object => new Lazy<UObject>(() => new UScriptClass(Name.Text));

		public ResolvedScriptObject(FScriptObjectEntry scriptImport, IoPackage package)
			: base(package)
		{
			ScriptImport = scriptImport;
		}
	}

	public readonly IoGlobalData GlobalData;

	public readonly ulong[]? ImportedPublicExportHashes;

	public readonly FPackageObjectIndex[] ImportMap;

	public readonly FExportMapEntry[] ExportMap;

	public readonly FBulkDataMapEntry[] BulkDataMap;

	public readonly Lazy<IoPackage?[]> ImportedPackages;

	public override FPackageFileSummary Summary { get; }

	public override FNameEntrySerialized[] NameMap { get; }

	public override Lazy<UObject>[] ExportsLazy { get; }

	public override bool IsFullyLoaded { get; }

	public unsafe IoPackage(FArchive uasset, IoGlobalData globalData, FIoContainerHeader? containerHeader = null, Lazy<FArchive?>? ubulk = null, Lazy<FArchive?>? uptnl = null, IFileProvider? provider = null, TypeMappings? mappings = null)
		: base(uasset.Name.SubstringBeforeLast('.'), provider, mappings)
	{
		IoPackage ioPackage = this;
		GlobalData = globalData;
		FAssetArchive uassetAr = new FAssetArchive(uasset, this);
		FExportBundleHeader[] bundleHeadersArray = Array.Empty<FExportBundleHeader>();
		FExportBundleEntry[] bundleEntriesArray;
		FPackageId[] importedPackageIds;
		int num4;
		if (uassetAr.Game >= EGame.GAME_UE5_0)
		{
			FZenPackageSummary fZenPackageSummary = new FZenPackageSummary(uassetAr);
			Summary = new FPackageFileSummary
			{
				PackageFlags = fZenPackageSummary.PackageFlags,
				TotalHeaderSize = fZenPackageSummary.GraphDataOffset + (int)fZenPackageSummary.HeaderSize,
				ExportCount = (fZenPackageSummary.ExportBundleEntriesOffset - fZenPackageSummary.ExportMapOffset) / 72,
				ImportCount = (fZenPackageSummary.ExportMapOffset - fZenPackageSummary.ImportMapOffset) / 8
			};
			if (fZenPackageSummary.bHasVersioningInfo != 0)
			{
				FZenPackageVersioningInfo fZenPackageVersioningInfo = new FZenPackageVersioningInfo(uassetAr);
				Summary.FileVersionUE = fZenPackageVersioningInfo.PackageVersion;
				Summary.FileVersionLicenseeUE = (EUnrealEngineObjectLicenseeUEVersion)fZenPackageVersioningInfo.LicenseeVersion;
				Summary.CustomVersionContainer = fZenPackageVersioningInfo.CustomVersions;
				if (!uassetAr.Versions.bExplicitVer)
				{
					uassetAr.Versions.Ver = fZenPackageVersioningInfo.PackageVersion;
					uassetAr.Versions.CustomVersions = fZenPackageVersioningInfo.CustomVersions;
				}
			}
			else
			{
				Summary.bUnversioned = true;
			}
			NameMap = FNameEntrySerialized.LoadNameBatch(uassetAr);
			Summary.NameCount = NameMap.Length;
			base.Name = CreateFNameFromMappedName(fZenPackageSummary.Name).Text;
			FFilePackageStoreEntry fFilePackageStoreEntry = null;
			if (containerHeader != null)
			{
				FPackageId value = FPackageId.FromName(base.Name);
				int num = Array.IndexOf(containerHeader.PackageIds, value);
				if (num != -1)
				{
					fFilePackageStoreEntry = containerHeader.StoreEntries[num];
				}
				else
				{
					int num2 = Array.IndexOf(containerHeader.OptionalSegmentPackageIds, value);
					if (num2 != -1)
					{
						fFilePackageStoreEntry = containerHeader.OptionalSegmentStoreEntries[num2];
					}
					else
					{
						Log.Warning("Couldn't find store entry for package {0}, its data will not be fully read", base.Name);
					}
				}
			}
			BulkDataMap = Array.Empty<FBulkDataMapEntry>();
			if (uassetAr.Ver >= EUnrealEngineObjectUE5Version.DATA_RESOURCES)
			{
				ulong num3 = uassetAr.Read<ulong>();
				BulkDataMap = uassetAr.ReadArray<FBulkDataMapEntry>((int)(num3 / 32));
			}
			uassetAr.Position = fZenPackageSummary.ImportedPublicExportHashesOffset;
			ImportedPublicExportHashes = uassetAr.ReadArray<ulong>((fZenPackageSummary.ImportMapOffset - fZenPackageSummary.ImportedPublicExportHashesOffset) / 8);
			uassetAr.Position = fZenPackageSummary.ImportMapOffset;
			ImportMap = uasset.ReadArray<FPackageObjectIndex>(Summary.ImportCount);
			uassetAr.Position = fZenPackageSummary.ExportMapOffset;
			ExportMap = uasset.ReadArray(Summary.ExportCount, () => new FExportMapEntry(uassetAr));
			ExportsLazy = new Lazy<UObject>[Summary.ExportCount];
			uassetAr.Position = fZenPackageSummary.ExportBundleEntriesOffset;
			bundleEntriesArray = uassetAr.ReadArray<FExportBundleEntry>(Summary.ExportCount * 2);
			if (uassetAr.Game < EGame.GAME_UE5_3)
			{
				uassetAr.Position = fZenPackageSummary.GraphDataOffset;
				int length = fFilePackageStoreEntry?.ExportBundleCount ?? 1;
				bundleHeadersArray = uassetAr.ReadArray<FExportBundleHeader>(length);
			}
			importedPackageIds = fFilePackageStoreEntry?.ImportedPackages ?? Array.Empty<FPackageId>();
			num4 = (int)fZenPackageSummary.HeaderSize;
		}
		else
		{
			FPackageSummary fPackageSummary = uassetAr.Read<FPackageSummary>();
			Summary = new FPackageFileSummary
			{
				PackageFlags = fPackageSummary.PackageFlags,
				TotalHeaderSize = fPackageSummary.GraphDataOffset + fPackageSummary.GraphDataSize,
				NameCount = fPackageSummary.NameMapHashesSize / 8 - 1,
				ExportCount = (fPackageSummary.ExportBundlesOffset - fPackageSummary.ExportMapOffset) / 72,
				ImportCount = (fPackageSummary.ExportMapOffset - fPackageSummary.ImportMapOffset) / 8,
				bUnversioned = true
			};
			uassetAr.Position = fPackageSummary.NameMapNamesOffset;
			NameMap = FNameEntrySerialized.LoadNameBatch(uassetAr, Summary.NameCount);
			base.Name = CreateFNameFromMappedName(fPackageSummary.Name).Text;
			uassetAr.Position = fPackageSummary.ImportMapOffset;
			ImportMap = uasset.ReadArray<FPackageObjectIndex>(Summary.ImportCount);
			uassetAr.Position = fPackageSummary.ExportMapOffset;
			ExportMap = uasset.ReadArray(Summary.ExportCount, () => new FExportMapEntry(uassetAr));
			ExportsLazy = new Lazy<UObject>[Summary.ExportCount];
			uassetAr.Position = fPackageSummary.ExportBundlesOffset;
			LoadExportBundles(uassetAr, fPackageSummary.GraphDataOffset - fPackageSummary.ExportBundlesOffset, out bundleHeadersArray, out bundleEntriesArray);
			uassetAr.Position = fPackageSummary.GraphDataOffset;
			importedPackageIds = LoadGraphData(uassetAr);
			num4 = fPackageSummary.GraphDataOffset + fPackageSummary.GraphDataSize;
		}
		ImportedPackages = new Lazy<IoPackage[]>((provider != null) ? ((Func<IoPackage[]>)delegate
		{
			IoPackage[] array2 = new IoPackage[importedPackageIds.Length];
			for (int i = 0; i < importedPackageIds.Length; i++)
			{
				provider.TryLoadPackage(importedPackageIds[i], out array2[i]);
			}
			return array2;
		}) : new Func<IoPackage[]>(Array.Empty<IoPackage>));
		if (ubulk != null)
		{
			uassetAr.AddPayload(PayloadType.UBULK, Summary.BulkDataStartOffset, ubulk);
		}
		if (uptnl != null)
		{
			uassetAr.AddPayload(PayloadType.UPTNL, Summary.BulkDataStartOffset, uptnl);
		}
		if (HasFlags(EPackageFlags.PKG_UnversionedProperties) && mappings == null)
		{
			throw new ParserException("Package has unversioned properties but mapping file is missing, can't serialize");
		}
		int num5 = num4;
		if (uassetAr.Game >= EGame.GAME_UE5_3)
		{
			fixed (FExportBundleEntry* ptr = bundleEntriesArray)
			{
				FExportBundleEntry* ptr2 = ptr;
				int num6 = bundleEntriesArray.Length;
				for (FExportBundleEntry* ptr3 = ptr2 + num6; ptr2 < ptr3; ptr2++)
				{
					if (ptr2->CommandType == EExportCommandType.ExportCommandType_Serialize)
					{
						uint localExportIndex = ptr2->LocalExportIndex;
						FExportMapEntry export = ExportMap[localExportIndex];
						int localExportDataOffset = num5;
						ExportsLazy[localExportIndex] = new Lazy<UObject>(delegate
						{
							UObject uObject = AbstractUePackage.ConstructObject(ioPackage.ResolveObjectIndex(export.ClassIndex)?.Object?.Value as UStruct);
							uObject.Name = ioPackage.CreateFNameFromMappedName(export.ObjectName).Text;
							uObject.Outer = (ioPackage.ResolveObjectIndex(export.OuterIndex) as ResolvedExportObject)?.ExportObject.Value ?? ioPackage;
							uObject.Super = ioPackage.ResolveObjectIndex(export.SuperIndex) as ResolvedExportObject;
							uObject.Template = ioPackage.ResolveObjectIndex(export.TemplateIndex) as ResolvedExportObject;
							uObject.Flags |= export.ObjectFlags;
							FAssetArchive fAssetArchive = (FAssetArchive)uassetAr.Clone();
							fAssetArchive.AbsoluteOffset = (int)export.CookedSerialOffset - localExportDataOffset;
							fAssetArchive.Position = localExportDataOffset;
							AbstractUePackage.DeserializeObject(uObject, fAssetArchive, (long)export.CookedSerialSize);
							uObject.Flags |= EObjectFlags.RF_LoadCompleted;
							uObject.PostLoad();
							return uObject;
						});
						num5 += (int)export.CookedSerialSize;
					}
				}
			}
		}
		else
		{
			FExportBundleHeader[] array = bundleHeadersArray;
			for (int num7 = 0; num7 < array.Length; num7++)
			{
				FExportBundleHeader fExportBundleHeader = array[num7];
				for (uint num8 = 0u; num8 < fExportBundleHeader.EntryCount; num8++)
				{
					FExportBundleEntry fExportBundleEntry = bundleEntriesArray[fExportBundleHeader.FirstEntryIndex + num8];
					if (fExportBundleEntry.CommandType == EExportCommandType.ExportCommandType_Serialize)
					{
						uint localExportIndex2 = fExportBundleEntry.LocalExportIndex;
						FExportMapEntry export2 = ExportMap[localExportIndex2];
						int localExportDataOffset2 = num5;
						ExportsLazy[localExportIndex2] = new Lazy<UObject>(delegate
						{
							UObject uObject = AbstractUePackage.ConstructObject(ioPackage.ResolveObjectIndex(export2.ClassIndex)?.Object?.Value as UStruct);
							uObject.Name = ioPackage.CreateFNameFromMappedName(export2.ObjectName).Text;
							uObject.Outer = (ioPackage.ResolveObjectIndex(export2.OuterIndex) as ResolvedExportObject)?.ExportObject.Value ?? ioPackage;
							uObject.Super = ioPackage.ResolveObjectIndex(export2.SuperIndex) as ResolvedExportObject;
							uObject.Template = ioPackage.ResolveObjectIndex(export2.TemplateIndex) as ResolvedExportObject;
							uObject.Flags |= export2.ObjectFlags;
							FAssetArchive fAssetArchive = (FAssetArchive)uassetAr.Clone();
							fAssetArchive.AbsoluteOffset = (int)export2.CookedSerialOffset - localExportDataOffset2;
							fAssetArchive.Position = localExportDataOffset2;
							AbstractUePackage.DeserializeObject(uObject, fAssetArchive, (long)export2.CookedSerialSize);
							uObject.Flags |= EObjectFlags.RF_LoadCompleted;
							uObject.PostLoad();
							return uObject;
						});
						num5 += (int)export2.CookedSerialSize;
					}
				}
			}
		}
		Summary.BulkDataStartOffset = num5;
		IsFullyLoaded = true;
	}

	public IoPackage(FArchive uasset, IoGlobalData globalData, FIoContainerHeader? containerHeader = null, FArchive? ubulk = null, FArchive? uptnl = null, IFileProvider? provider = null, TypeMappings? mappings = null)
		: this(uasset, globalData, containerHeader, (ubulk != null) ? new Lazy<FArchive>(() => ubulk) : null, (uptnl != null) ? new Lazy<FArchive>(() => uptnl) : null, provider, mappings)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private FName CreateFNameFromMappedName(FMappedName mappedName)
	{
		return new FName(mappedName, mappedName.IsGlobal ? GlobalData.GlobalNameMap : NameMap);
	}

	private void LoadExportBundles(FArchive Ar, int graphDataSize, out FExportBundleHeader[] bundleHeadersArray, out FExportBundleEntry[] bundleEntriesArray)
	{
		int num = graphDataSize / 8;
		int num2 = 0;
		List<FExportBundleHeader> list = new List<FExportBundleHeader>();
		while (num2 < num)
		{
			num--;
			FExportBundleHeader item = new FExportBundleHeader(Ar);
			num2 += (int)item.EntryCount;
			list.Add(item);
		}
		if (num2 != num)
		{
			throw new ParserException(Ar, $"FoundBundlesCount {num2} != RemainingBundleEntryCount {num}");
		}
		bundleHeadersArray = list.ToArray();
		bundleEntriesArray = Ar.ReadArray<FExportBundleEntry>(num2);
	}

	private FPackageId[] LoadGraphData(FArchive Ar)
	{
		int num = Ar.Read<int>();
		if (num == 0)
		{
			return Array.Empty<FPackageId>();
		}
		FPackageId[] array = new FPackageId[num];
		for (int i = 0; i < num; i++)
		{
			FPackageId fPackageId = Ar.Read<FPackageId>();
			int num2 = Ar.Read<int>();
			Ar.Position += num2 * 8;
			array[i] = fPackageId;
		}
		return array;
	}

	public override UObject? GetExportOrNull(string name, StringComparison comparisonType = StringComparison.Ordinal)
	{
		for (int i = 0; i < ExportMap.Length; i++)
		{
			FExportMapEntry fExportMapEntry = ExportMap[i];
			if (CreateFNameFromMappedName(fExportMapEntry.ObjectName).Text.Equals(name, comparisonType))
			{
				return ExportsLazy[i].Value;
			}
		}
		return null;
	}

	public override ResolvedObject? ResolvePackageIndex(FPackageIndex? index)
	{
		if (index == null || index.IsNull)
		{
			return null;
		}
		if (index.IsImport && -index.Index - 1 < ImportMap.Length)
		{
			return ResolveObjectIndex(ImportMap[-index.Index - 1]);
		}
		if (index.IsExport && index.Index - 1 < ExportMap.Length)
		{
			return new ResolvedExportObject(index.Index - 1, this);
		}
		return null;
	}

	public ResolvedObject? ResolveObjectIndex(FPackageObjectIndex index)
	{
		if (index.IsNull)
		{
			return null;
		}
		if (index.IsExport)
		{
			return new ResolvedExportObject((int)index.AsExport, this);
		}
		if (index.IsScriptImport && GlobalData.ScriptObjectEntriesMap.TryGetValue(index, out var value))
		{
			return new ResolvedScriptObject(value, this);
		}
		if (index.IsPackageImport && base.Provider != null)
		{
			if (ImportedPublicExportHashes != null)
			{
				FPackageImportReference asPackageImportRef = index.AsPackageImportRef;
				IoPackage[] value2 = ImportedPackages.Value;
				if (asPackageImportRef.ImportedPackageIndex < value2.Length)
				{
					IoPackage ioPackage = value2[asPackageImportRef.ImportedPackageIndex];
					if (ioPackage != null)
					{
						for (int i = 0; i < ioPackage.ExportMap.Length; i++)
						{
							if (ioPackage.ExportMap[i].PublicExportHash == ImportedPublicExportHashes[asPackageImportRef.ImportedPublicExportHashIndex])
							{
								return new ResolvedExportObject(i, ioPackage);
							}
						}
					}
				}
			}
			else
			{
				IoPackage[] value3 = ImportedPackages.Value;
				foreach (IoPackage ioPackage2 in value3)
				{
					if (ioPackage2 == null)
					{
						continue;
					}
					for (int k = 0; k < ioPackage2.ExportMap.Length; k++)
					{
						if (ioPackage2.ExportMap[k].GlobalImportIndex == index)
						{
							return new ResolvedExportObject(k, ioPackage2);
						}
					}
				}
			}
		}
		if (Globals.WarnMissingImportPackage)
		{
			Log.Warning("Missing {0} import 0x{1:X} for package {2}", index.IsScriptImport ? "script" : "package", index.Value, base.Name);
		}
		return null;
	}
}
