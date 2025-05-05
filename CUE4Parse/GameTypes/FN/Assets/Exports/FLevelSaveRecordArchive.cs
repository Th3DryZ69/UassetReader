using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.GameTypes.FN.Assets.Exports;

public class FLevelSaveRecordArchive : FObjectAndNameAsStringProxyArchive
{
	public readonly ELevelSaveRecordVersion Version;

	public FLevelSaveRecordArchive(FAssetArchive Ar, ELevelSaveRecordVersion version)
		: base(Ar)
	{
		Version = version;
	}

	public FLevelSaveRecordArchive(FArchive Ar, ELevelSaveRecordVersion version)
		: base(Ar)
	{
		Version = version;
	}

	public override object Clone()
	{
		return new FLevelSaveRecordArchive((FArchive)InnerArchive.Clone(), Version);
	}
}
