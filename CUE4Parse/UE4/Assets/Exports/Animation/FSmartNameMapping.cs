using System.Collections.Generic;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine.Curves;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

[JsonConverter(typeof(FSmartNameMappingConverter))]
public class FSmartNameMapping
{
	public readonly Dictionary<FName, FGuid> GuidMap;

	public readonly Dictionary<ushort, FName> UidMap;

	public readonly Dictionary<FName, FCurveMetaData> CurveMetaDataMap;

	public FSmartNameMapping(FArchive Ar)
	{
		FFrameworkObjectVersion.Type type = FFrameworkObjectVersion.Get(Ar);
		FAnimPhysObjectVersion.Type type2 = FAnimPhysObjectVersion.Get(Ar);
		if (type >= FFrameworkObjectVersion.Type.SmartNameRefactor)
		{
			if (type2 < FAnimPhysObjectVersion.Type.SmartNameRefactorForDeterministicCooking)
			{
				int num = Ar.Read<int>();
				GuidMap = new Dictionary<FName, FGuid>(num);
				for (int i = 0; i < num; i++)
				{
					GuidMap[Ar.ReadFName()] = Ar.Read<FGuid>();
				}
			}
		}
		else if (Ar.Ver >= EUnrealEngineObjectUE4Version.SKELETON_ADD_SMARTNAMES)
		{
			Ar.Read<ushort>();
			int num2 = Ar.Read<int>();
			UidMap = new Dictionary<ushort, FName>(num2);
			for (int j = 0; j < num2; j++)
			{
				UidMap[Ar.Read<ushort>()] = Ar.ReadFName();
			}
		}
		if (type >= FFrameworkObjectVersion.Type.MoveCurveTypesToSkeleton)
		{
			int num3 = Ar.Read<int>();
			CurveMetaDataMap = new Dictionary<FName, FCurveMetaData>(num3);
			for (int k = 0; k < num3; k++)
			{
				CurveMetaDataMap[Ar.ReadFName()] = new FCurveMetaData(Ar, type2);
			}
		}
	}
}
