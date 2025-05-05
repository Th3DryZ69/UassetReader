using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Objects.UObject;

[SkipObjectRegistration]
public class UClass : UStruct
{
	public class FImplementedInterface
	{
		public FPackageIndex Class;

		public int PointerOffset;

		public bool bImplementedByK2;

		public FImplementedInterface(FAssetArchive Ar)
		{
			Class = new FPackageIndex(Ar);
			PointerOffset = Ar.Read<int>();
			bImplementedByK2 = Ar.ReadBoolean();
		}
	}

	public bool bCooked;

	public uint ClassFlags;

	public FPackageIndex ClassWithin;

	public FPackageIndex ClassGeneratedBy;

	public FName ClassConfigName;

	public FPackageIndex ClassDefaultObject;

	public Dictionary<FName, FPackageIndex> FuncMap;

	public FImplementedInterface[] Interfaces;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		if (Ar.Game == EGame.GAME_AWayOut)
		{
			Ar.Position += 4L;
		}
		FuncMap = new Dictionary<FName, FPackageIndex>();
		int num = Ar.Read<int>();
		for (int i = 0; i < num; i++)
		{
			FuncMap[Ar.ReadFName()] = new FPackageIndex(Ar);
		}
		ClassFlags = Ar.Read<uint>();
		if (Ar.Game == EGame.GAME_StarWarsJediFallenOrder || Ar.Game == EGame.GAME_StarWarsJediSurvivor)
		{
			Ar.Position += 4L;
		}
		ClassWithin = new FPackageIndex(Ar);
		ClassConfigName = Ar.ReadFName();
		ClassGeneratedBy = new FPackageIndex(Ar);
		Interfaces = Ar.ReadArray(() => new FImplementedInterface(Ar));
		Ar.ReadBoolean();
		Ar.ReadFName();
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.ADD_COOKED_TO_UCLASS)
		{
			bCooked = Ar.ReadBoolean();
		}
		ClassDefaultObject = new FPackageIndex(Ar);
	}

	public CUE4Parse.UE4.Assets.Exports.UObject? ConstructObject()
	{
		Type type = ObjectTypeRegistry.Get(base.Name);
		if (type != null)
		{
			try
			{
				if (Activator.CreateInstance(type) is CUE4Parse.UE4.Assets.Exports.UObject result)
				{
					return result;
				}
				Log.Warning("Class {Type} did have a valid constructor but does not inherit UObject", type);
			}
			catch (Exception exception)
			{
				Log.Warning(exception, "Class {Type} could not be constructed", type);
			}
		}
		return null;
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		Dictionary<FName, FPackageIndex> funcMap = FuncMap;
		if (funcMap != null && funcMap.Count > 0)
		{
			writer.WritePropertyName("FuncMap");
			serializer.Serialize(writer, FuncMap);
		}
		if (ClassFlags != 0)
		{
			writer.WritePropertyName("ClassFlags");
			serializer.Serialize(writer, ClassFlags);
		}
		FPackageIndex classWithin = ClassWithin;
		if (classWithin != null && !classWithin.IsNull)
		{
			writer.WritePropertyName("ClassWithin");
			serializer.Serialize(writer, ClassWithin);
		}
		if (!ClassConfigName.IsNone)
		{
			writer.WritePropertyName("ClassConfigName");
			serializer.Serialize(writer, ClassConfigName);
		}
		classWithin = ClassGeneratedBy;
		if (classWithin != null && !classWithin.IsNull)
		{
			writer.WritePropertyName("ClassGeneratedBy");
			serializer.Serialize(writer, ClassGeneratedBy);
		}
		FImplementedInterface[] interfaces = Interfaces;
		if (interfaces != null && interfaces.Length > 0)
		{
			writer.WritePropertyName("Interfaces");
			serializer.Serialize(writer, Interfaces);
		}
		if (bCooked)
		{
			writer.WritePropertyName("bCooked");
			serializer.Serialize(writer, bCooked);
		}
		classWithin = ClassDefaultObject;
		if (classWithin != null && !classWithin.IsNull)
		{
			writer.WritePropertyName("ClassDefaultObject");
			serializer.Serialize(writer, ClassDefaultObject);
		}
	}
}
