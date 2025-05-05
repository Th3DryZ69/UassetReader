using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Objects.Core.Serialization;

namespace CUE4Parse.UE4.Versions;

public class VersionContainer : ICloneable
{
	public static readonly VersionContainer DEFAULT_VERSION_CONTAINER = new VersionContainer();

	private EGame _game;

	private ETexturePlatform _platform;

	private FPackageFileVersion _ver;

	public FCustomVersionContainer? CustomVersions;

	public readonly Dictionary<string, bool> Options = new Dictionary<string, bool>();

	public readonly Dictionary<string, KeyValuePair<string, string>> MapStructTypes = new Dictionary<string, KeyValuePair<string, string>>();

	private readonly Dictionary<string, bool>? _optionOverrides;

	private readonly Dictionary<string, KeyValuePair<string, string>>? _mapStructTypesOverrides;

	public EGame Game
	{
		get
		{
			return _game;
		}
		set
		{
			_game = value;
			InitOptions();
			InitMapStructTypes();
		}
	}

	public FPackageFileVersion Ver
	{
		get
		{
			return _ver;
		}
		set
		{
			bExplicitVer = value.FileVersionUE4 != 0 || value.FileVersionUE5 != 0;
			_ver = (bExplicitVer ? value : _game.GetVersion());
		}
	}

	public ETexturePlatform Platform
	{
		get
		{
			return _platform;
		}
		set
		{
			_platform = value;
			InitOptions();
			InitMapStructTypes();
		}
	}

	public bool bExplicitVer { get; private set; }

	public bool this[string optionKey]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Options[optionKey];
		}
	}

	public VersionContainer(EGame game = EGame.GAME_UE4_28, ETexturePlatform platform = ETexturePlatform.DesktopMobile, FPackageFileVersion ver = default(FPackageFileVersion), FCustomVersionContainer? customVersions = null, Dictionary<string, bool>? optionOverrides = null, Dictionary<string, KeyValuePair<string, string>>? mapStructTypesOverrides = null)
	{
		_optionOverrides = optionOverrides;
		_mapStructTypesOverrides = mapStructTypesOverrides;
		Game = game;
		Ver = ver;
		Platform = platform;
		CustomVersions = customVersions;
	}

	private void InitOptions()
	{
		Options.Clear();
		Options["MorphTarget"] = true;
		Options["RawIndexBuffer.HasShouldExpandTo32Bit"] = Game >= EGame.GAME_UE4_25;
		Options["ShaderMap.UseNewCookedFormat"] = Game >= EGame.GAME_UE5_0;
		Options["SkeletalMesh.KeepMobileMinLODSettingOnDesktop"] = Game >= EGame.GAME_UE5_2;
		Options["SkeletalMesh.UseNewCookedFormat"] = Game >= EGame.GAME_UE4_24;
		Dictionary<string, bool> options = Options;
		EGame game = Game;
		options["SkeletalMesh.HasRayTracingData"] = game >= EGame.GAME_UE4_27 || game == EGame.GAME_UE4_25_Plus;
		Dictionary<string, bool> options2 = Options;
		game = Game;
		options2["StaticMesh.HasLODsShareStaticLighting"] = game < EGame.GAME_UE4_15 || game >= EGame.GAME_UE4_16;
		Options["StaticMesh.HasRayTracingGeometry"] = Game >= EGame.GAME_UE4_25;
		Options["StaticMesh.HasVisibleInRayTracing"] = Game >= EGame.GAME_UE4_26;
		Options["StaticMesh.KeepMobileMinLODSettingOnDesktop"] = Game >= EGame.GAME_UE5_2;
		Options["StaticMesh.UseNewCookedFormat"] = Game >= EGame.GAME_UE4_23;
		Options["VirtualTextures"] = Game >= EGame.GAME_UE4_23;
		Options["SoundWave.UseAudioStreaming"] = Game >= EGame.GAME_UE4_25 && Game != EGame.GAME_UE4_28 && Game != EGame.GAME_GTATheTrilogyDefinitiveEdition && Game != EGame.GAME_ReadyOrNot && Game != EGame.GAME_BladeAndSoul;
		Options["AnimSequence.HasCompressedRawSize"] = Game >= EGame.GAME_UE4_17;
		Options["StaticMesh.HasNavCollision"] = Ver >= EUnrealEngineObjectUE4Version.STATIC_MESH_STORE_NAV_COLLISION && Game != EGame.GAME_GearsOfWar4 && Game != EGame.GAME_TEKKEN7;
		if (_optionOverrides == null)
		{
			return;
		}
		foreach (var (key, value) in _optionOverrides)
		{
			Options[key] = value;
		}
	}

	private void InitMapStructTypes()
	{
		MapStructTypes.Clear();
		MapStructTypes["BindingIdToReferences"] = new KeyValuePair<string, string>("Guid", null);
		MapStructTypes["UserParameterRedirects"] = new KeyValuePair<string, string>("NiagaraVariable", "NiagaraVariable");
		MapStructTypes["Tracks"] = new KeyValuePair<string, string>("MovieSceneTrackIdentifier", null);
		MapStructTypes["SubSequences"] = new KeyValuePair<string, string>("MovieSceneSequenceID", null);
		MapStructTypes["Hierarchy"] = new KeyValuePair<string, string>("MovieSceneSequenceID", null);
		MapStructTypes["TrackSignatureToTrackIdentifier"] = new KeyValuePair<string, string>("Guid", "MovieSceneTrackIdentifier");
		if (_mapStructTypesOverrides == null)
		{
			return;
		}
		foreach (var (key, value) in _mapStructTypesOverrides)
		{
			MapStructTypes[key] = value;
		}
	}

	public object Clone()
	{
		return new VersionContainer(Game, Platform, Ver, CustomVersions, _optionOverrides, _mapStructTypesOverrides)
		{
			bExplicitVer = bExplicitVer
		};
	}
}
