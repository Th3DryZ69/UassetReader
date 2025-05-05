using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FSequencerObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		RenameMediaSourcePlatformPlayers = 1,
		ConvertEnableRootMotionToForceRootLock = 2,
		ConvertMultipleRowsToTracks = 3,
		WhenFinishedDefaultsToRestoreState = 4,
		EvaluationTree = 5,
		WhenFinishedDefaultsToProjectDefault = 6,
		FloatToIntConversion = 7,
		PurgeSpawnableBlueprints = 8,
		FinishUMGEvaluation = 9,
		SerializeFloatChannel = 10,
		ModifyLinearKeysForOldInterp = 11,
		SerializeFloatChannelCompletely = 12,
		SpawnableImprovements = 13,
		VersionPlusOne = 14,
		LatestVersion = 13
	}

	public static readonly FGuid GUID = new FGuid(2069555020u, 3530574864u, 2841139096u, 186722906u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game < EGame.GAME_UE4_20)
		{
			if (game < EGame.GAME_UE4_16)
			{
				if (game >= EGame.GAME_UE4_14)
				{
					if (game < EGame.GAME_UE4_15)
					{
						return Type.RenameMediaSourcePlatformPlayers;
					}
					return Type.ConvertMultipleRowsToTracks;
				}
				return Type.BeforeCustomVersionWasAdded;
			}
			if (game < EGame.GAME_UE4_19)
			{
				return Type.WhenFinishedDefaultsToRestoreState;
			}
			return Type.WhenFinishedDefaultsToProjectDefault;
		}
		if (game < EGame.GAME_UE4_25)
		{
			if (game < EGame.GAME_UE4_22)
			{
				return Type.FinishUMGEvaluation;
			}
			return Type.ModifyLinearKeysForOldInterp;
		}
		if (game < EGame.GAME_UE4_27)
		{
			return Type.SerializeFloatChannelCompletely;
		}
		return Type.SpawnableImprovements;
	}
}
