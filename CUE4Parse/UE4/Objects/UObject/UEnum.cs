using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

[SkipObjectRegistration]
public class UEnum : CUE4Parse.UE4.Assets.Exports.UObject
{
	public enum ECppForm : byte
	{
		Regular,
		Namespaced,
		EnumClass
	}

	public (FName, long)[] Names;

	public ECppForm CppForm;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		if (Ar.Ver < EUnrealEngineObjectUE4Version.TIGHTLY_PACKED_ENUMS)
		{
			FName[] array = Ar.ReadArray(Ar.ReadFName);
			Names = new(FName, long)[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				Names[i] = (array[i], i);
			}
		}
		else if (FCoreObjectVersion.Get(Ar) < FCoreObjectVersion.Type.EnumProperties)
		{
			(FName, byte)[] array2 = Ar.ReadArray(() => (Ar.ReadFName(), Ar.Read<byte>()));
			Names = new(FName, long)[array2.Length];
			for (int num = 0; num < array2.Length; num++)
			{
				(FName, long)[] names = Names;
				int num2 = num;
				(FName, byte) tuple = array2[num];
				names[num2] = (tuple.Item1, tuple.Item2);
			}
		}
		else
		{
			Names = Ar.ReadArray(() => (Ar.ReadFName(), Ar.Read<long>()));
		}
		if (Ar.Ver < EUnrealEngineObjectUE4Version.ENUM_CLASS_SUPPORT)
		{
			bool flag = Ar.ReadBoolean();
			CppForm = (flag ? ECppForm.Namespaced : ECppForm.Regular);
		}
		else
		{
			CppForm = Ar.Read<ECppForm>();
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("Names");
		writer.WriteStartObject();
		(FName, long)[] names = Names;
		for (int i = 0; i < names.Length; i++)
		{
			var (fName, value) = names[i];
			writer.WritePropertyName(fName.Text);
			writer.WriteValue(value);
		}
		writer.WriteEndObject();
		writer.WritePropertyName("CppForm");
		writer.WriteValue(CppForm.ToString());
	}
}
