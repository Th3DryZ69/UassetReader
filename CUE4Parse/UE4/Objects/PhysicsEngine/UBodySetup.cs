using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.PhysicsEngine;

public class UBodySetup : CUE4Parse.UE4.Assets.Exports.UObject
{
	public FGuid BodySetupGuid;

	public FFormatContainer? CookedFormatData;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		BodySetupGuid = Ar.Read<FGuid>();
		if (Ar.ReadBoolean())
		{
			if (Ar.Ver >= EUnrealEngineObjectUE4Version.STORE_HASCOOKEDDATA_FOR_BODYSETUP)
			{
				Ar.ReadBoolean();
			}
			CookedFormatData = new FFormatContainer(Ar);
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("BodySetupGuid");
		writer.WriteValue(BodySetupGuid.ToString());
		FFormatContainer? cookedFormatData = CookedFormatData;
		if (cookedFormatData == null || cookedFormatData.Formats.Count > 0)
		{
			writer.WritePropertyName("CookedFormatData");
			serializer.Serialize(writer, CookedFormatData);
		}
	}
}
