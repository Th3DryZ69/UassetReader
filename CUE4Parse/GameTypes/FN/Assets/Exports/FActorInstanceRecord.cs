using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.GameTypes.FN.Assets.Exports;

public class FActorInstanceRecord
{
	public ulong RecordID;

	public ulong TemplateRecordID;

	public FName ActorId;

	public FGuid ActorGuid;

	public FTransform Transform;

	public FActorInstanceRecord(FLevelSaveRecordArchive Ar)
	{
		if (Ar.Version < ELevelSaveRecordVersion.TimestampConversion)
		{
			RecordID = Ar.Read<ulong>();
		}
		else
		{
			RecordID = Ar.Read<ulong>();
		}
		TemplateRecordID = Ar.Read<ulong>();
		if (Ar.Version < ELevelSaveRecordVersion.UsingSaveActorGUID)
		{
			ActorId = Ar.ReadFName();
		}
		else
		{
			if (Ar.Version != ELevelSaveRecordVersion.UsingSaveActorGUID)
			{
				ActorId = Ar.ReadFName();
			}
			ActorGuid = Ar.Read<FGuid>();
		}
		Transform = new FTransform(Ar);
	}
}
