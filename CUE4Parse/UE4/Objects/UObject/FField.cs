using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

[JsonConverter(typeof(FFieldConverter))]
public class FField
{
	public FName Name;

	public EObjectFlags Flags;

	public virtual void Deserialize(FAssetArchive Ar)
	{
		Name = Ar.ReadFName();
		Flags = Ar.Read<EObjectFlags>();
	}

	protected internal virtual void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		writer.WritePropertyName("Type");
		string name = GetType().Name;
		serializer.Serialize(writer, name.Substring(1, name.Length - 1));
		writer.WritePropertyName("Name");
		serializer.Serialize(writer, Name);
		if (Flags != EObjectFlags.RF_NoFlags)
		{
			writer.WritePropertyName("Flags");
			writer.WriteValue(Flags.ToStringBitfield());
		}
	}

	public override string ToString()
	{
		return Name.Text;
	}

	public static FField Construct(FName fieldTypeName)
	{
		return fieldTypeName.Text switch
		{
			"ArrayProperty" => new FArrayProperty(), 
			"BoolProperty" => new FBoolProperty(), 
			"ByteProperty" => new FByteProperty(), 
			"ClassProperty" => new FClassProperty(), 
			"DelegateProperty" => new FDelegateProperty(), 
			"EnumProperty" => new FEnumProperty(), 
			"FieldPathProperty" => new FFieldPathProperty(), 
			"DoubleProperty" => new FDoubleProperty(), 
			"FloatProperty" => new FFloatProperty(), 
			"Int16Property" => new FInt16Property(), 
			"Int64Property" => new FInt64Property(), 
			"Int8Property" => new FInt8Property(), 
			"IntProperty" => new FIntProperty(), 
			"InterfaceProperty" => new FInterfaceProperty(), 
			"MapProperty" => new FMapProperty(), 
			"MulticastDelegateProperty" => new FMulticastDelegateProperty(), 
			"MulticastInlineDelegateProperty" => new FMulticastInlineDelegateProperty(), 
			"NameProperty" => new FNameProperty(), 
			"ObjectProperty" => new FObjectProperty(), 
			"SetProperty" => new FSetProperty(), 
			"SoftClassProperty" => new FSoftClassProperty(), 
			"SoftObjectProperty" => new FSoftObjectProperty(), 
			"StrProperty" => new FStrProperty(), 
			"StructProperty" => new FStructProperty(), 
			"TextProperty" => new FTextProperty(), 
			"UInt16Property" => new FUInt16Property(), 
			"UInt32Property" => new FUInt32Property(), 
			"UInt64Property" => new FUInt64Property(), 
			_ => throw new ParserException("Unsupported serialized property type " + fieldTypeName), 
		};
	}

	public static FField? SerializeSingleField(FAssetArchive Ar)
	{
		FName fieldTypeName = Ar.ReadFName();
		if (!fieldTypeName.IsNone)
		{
			FField fField = Construct(fieldTypeName);
			fField.Deserialize(Ar);
			return fField;
		}
		return null;
	}
}
