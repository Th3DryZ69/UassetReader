using System;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Core.Serialization;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.UObject;

public class FPackageFileSummary
{
	public const uint PACKAGE_FILE_TAG = 2653586369u;

	public const uint PACKAGE_FILE_TAG_SWAPPED = 3246598814u;

	public const uint PACKAGE_FILE_TAG_ACE7 = 927286081u;

	private const uint PACKAGE_FILE_TAG_ONE = 6647407u;

	public readonly uint Tag;

	public FPackageFileVersion FileVersionUE;

	public EUnrealEngineObjectLicenseeUEVersion FileVersionLicenseeUE;

	public FCustomVersionContainer CustomVersionContainer;

	public EPackageFlags PackageFlags;

	public int TotalHeaderSize;

	public readonly string FolderName;

	public int NameCount;

	public readonly int NameOffset;

	public readonly int SoftObjectPathsCount;

	public readonly int SoftObjectPathsOffset;

	public readonly string? LocalizationId;

	public readonly int GatherableTextDataCount;

	public readonly int GatherableTextDataOffset;

	public int ExportCount;

	public readonly int ExportOffset;

	public int ImportCount;

	public readonly int ImportOffset;

	public readonly int DependsOffset;

	public readonly int SoftPackageReferencesCount;

	public readonly int SoftPackageReferencesOffset;

	public readonly int SearchableNamesOffset;

	public readonly int ThumbnailTableOffset;

	public readonly FGuid Guid;

	public readonly FGuid PersistentGuid;

	public readonly FGenerationInfo[] Generations;

	public readonly FEngineVersion? SavedByEngineVersion;

	public readonly FEngineVersion? CompatibleWithEngineVersion;

	public readonly ECompressionFlags CompressionFlags;

	public readonly int PackageSource;

	public bool bUnversioned;

	public readonly int AssetRegistryDataOffset;

	public int BulkDataStartOffset;

	public readonly int WorldTileInfoDataOffset;

	public readonly int[] ChunkIds;

	public readonly int PreloadDependencyCount;

	public readonly int PreloadDependencyOffset;

	public readonly int NamesReferencedFromExportDataCount;

	public readonly long PayloadTocOffset;

	public readonly int DataResourceOffset;

	public FPackageFileSummary()
	{
		CustomVersionContainer = new FCustomVersionContainer();
		FolderName = string.Empty;
		Generations = Array.Empty<FGenerationInfo>();
		ChunkIds = Array.Empty<int>();
	}

	internal FPackageFileSummary(FArchive Ar)
	{
		Tag = Ar.Read<uint>();
		int num = -8;
		if (Tag == 6647407)
		{
			Ar.Game = EGame.GAME_StateOfDecay2;
			Ar.Ver = Ar.Game.GetVersion();
			num = Ar.Read<int>();
			bUnversioned = true;
			FileVersionUE = Ar.Ver;
			CustomVersionContainer = new FCustomVersionContainer();
			FolderName = "None";
			PackageFlags = EPackageFlags.PKG_FilterEditorOnly;
		}
		else
		{
			if (Tag != 2653586369u && Tag != 3246598814u)
			{
				throw new ParserException($"Invalid uasset magic: 0x{Tag:X8} != 0x{-1641380927:X8}");
			}
			if (Tag == 3246598814u)
			{
				throw new ParserException("Byte swapping for packages not supported");
			}
			num = Ar.Read<int>();
			if (num >= 0)
			{
				throw new ParserException("Can't load legacy UE3 file");
			}
			if (num < -8)
			{
				FileVersionUE.Reset();
				FileVersionLicenseeUE = EUnrealEngineObjectLicenseeUEVersion.VER_LIC_NONE;
				throw new ParserException("Can't load legacy UE3 file");
			}
			if (num != -4)
			{
				Ar.Read<int>();
			}
			FileVersionUE.FileVersionUE4 = Ar.Read<int>();
			if (num <= -8)
			{
				FileVersionUE.FileVersionUE5 = Ar.Read<int>();
			}
			FileVersionLicenseeUE = Ar.Read<EUnrealEngineObjectLicenseeUEVersion>();
			CustomVersionContainer = ((num <= -2) ? new FCustomVersionContainer(Ar) : new FCustomVersionContainer());
			if (Ar.Versions.CustomVersions == null && CustomVersionContainer.Versions.Length != 0)
			{
				Ar.Versions.CustomVersions = CustomVersionContainer;
			}
			if (FileVersionUE.FileVersionUE4 == 0 && FileVersionUE.FileVersionUE5 == 0 && FileVersionLicenseeUE == EUnrealEngineObjectLicenseeUEVersion.VER_LIC_NONE)
			{
				bUnversioned = true;
				FileVersionUE = Ar.Ver;
				FileVersionLicenseeUE = EUnrealEngineObjectLicenseeUEVersion.VER_LIC_NONE;
			}
			else
			{
				bUnversioned = false;
				if (!Ar.Versions.bExplicitVer)
				{
					Ar.Ver = FileVersionUE;
				}
			}
			TotalHeaderSize = Ar.Read<int>();
			FolderName = Ar.ReadFString();
			PackageFlags = Ar.Read<EPackageFlags>();
		}
		NameCount = Ar.Read<int>();
		NameOffset = Ar.Read<int>();
		if (FileVersionUE >= EUnrealEngineObjectUE5Version.ADD_SOFTOBJECTPATH_LIST)
		{
			SoftObjectPathsCount = Ar.Read<int>();
			SoftObjectPathsOffset = Ar.Read<int>();
		}
		if (!PackageFlags.HasFlag(EPackageFlags.PKG_FilterEditorOnly) && FileVersionUE >= EUnrealEngineObjectUE4Version.ADDED_PACKAGE_SUMMARY_LOCALIZATION_ID)
		{
			LocalizationId = Ar.ReadFString();
		}
		if (FileVersionUE >= EUnrealEngineObjectUE4Version.SERIALIZE_TEXT_IN_PACKAGES)
		{
			GatherableTextDataCount = Ar.Read<int>();
			GatherableTextDataOffset = Ar.Read<int>();
		}
		ExportCount = Ar.Read<int>();
		ExportOffset = Ar.Read<int>();
		ImportCount = Ar.Read<int>();
		ImportOffset = Ar.Read<int>();
		DependsOffset = Ar.Read<int>();
		if (FileVersionUE < EUnrealEngineObjectUE4Version.OLDEST_LOADABLE_PACKAGE || FileVersionUE > EUnrealEngineObjectUE4Version.CORRECT_LICENSEE_FLAG)
		{
			Generations = Array.Empty<FGenerationInfo>();
			ChunkIds = Array.Empty<int>();
			return;
		}
		if (FileVersionUE >= EUnrealEngineObjectUE4Version.ADD_STRING_ASSET_REFERENCES_MAP)
		{
			SoftPackageReferencesCount = Ar.Read<int>();
			SoftPackageReferencesOffset = Ar.Read<int>();
		}
		if (FileVersionUE >= EUnrealEngineObjectUE4Version.ADDED_SEARCHABLE_NAMES)
		{
			SearchableNamesOffset = Ar.Read<int>();
		}
		ThumbnailTableOffset = Ar.Read<int>();
		EGame game = Ar.Game;
		if (game == EGame.GAME_Valorant || game == EGame.GAME_HYENAS)
		{
			Ar.Position += 8L;
		}
		Guid = Ar.Read<FGuid>();
		if (!PackageFlags.HasFlag(EPackageFlags.PKG_FilterEditorOnly))
		{
			if (FileVersionUE >= EUnrealEngineObjectUE4Version.ADDED_PACKAGE_OWNER)
			{
				PersistentGuid = Ar.Read<FGuid>();
			}
			else
			{
				PersistentGuid = Guid;
			}
			if (FileVersionUE >= EUnrealEngineObjectUE4Version.ADDED_PACKAGE_OWNER && FileVersionUE < EUnrealEngineObjectUE4Version.NON_OUTER_PACKAGE_IMPORT)
			{
				Ar.Read<FGuid>();
			}
		}
		Generations = Ar.ReadArray<FGenerationInfo>();
		if (FileVersionUE >= EUnrealEngineObjectUE4Version.ENGINE_VERSION_OBJECT)
		{
			SavedByEngineVersion = new FEngineVersion(Ar);
			FixCorruptEngineVersion(FileVersionUE, SavedByEngineVersion);
		}
		else
		{
			int num2 = Ar.Read<int>();
			if (num2 != 0)
			{
				SavedByEngineVersion = new FEngineVersion(4, 0, 0, (uint)num2, string.Empty);
			}
		}
		if (FileVersionUE >= EUnrealEngineObjectUE4Version.PACKAGE_SUMMARY_HAS_COMPATIBLE_ENGINE_VERSION)
		{
			CompatibleWithEngineVersion = new FEngineVersion(Ar);
			FixCorruptEngineVersion(FileVersionUE, CompatibleWithEngineVersion);
		}
		else
		{
			CompatibleWithEngineVersion = SavedByEngineVersion;
		}
		CompressionFlags = Ar.Read<ECompressionFlags>();
		if (!VerifyCompressionFlagsValid((int)CompressionFlags))
		{
			throw new ParserException($"Invalid compression flags ({CompressionFlags})");
		}
		if (Ar.ReadArray<FCompressedChunk>().Length != 0)
		{
			throw new ParserException("Package level compression is enabled");
		}
		PackageSource = Ar.Read<int>();
		if (Ar.Game == EGame.GAME_ArkSurvivalEvolved && FileVersionLicenseeUE >= (EUnrealEngineObjectLicenseeUEVersion)10)
		{
			Ar.Position += 8L;
		}
		Ar.ReadArray(Ar.ReadFString);
		if (num > -7 && Ar.Read<int>() != 0)
		{
			throw new ParserException("NumTextureAllocations != 0");
		}
		if (FileVersionUE >= EUnrealEngineObjectUE4Version.ASSET_REGISTRY_TAGS)
		{
			AssetRegistryDataOffset = Ar.Read<int>();
		}
		if (Ar.Game == EGame.GAME_TowerOfFantasy)
		{
			TotalHeaderSize = (int)(TotalHeaderSize ^ 0xEEB2CEC7u);
			NameCount = (int)(NameCount ^ 0xEEB2CEC7u);
			NameOffset = (int)(NameOffset ^ 0xEEB2CEC7u);
			ExportCount = (int)(ExportCount ^ 0xEEB2CEC7u);
			ExportOffset = (int)(ExportOffset ^ 0xEEB2CEC7u);
			ImportCount = (int)(ImportCount ^ 0xEEB2CEC7u);
			ImportOffset = (int)(ImportOffset ^ 0xEEB2CEC7u);
			DependsOffset = (int)(DependsOffset ^ 0xEEB2CEC7u);
			PackageSource = (int)(PackageSource ^ 0xEEB2CEC7u);
			AssetRegistryDataOffset = (int)(AssetRegistryDataOffset ^ 0xEEB2CEC7u);
		}
		game = Ar.Game;
		if (game == EGame.GAME_SeaOfThieves || game == EGame.GAME_GearsOfWar4)
		{
			Ar.Position += 6L;
		}
		if (FileVersionUE >= EUnrealEngineObjectUE4Version.SUMMARY_HAS_BULKDATA_OFFSET)
		{
			BulkDataStartOffset = (int)Ar.Read<long>();
		}
		if (FileVersionUE >= EUnrealEngineObjectUE4Version.WORLD_LEVEL_INFO)
		{
			WorldTileInfoDataOffset = Ar.Read<int>();
		}
		if (FileVersionUE >= EUnrealEngineObjectUE4Version.CHANGED_CHUNKID_TO_BE_AN_ARRAY_OF_CHUNKIDS)
		{
			ChunkIds = Ar.ReadArray<int>();
		}
		else if (FileVersionUE >= EUnrealEngineObjectUE4Version.ADDED_CHUNKID_TO_ASSETDATA_AND_UPACKAGE)
		{
			int num3 = Ar.Read<int>();
			ChunkIds = ((num3 < 0) ? Array.Empty<int>() : new int[1] { num3 });
		}
		else
		{
			ChunkIds = Array.Empty<int>();
		}
		if (FileVersionUE >= EUnrealEngineObjectUE4Version.PRELOAD_DEPENDENCIES_IN_COOKED_EXPORTS)
		{
			PreloadDependencyCount = Ar.Read<int>();
			PreloadDependencyOffset = Ar.Read<int>();
		}
		else
		{
			PreloadDependencyCount = -1;
			PreloadDependencyOffset = 0;
		}
		NamesReferencedFromExportDataCount = ((FileVersionUE >= EUnrealEngineObjectUE5Version.NAMES_REFERENCED_FROM_EXPORT_DATA) ? Ar.Read<int>() : NameCount);
		PayloadTocOffset = ((FileVersionUE >= EUnrealEngineObjectUE5Version.PAYLOAD_TOC) ? Ar.Read<long>() : (-1));
		DataResourceOffset = ((FileVersionUE >= EUnrealEngineObjectUE5Version.DATA_RESOURCES) ? Ar.Read<int>() : (-1));
		if (Tag == 6647407 && Ar is FAssetArchive fAssetArchive)
		{
			fAssetArchive.AbsoluteOffset = NameOffset - (int)Ar.Position;
		}
		static bool VerifyCompressionFlagsValid(int compressionFlags)
		{
			return (compressionFlags & -512) == 0;
		}
	}

	private static void FixCorruptEngineVersion(FPackageFileVersion objectVersion, FEngineVersion version)
	{
		if (objectVersion < EUnrealEngineObjectUE4Version.CORRECT_LICENSEE_FLAG && version != null && version.Major == 4 && version.Minor == 26 && version.Patch == 0 && version.Changelist >= 12740027 && version.IsLicenseeVersion())
		{
			version.Set(4, 26, 0, version.Changelist, version.Branch);
		}
	}
}
