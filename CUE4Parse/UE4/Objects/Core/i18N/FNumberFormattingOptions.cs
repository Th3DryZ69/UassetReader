using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Objects.Core.i18N;

public class FNumberFormattingOptions : IUStruct
{
	private const int _DBL_DIG = 15;

	private const int _DBL_MAX_10_EXP = 308;

	public bool AlwaysSign;

	public bool UseGrouping;

	public ERoundingMode RoundingMode;

	public int MinimumIntegralDigits;

	public int MaximumIntegralDigits;

	public int MinimumFractionalDigits;

	public int MaximumFractionalDigits;

	public FNumberFormattingOptions()
	{
		AlwaysSign = false;
		UseGrouping = true;
		RoundingMode = ERoundingMode.HalfToEven;
		MinimumIntegralDigits = 1;
		MaximumIntegralDigits = 324;
		MinimumFractionalDigits = 0;
		MaximumFractionalDigits = 3;
	}

	public FNumberFormattingOptions(FAssetArchive Ar)
	{
		AlwaysSign = Ar.ReadBoolean();
		UseGrouping = Ar.ReadBoolean();
		RoundingMode = Ar.Read<ERoundingMode>();
		MinimumIntegralDigits = Ar.Read<int>();
		MaximumIntegralDigits = Ar.Read<int>();
		MinimumFractionalDigits = Ar.Read<int>();
		MaximumFractionalDigits = Ar.Read<int>();
	}
}
