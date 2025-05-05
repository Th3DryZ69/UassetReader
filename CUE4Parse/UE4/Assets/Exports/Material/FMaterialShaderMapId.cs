using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialShaderMapId
{
	public EMaterialQualityLevel QualityLevel;

	public ERHIFeatureLevel FeatureLevel;

	public FSHAHash CookedShaderMapIdHash;

	public FPlatformTypeLayoutParameters? LayoutParams;

	public FMaterialShaderMapId(FArchive Ar)
	{
		bool num = Ar.Ver < EUnrealEngineObjectUE4Version.PURGED_FMATERIAL_COMPILE_OUTPUTS;
		if (!num)
		{
			QualityLevel = ((Ar.Game >= EGame.GAME_UE5_2) ? ((EMaterialQualityLevel)Ar.Read<byte>()) : ((EMaterialQualityLevel)Ar.Read<int>()));
			FeatureLevel = (ERHIFeatureLevel)Ar.Read<int>();
		}
		else
		{
			Ar.Read<byte>();
		}
		CookedShaderMapIdHash = new FSHAHash(Ar);
		if (!num)
		{
			LayoutParams = new FPlatformTypeLayoutParameters(Ar);
		}
	}
}
