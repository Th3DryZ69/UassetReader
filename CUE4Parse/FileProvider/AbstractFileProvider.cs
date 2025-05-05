using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.IO;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Localization;
using CUE4Parse.UE4.Objects.Core.i18N;
using CUE4Parse.UE4.Pak.Objects;
using CUE4Parse.UE4.Plugins;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.FileProvider;

public abstract class AbstractFileProvider : IFileProvider
{
	protected static readonly ILogger Log = Serilog.Log.ForContext<IFileProvider>();

	private string _gameName;

	public virtual VersionContainer Versions { get; set; }

	public virtual ITypeMappingsProvider? MappingsContainer { get; set; }

	public virtual TypeMappings? MappingsForGame => MappingsContainer?.MappingsForGame;

	public virtual IDictionary<string, IDictionary<string, string>> LocalizedResources { get; } = new Dictionary<string, IDictionary<string, string>>();

	public Dictionary<string, string> VirtualPaths { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	public abstract IReadOnlyDictionary<string, GameFile> Files { get; }

	public abstract IReadOnlyDictionary<FPackageId, GameFile> FilesById { get; }

	public virtual bool IsCaseInsensitive { get; }

	public bool ReadScriptData { get; set; }

	public virtual bool UseLazySerialization { get; set; } = true;

	public virtual string GameName
	{
		get
		{
			if (string.IsNullOrEmpty(_gameName))
			{
				string s = Files.Keys.FirstOrDefault((string it) => !it.SubstringBefore('/').EndsWith("engine", StringComparison.OrdinalIgnoreCase) && !it.StartsWith('/')) ?? string.Empty;
				_gameName = s.SubstringBefore('/');
			}
			return _gameName;
		}
	}

	public virtual GameFile this[string path] => Files[FixPath(path)];

	protected AbstractFileProvider(bool isCaseInsensitive = false, VersionContainer? versions = null)
	{
		IsCaseInsensitive = isCaseInsensitive;
		Versions = versions ?? VersionContainer.DEFAULT_VERSION_CONTAINER;
	}

	public virtual int LoadLocalization(ELanguage language = ELanguage.English, CancellationToken cancellationToken = default(CancellationToken))
	{
		Regex regex = new Regex($"^{GameName}/.+/{GetLanguageCode(language)}/.+.locres$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		LocalizedResources.Clear();
		int num = 0;
		foreach (KeyValuePair<string, GameFile> item in Files.Where<KeyValuePair<string, GameFile>>((KeyValuePair<string, GameFile> x) => regex.IsMatch(x.Key)))
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (!item.Value.TryCreateReader(out FArchive reader))
			{
				continue;
			}
			foreach (KeyValuePair<FTextKey, Dictionary<FTextKey, FEntry>> entry in new FTextLocalizationResource(reader).Entries)
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (!LocalizedResources.ContainsKey(entry.Key.Str))
				{
					LocalizedResources[entry.Key.Str] = new Dictionary<string, string>();
				}
				foreach (KeyValuePair<FTextKey, FEntry> item2 in entry.Value)
				{
					cancellationToken.ThrowIfCancellationRequested();
					LocalizedResources[entry.Key.Str][item2.Key.Str] = item2.Value.LocalizedString;
					num++;
				}
			}
		}
		return num;
	}

	public virtual string GetLocalizedString(string namespacee, string key, string? defaultValue)
	{
		if (LocalizedResources.TryGetValue(namespacee, out IDictionary<string, string> value) && value.TryGetValue(key, out var value2))
		{
			return value2;
		}
		return defaultValue ?? string.Empty;
	}

	public string GetLanguageCode(ELanguage language)
	{
		return GameName.ToLowerInvariant() switch
		{
			"fortnitegame" => language switch
			{
				ELanguage.English => "en", 
				ELanguage.French => "fr", 
				ELanguage.German => "de", 
				ELanguage.Italian => "it", 
				ELanguage.Spanish => "es", 
				ELanguage.SpanishLatin => "es-419", 
				ELanguage.Arabic => "ar", 
				ELanguage.Japanese => "ja", 
				ELanguage.Korean => "ko", 
				ELanguage.Polish => "pl", 
				ELanguage.PortugueseBrazil => "pt-BR", 
				ELanguage.Russian => "ru", 
				ELanguage.Turkish => "tr", 
				ELanguage.Chinese => "zh-CN", 
				ELanguage.TraditionalChinese => "zh-Hant", 
				_ => "en", 
			}, 
			"worldexplorers" => language switch
			{
				ELanguage.English => "en", 
				ELanguage.French => "fr", 
				ELanguage.German => "de", 
				ELanguage.Italian => "it", 
				ELanguage.Spanish => "es", 
				ELanguage.Japanese => "ja", 
				ELanguage.Korean => "ko", 
				ELanguage.PortugueseBrazil => "pt-BR", 
				ELanguage.Russian => "ru", 
				ELanguage.Chinese => "zh-Hans", 
				_ => "en", 
			}, 
			"shootergame" => language switch
			{
				ELanguage.English => "en-US", 
				ELanguage.French => "fr-FR", 
				ELanguage.German => "de-DE", 
				ELanguage.Italian => "it-IT", 
				ELanguage.Spanish => "es-ES", 
				ELanguage.SpanishMexico => "es-MX", 
				ELanguage.Arabic => "ar-AE", 
				ELanguage.Japanese => "ja-JP", 
				ELanguage.Korean => "ko-KR", 
				ELanguage.Polish => "pl-PL", 
				ELanguage.PortugueseBrazil => "pt-BR", 
				ELanguage.Russian => "ru-RU", 
				ELanguage.Turkish => "tr-TR", 
				ELanguage.Chinese => "zh-CN", 
				ELanguage.TraditionalChinese => "zh-TW", 
				ELanguage.Indonesian => "id-ID", 
				ELanguage.Thai => "th-TH", 
				ELanguage.VietnameseVietnam => "vi-VN", 
				_ => "en-US", 
			}, 
			"stateofdecay2" => language switch
			{
				ELanguage.English => "en-US", 
				ELanguage.AustralianEnglish => "en-AU", 
				ELanguage.French => "fr-FR", 
				ELanguage.German => "de-DE", 
				ELanguage.Italian => "it-IT", 
				ELanguage.SpanishMexico => "es-MX", 
				ELanguage.PortugueseBrazil => "pt-BR", 
				ELanguage.Russian => "ru-RU", 
				ELanguage.Chinese => "zh-CN", 
				_ => "en-US", 
			}, 
			"oakgame" => language switch
			{
				ELanguage.English => "en", 
				ELanguage.French => "fr", 
				ELanguage.German => "de", 
				ELanguage.Italian => "it", 
				ELanguage.Spanish => "es", 
				ELanguage.Japanese => "ja", 
				ELanguage.Korean => "ko", 
				ELanguage.PortugueseBrazil => "pt-BR", 
				ELanguage.Russian => "ru", 
				ELanguage.Chinese => "zh-Hans-CN", 
				ELanguage.TraditionalChinese => "zh-Hant-TW", 
				_ => "en", 
			}, 
			"multiversus" => language switch
			{
				ELanguage.English => "en", 
				ELanguage.French => "fr", 
				ELanguage.German => "de", 
				ELanguage.Italian => "it", 
				ELanguage.Spanish => "es", 
				ELanguage.SpanishLatin => "es-419", 
				ELanguage.Polish => "pl", 
				ELanguage.PortugueseBrazil => "pt-BR", 
				ELanguage.Russian => "ru", 
				ELanguage.Chinese => "zh-Hans", 
				_ => "en", 
			}, 
			_ => language switch
			{
				ELanguage.English => "en", 
				ELanguage.AustralianEnglish => "en-AU", 
				ELanguage.BritishEnglish => "en-GB", 
				ELanguage.French => "fr", 
				ELanguage.German => "de", 
				ELanguage.Italian => "it", 
				ELanguage.Spanish => "es", 
				ELanguage.SpanishLatin => "es-419", 
				ELanguage.SpanishMexico => "es-MX", 
				ELanguage.Arabic => "ar", 
				ELanguage.Japanese => "ja", 
				ELanguage.Korean => "ko", 
				ELanguage.Polish => "pl", 
				ELanguage.Portuguese => "pt", 
				ELanguage.PortugueseBrazil => "pt-BR", 
				ELanguage.Russian => "ru", 
				ELanguage.Turkish => "tr", 
				ELanguage.Chinese => "zh", 
				ELanguage.TraditionalChinese => "zh-Hant", 
				ELanguage.Swedish => "sv", 
				ELanguage.Thai => "th", 
				ELanguage.Indonesian => "id", 
				ELanguage.VietnameseVietnam => "vi-VN", 
				ELanguage.Zulu => "zu", 
				_ => "en", 
			}, 
		};
	}

	public virtual int LoadVirtualPaths()
	{
		return LoadVirtualPaths(Versions.Ver);
	}

	public virtual int LoadVirtualPaths(FPackageFileVersion version, CancellationToken cancellationToken = default(CancellationToken))
	{
		Regex regex = new Regex("^" + GameName + "/Plugins/.+.upluginmanifest$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		VirtualPaths.Clear();
		int num = 0;
		bool flag = version < EUnrealEngineObjectUE4Version.ADDED_SOFT_OBJECT_PATH || !Files.Any<KeyValuePair<string, GameFile>>((KeyValuePair<string, GameFile> file) => file.Key.EndsWith(".upluginmanifest"));
		foreach (var (text2, gameFile2) in Files)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (flag)
			{
				if (!text2.EndsWith(".uplugin") || !TryCreateReader(gameFile2.Path, out FArchive reader))
				{
					continue;
				}
				using StreamReader streamReader = new StreamReader(reader);
				if (JsonConvert.DeserializeObject<UPluginDescriptor>(streamReader.ReadToEnd()).CanContainContent)
				{
					string key = gameFile2.Path.SubstringAfterLast('/').SubstringBeforeLast('.');
					string value = gameFile2.Path.SubstringBeforeLast('/');
					if (!VirtualPaths.ContainsKey(key))
					{
						VirtualPaths.Add(key, value);
						num++;
					}
					else
					{
						VirtualPaths[key] = value;
					}
				}
			}
			else
			{
				if (!regex.IsMatch(text2) || !TryCreateReader(gameFile2.Path, out FArchive reader2))
				{
					continue;
				}
				using StreamReader streamReader2 = new StreamReader(reader2);
				UPluginContents[] contents = JsonConvert.DeserializeObject<UPluginManifest>(streamReader2.ReadToEnd()).Contents;
				foreach (UPluginContents uPluginContents in contents)
				{
					cancellationToken.ThrowIfCancellationRequested();
					if (uPluginContents.Descriptor.CanContainContent)
					{
						string key2 = uPluginContents.File.SubstringAfterLast('/').SubstringBeforeLast('.');
						string value2 = uPluginContents.File.Replace("../../../", string.Empty).SubstringBeforeLast('/');
						if (!VirtualPaths.ContainsKey(key2))
						{
							VirtualPaths.Add(key2, value2);
							num++;
						}
						else
						{
							VirtualPaths[key2] = value2;
						}
					}
				}
			}
		}
		return num;
	}

	public virtual bool TryFindGameFile(string path, out GameFile file)
	{
		string text = FixPath(path);
		if (Files.TryGetValue(text, out file))
		{
			return true;
		}
		string key = text.SubstringBeforeWithLast('.') + GameFile.Ue4PackageExtensions[1];
		if (Files.TryGetValue(key, out file))
		{
			return true;
		}
		return Files.TryGetValue(IsCaseInsensitive ? path.ToLowerInvariant() : path, out file);
	}

	public virtual string FixPath(string path)
	{
		return FixPath(path, IsCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
	}

	public virtual string FixPath(string path, StringComparison comparisonType)
	{
		path = path.Replace('\\', '/');
		if (path[0] == '/')
		{
			string text = path;
			path = text.Substring(1, text.Length - 1);
		}
		string text2 = path.SubstringAfterLast('/');
		if (text2.Contains('.') && text2.SubstringBefore('.') == text2.SubstringAfter('.'))
		{
			path = path.SubstringBeforeLast('/') + "/" + text2.SubstringBefore('.');
		}
		string text3 = path;
		if (text3[text3.Length - 1] != '/' && !text2.Contains('.'))
		{
			path = path + "." + GameFile.Ue4PackageExtensions[0];
		}
		string text4 = path.SubstringBefore('/');
		if (text4.Equals(GameName, StringComparison.OrdinalIgnoreCase))
		{
			if (comparisonType != StringComparison.OrdinalIgnoreCase)
			{
				return path;
			}
			return path.ToLowerInvariant();
		}
		if (text4.Equals("Game", comparisonType) || text4.Equals("Engine", comparisonType))
		{
			string text5 = (text4.Equals("Engine", comparisonType) ? "Engine" : GameName);
			string text6 = path.SubstringAfter('/').SubstringBefore('/');
			if (text6.Contains('.'))
			{
				string text7 = text5 + "/Content/" + path.SubstringAfter('/');
				if (comparisonType != StringComparison.OrdinalIgnoreCase)
				{
					return text7;
				}
				return text7.ToLowerInvariant();
			}
			if (text6.Equals("Config", comparisonType) || text6.Equals("Content", comparisonType) || text6.Equals("Plugins", comparisonType))
			{
				string text8 = text5 + '/' + path.SubstringAfter('/');
				if (comparisonType != StringComparison.OrdinalIgnoreCase)
				{
					return text8;
				}
				return text8.ToLowerInvariant();
			}
			string text9 = text5 + "/Content/" + path.SubstringAfter('/');
			if (comparisonType != StringComparison.OrdinalIgnoreCase)
			{
				return text9;
			}
			return text9.ToLowerInvariant();
		}
		if (VirtualPaths.TryGetValue(text4, out string value))
		{
			string text10 = value + "/Content/" + path.SubstringAfter('/');
			if (comparisonType != StringComparison.OrdinalIgnoreCase)
			{
				return text10;
			}
			return text10.ToLowerInvariant();
		}
		string text11 = string.Concat(GameName, "/Plugins/" + (GameName.ToLowerInvariant().Equals("fortnitegame") ? "GameFeatures/" : "") + text4 + "/Content/", path.SubstringAfter('/'));
		if (comparisonType != StringComparison.OrdinalIgnoreCase)
		{
			return text11;
		}
		return text11.ToLowerInvariant();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual byte[] SaveAsset(string path)
	{
		return this[path].Read();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual bool TrySaveAsset(string path, out byte[] data)
	{
		if (!TryFindGameFile(path, out GameFile file))
		{
			data = null;
			return false;
		}
		return file.TryRead(out data);
	}

	public virtual async Task<byte[]> SaveAssetAsync(string path)
	{
		return await Task.Run(() => SaveAsset(path));
	}

	public virtual async Task<byte[]?> TrySaveAssetAsync(string path)
	{
		return await Task.Run(delegate
		{
			TrySaveAsset(path, out byte[] data);
			return data;
		});
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual FArchive CreateReader(string path)
	{
		return this[path].CreateReader();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual bool TryCreateReader(string path, out FArchive reader)
	{
		if (!TryFindGameFile(path, out GameFile file))
		{
			reader = null;
			return false;
		}
		return file.TryCreateReader(out reader);
	}

	public virtual async Task<FArchive> CreateReaderAsync(string path)
	{
		return await Task.Run(() => CreateReader(path));
	}

	public virtual async Task<FArchive?> TryCreateReaderAsync(string path)
	{
		return await Task.Run(delegate
		{
			TryCreateReader(path, out FArchive reader);
			return reader;
		});
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual IPackage LoadPackage(string path)
	{
		return LoadPackage(this[path]);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual IPackage LoadPackage(GameFile file)
	{
		return LoadPackageAsync(file).Result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual IoPackage LoadPackage(FPackageId id)
	{
		return (IoPackage)LoadPackage(FilesById[id]);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual bool TryLoadPackage(string path, out IPackage package)
	{
		if (!TryFindGameFile(path, out GameFile file))
		{
			package = null;
			return false;
		}
		return TryLoadPackage(file, out package);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual bool TryLoadPackage(GameFile file, out IPackage package)
	{
		package = TryLoadPackageAsync(file).Result;
		return package != null;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual bool TryLoadPackage(FPackageId id, out IoPackage package)
	{
		if (FilesById.TryGetValue(id, out GameFile value) && TryLoadPackage(value, out IPackage package2))
		{
			package = (IoPackage)package2;
			return true;
		}
		package = null;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual async Task<IPackage> LoadPackageAsync(string path)
	{
		return await LoadPackageAsync(this[path]);
	}

	public virtual async Task<IPackage> LoadPackageAsync(GameFile file)
	{
		if (!file.IsUE4Package)
		{
			throw new ArgumentException("File must be a package to be a loaded as one", "file");
		}
		Files.TryGetValue(file.PathWithoutExtension + ".uexp", out GameFile value);
		Files.TryGetValue(file.PathWithoutExtension + ".ubulk", out GameFile value2);
		Files.TryGetValue(file.PathWithoutExtension + ".uptnl", out GameFile value3);
		Task<FArchive> task = file.CreateReaderAsync();
		Task<FArchive> uexpTask = value?.CreateReaderAsync();
		Task<FArchive> ubulkTask = value2?.CreateReaderAsync();
		Task<FArchive> uptnlTask = value3?.CreateReaderAsync();
		FArchive uasset = await task;
		FArchive fArchive = ((uexpTask == null) ? null : (await uexpTask));
		FArchive uexp = fArchive;
		fArchive = ((ubulkTask == null) ? null : (await ubulkTask));
		FArchive ubulk = fArchive;
		fArchive = ((uptnlTask == null) ? null : (await uptnlTask));
		FArchive uptnl = fArchive;
		if (file is FPakEntry || file is OsGameFile)
		{
			return new Package(uasset, uexp, ubulk, uptnl, this, MappingsForGame, UseLazySerialization);
		}
		if (!(this is IVfsFileProvider { GlobalData: not null } vfsFileProvider))
		{
			throw new ParserException("Found IoStore Package but global data is missing, can't serialize");
		}
		FIoContainerHeader containerHeader = ((FIoStoreEntry)file).IoStoreReader.ContainerHeader;
		return new IoPackage(uasset, vfsFileProvider.GlobalData, containerHeader, ubulk, uptnl, this, MappingsForGame);
	}

	public virtual async Task<IPackage?> TryLoadPackageAsync(string path)
	{
		if (!TryFindGameFile(path, out GameFile file))
		{
			return null;
		}
		return await TryLoadPackageAsync(file).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async Task<IPackage?> TryLoadPackageAsync(GameFile file)
	{
		if (!file.IsUE4Package)
		{
			return null;
		}
		Files.TryGetValue(file.PathWithoutExtension + ".uexp", out GameFile value);
		Files.TryGetValue(file.PathWithoutExtension + ".ubulk", out GameFile ubulkFile);
		Files.TryGetValue(file.PathWithoutExtension + ".uptnl", out GameFile uptnlFile);
		ConfiguredTaskAwaitable<FArchive> configuredTaskAwaitable = file.TryCreateReaderAsync().ConfigureAwait(continueOnCapturedContext: false);
		ConfiguredTaskAwaitable<FArchive?>? uexpTask = value?.TryCreateReaderAsync().ConfigureAwait(continueOnCapturedContext: false);
		FArchive reader;
		Lazy<FArchive?> lazyUbulk = ((ubulkFile != null) ? new Lazy<FArchive>(() => (!ubulkFile.TryCreateReader(out reader)) ? null : reader) : null);
		Lazy<FArchive?> lazyUptnl = ((uptnlFile != null) ? new Lazy<FArchive>(() => (!uptnlFile.TryCreateReader(out reader)) ? null : reader) : null);
		FArchive uasset = await configuredTaskAwaitable;
		if (uasset == null)
		{
			return null;
		}
		FArchive fArchive = ((!uexpTask.HasValue) ? null : (await uexpTask.Value));
		FArchive uexp = fArchive;
		try
		{
			if (file is FPakEntry || file is OsGameFile)
			{
				return new Package(uasset, uexp, lazyUbulk, lazyUptnl, this, MappingsForGame, UseLazySerialization);
			}
			if (file is FIoStoreEntry fIoStoreEntry)
			{
				IoGlobalData globalData = ((IVfsFileProvider)this).GlobalData;
				return (globalData != null) ? new IoPackage(uasset, globalData, fIoStoreEntry.IoStoreReader.ContainerHeader, lazyUbulk, lazyUptnl, this, MappingsForGame) : null;
			}
			return null;
		}
		catch (Exception exception)
		{
			Log.Error(exception, "Failed to load package " + file);
			return null;
		}
	}

	public virtual IReadOnlyDictionary<string, byte[]> SavePackage(string path)
	{
		return SavePackage(this[path]);
	}

	public virtual IReadOnlyDictionary<string, byte[]> SavePackage(GameFile file)
	{
		return SavePackageAsync(file).Result;
	}

	public virtual bool TrySavePackage(string path, out IReadOnlyDictionary<string, byte[]> package)
	{
		if (!TryFindGameFile(path, out GameFile file))
		{
			package = null;
			return false;
		}
		return TrySavePackage(file, out package);
	}

	public virtual bool TrySavePackage(GameFile file, out IReadOnlyDictionary<string, byte[]> package)
	{
		package = TrySavePackageAsync(file).Result;
		return package != null;
	}

	public virtual async Task<IReadOnlyDictionary<string, byte[]>> SavePackageAsync(string path)
	{
		return await SavePackageAsync(this[path]);
	}

	public virtual async Task<IReadOnlyDictionary<string, byte[]>> SavePackageAsync(GameFile file)
	{
		Files.TryGetValue(file.PathWithoutExtension + ".uexp", out GameFile uexpFile);
		Files.TryGetValue(file.PathWithoutExtension + ".ubulk", out GameFile ubulkFile);
		Files.TryGetValue(file.PathWithoutExtension + ".uptnl", out GameFile uptnlFile);
		Task<byte[]> task = file.ReadAsync();
		Task<byte[]> uexpTask = uexpFile?.ReadAsync();
		Task<byte[]> ubulkTask = ubulkFile?.ReadAsync();
		Task<byte[]> uptnlTask = uptnlFile?.ReadAsync();
		Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
		Dictionary<string, byte[]> dictionary2 = dictionary;
		string path = file.Path;
		dictionary2.Add(path, await task);
		Dictionary<string, byte[]> dict = dictionary;
		byte[] array = ((uexpTask == null) ? null : (await uexpTask));
		byte[] uexp = array;
		array = ((ubulkTask == null) ? null : (await ubulkTask));
		byte[] ubulk = array;
		array = ((uptnlTask == null) ? null : (await uptnlTask));
		byte[] array2 = array;
		if (uexpFile != null && uexp != null)
		{
			dict[uexpFile.Path] = uexp;
		}
		if (ubulkFile != null && ubulk != null)
		{
			dict[ubulkFile.Path] = ubulk;
		}
		if (uptnlFile != null && array2 != null)
		{
			dict[uptnlFile.Path] = array2;
		}
		return dict;
	}

	public virtual async Task<IReadOnlyDictionary<string, byte[]>?> TrySavePackageAsync(string path)
	{
		if (!TryFindGameFile(path, out GameFile file))
		{
			return null;
		}
		return await TrySavePackageAsync(file).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async Task<IReadOnlyDictionary<string, byte[]>?> TrySavePackageAsync(GameFile file)
	{
		Files.TryGetValue(file.PathWithoutExtension + ".uexp", out GameFile uexpFile);
		Files.TryGetValue(file.PathWithoutExtension + ".ubulk", out GameFile ubulkFile);
		Files.TryGetValue(file.PathWithoutExtension + ".uptnl", out GameFile uptnlFile);
		ConfiguredTaskAwaitable<byte[]> configuredTaskAwaitable = file.TryReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		ConfiguredTaskAwaitable<byte[]?>? uexpTask = uexpFile?.TryReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		ConfiguredTaskAwaitable<byte[]?>? ubulkTask = ubulkFile?.TryReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		ConfiguredTaskAwaitable<byte[]?>? uptnlTask = uptnlFile?.TryReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		byte[] uasset = await configuredTaskAwaitable;
		if (uasset == null)
		{
			return null;
		}
		byte[] array = ((!uexpTask.HasValue) ? null : (await uexpTask.Value));
		byte[] uexp = array;
		array = ((!ubulkTask.HasValue) ? null : (await ubulkTask.Value));
		byte[] ubulk = array;
		array = ((!uptnlTask.HasValue) ? null : (await uptnlTask.Value));
		byte[] array2 = array;
		Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]> { { file.Path, uasset } };
		if (uexpFile != null && uexp != null)
		{
			dictionary[uexpFile.Path] = uexp;
		}
		if (ubulkFile != null && ubulk != null)
		{
			dictionary[ubulkFile.Path] = ubulk;
		}
		if (uptnlFile != null && array2 != null)
		{
			dictionary[uptnlFile.Path] = array2;
		}
		return dictionary;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual UObject LoadObject(string? objectPath)
	{
		return LoadObjectAsync(objectPath).Result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual bool TryLoadObject(string? objectPath, out UObject export)
	{
		export = TryLoadObjectAsync(objectPath).Result;
		return export != null;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual T LoadObject<T>(string? objectPath) where T : UObject
	{
		return LoadObjectAsync<T>(objectPath).Result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual bool TryLoadObject<T>(string? objectPath, out T export) where T : UObject
	{
		export = TryLoadObjectAsync<T>(objectPath).Result;
		return export != null;
	}

	public virtual async Task<UObject> LoadObjectAsync(string? objectPath)
	{
		if (objectPath == null)
		{
			throw new ArgumentException("ObjectPath can't be null", "objectPath");
		}
		string text = objectPath;
		int num = text.IndexOf('.');
		string objectName;
		if (num == -1)
		{
			objectName = text.SubstringAfterLast('/');
		}
		else
		{
			objectName = text.Substring(num + 1);
			text = text.Substring(0, num);
		}
		return (await LoadPackageAsync(text)).GetExport(objectName, IsCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
	}

	public virtual async Task<UObject?> TryLoadObjectAsync(string? objectPath)
	{
		if (objectPath == null || objectPath.Equals("None", IsCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
		{
			return null;
		}
		string text = objectPath;
		int num = text.IndexOf('.');
		string objectName;
		if (num == -1)
		{
			objectName = text.SubstringAfterLast('/');
		}
		else
		{
			objectName = text.Substring(num + 1);
			text = text.Substring(0, num);
		}
		return (await TryLoadPackageAsync(text).ConfigureAwait(continueOnCapturedContext: false))?.GetExportOrNull(objectName, IsCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual async Task<T> LoadObjectAsync<T>(string? objectPath) where T : UObject
	{
		return ((await LoadObjectAsync(objectPath)) as T) ?? throw new ParserException("Loaded object but it was of wrong type");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual async Task<T?> TryLoadObjectAsync<T>(string? objectPath) where T : UObject
	{
		return (await TryLoadObjectAsync(objectPath)) as T;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual IEnumerable<UObject> LoadObjectExports(string? objectPath)
	{
		if (objectPath == null)
		{
			throw new ArgumentException("ObjectPath can't be null", "objectPath");
		}
		return LoadPackage(objectPath).GetExports();
	}
}
