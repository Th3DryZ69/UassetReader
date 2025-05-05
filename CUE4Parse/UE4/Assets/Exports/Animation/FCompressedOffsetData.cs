using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FCompressedOffsetData : FCompressedOffsetDataBase<int>
{
	public FCompressedOffsetData(int stripSize = 2)
		: base(stripSize)
	{
	}

	public FCompressedOffsetData(FArchive Ar)
		: base(Ar)
	{
	}
}
