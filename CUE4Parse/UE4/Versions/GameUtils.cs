using System.Runtime.CompilerServices;

namespace CUE4Parse.UE4.Versions;

public static class GameUtils
{
	public const int GameUe4Base = 16777216;

	public const int GameUe5Base = 33554432;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GAME_UE4(int x)
	{
		return 16777216 + x << 4;
	}

	public static FPackageFileVersion GetVersion(this EGame game)
	{
		if (game >= EGame.GAME_UE5_0)
		{
			if (game >= EGame.GAME_UE5_1)
			{
				if (game < EGame.GAME_UE5_2)
				{
					return new FPackageFileVersion(522, 1008);
				}
				return new FPackageFileVersion(522, 1009);
			}
			return new FPackageFileVersion(522, 1004);
		}
		int version = ((game < EGame.GAME_UE4_12) ? ((game < EGame.GAME_UE4_8) ? ((game < EGame.GAME_UE4_4) ? ((game < EGame.GAME_UE4_2) ? ((game >= EGame.GAME_UE4_1) ? 352 : 342) : ((game >= EGame.GAME_UE4_3) ? 382 : 363)) : ((game < EGame.GAME_UE4_6) ? ((game >= EGame.GAME_UE4_5) ? 401 : 385) : ((game >= EGame.GAME_UE4_7) ? 434 : 413))) : ((game < EGame.GAME_UE4_10) ? ((game >= EGame.GAME_UE4_9) ? 482 : 451) : ((game >= EGame.GAME_UE4_11) ? 498 : 482))) : ((game < EGame.GAME_UE4_20) ? ((game < EGame.GAME_UE4_16) ? ((game < EGame.GAME_UE4_14) ? ((game >= EGame.GAME_UE4_13) ? 505 : 504) : ((game >= EGame.GAME_UE4_15) ? 510 : 508)) : ((game < EGame.GAME_UE4_18) ? ((game >= EGame.GAME_UE4_17) ? 513 : 513) : ((game >= EGame.GAME_UE4_19) ? 516 : 514))) : ((game < EGame.GAME_UE4_24) ? ((game < EGame.GAME_UE4_22) ? ((game >= EGame.GAME_UE4_21) ? 517 : 516) : ((game >= EGame.GAME_UE4_23) ? 517 : 517)) : ((game < EGame.GAME_UE4_26) ? ((game >= EGame.GAME_UE4_25) ? 518 : 518) : ((game >= EGame.GAME_UE4_27) ? 522 : 522)))));
		return FPackageFileVersion.CreateUE4Version(version);
	}
}
