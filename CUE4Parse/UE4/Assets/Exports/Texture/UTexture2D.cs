using System;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Versions;
using Serilog;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public class UTexture2D : UTexture
{
	public FIntPoint ImportedSize { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		ImportedSize = GetOrDefault<FIntPoint>("ImportedSize");
		Ar.Read<FStripDataFlags>();
		bool num = Ar.Ver >= EUnrealEngineObjectUE4Version.ADD_COOKED_TO_TEXTURE2D && Ar.ReadBoolean();
		if (Ar.Ver < EUnrealEngineObjectUE4Version.TEXTURE_SOURCE_ART_REFACTOR)
		{
			Log.Warning("Untested code: UTexture2D::LegacySerialize");
			FTexture2DMipMap[] array = Array.Empty<FTexture2DMipMap>();
			bool orDefault = GetOrDefault("bDisableDerivedDataCache_DEPRECATED", defaultValue: false);
			if (orDefault)
			{
				array = Ar.ReadArray(() => new FTexture2DMipMap(Ar));
			}
			Ar.Read<FGuid>();
			base.Format = GetOrDefault("Format", EPixelFormat.PF_Unknown);
			if (orDefault)
			{
				_ = array.LongLength;
			}
		}
		if (num)
		{
			bool bSerializeMipData = true;
			if (Ar.Game >= EGame.GAME_UE5_3)
			{
				bSerializeMipData = Ar.ReadBoolean();
			}
			DeserializeCookedPlatformData(Ar, bSerializeMipData);
		}
	}
}
