using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.Utils;
using Ionic.Crc;
using Serilog;

namespace CUE4Parse.GameTypes.FN.Assets.Exports;

public class FActorComponentRecord
{
	public FName ComponentName;

	public FSoftObjectPath ComponentClass;

	public byte[]? ComponentData;

	public uint DataHash;

	public FActorComponentRecord(FLevelSaveRecordArchive Ar)
	{
		ComponentName = Ar.ReadFName();
		if (Ar.Version < ELevelSaveRecordVersion.SoftActorComponentClassReferences)
		{
			UObject uObject = Ar.ReadUObject();
			if (uObject != null)
			{
				string pathName = uObject.GetPathName();
				ComponentClass = new FSoftObjectPath(pathName.SubstringBeforeLast(':'), "");
			}
			else
			{
				ComponentClass = default(FSoftObjectPath);
			}
		}
		else
		{
			ComponentClass = new FSoftObjectPath(Ar);
		}
		ComponentData = Ar.ReadArray<byte>();
		CRC32 cRC = new CRC32();
		cRC.SlurpBlock(ComponentData, 0, ComponentData.Length);
		uint crc32Result = (uint)cRC.Crc32Result;
		if (Ar.Version < ELevelSaveRecordVersion.AddingDataHash)
		{
			DataHash = crc32Result;
			return;
		}
		DataHash = Ar.Read<uint>();
		if (DataHash != crc32Result)
		{
			Log.Error("FActorComponentRecord::Serialize failed to deserialize data for: {0} dropping corrupted data.", ComponentClass.ToString());
			ComponentData = null;
			DataHash = 0u;
		}
	}
}
