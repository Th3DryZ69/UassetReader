using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine;

public class UUserDefinedStruct : UStruct
{
	public EUserDefinedStructureStatus Status;

	public uint StructFlags;

	public List<FPropertyTag>? DefaultProperties { get; set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		Status = GetOrDefault("Status", EUserDefinedStructureStatus.UDSS_UpToDate);
		if (!Flags.HasFlag(EObjectFlags.RF_ClassDefaultObject) && Status == EUserDefinedStructureStatus.UDSS_UpToDate)
		{
			StructFlags = Ar.Read<uint>();
			if (Ar.HasUnversionedProperties)
			{
				List<FPropertyTag> properties = (DefaultProperties = new List<FPropertyTag>());
				CUE4Parse.UE4.Assets.Exports.UObject.DeserializePropertiesUnversioned(properties, Ar, this);
			}
			else
			{
				List<FPropertyTag> properties = (DefaultProperties = new List<FPropertyTag>());
				CUE4Parse.UE4.Assets.Exports.UObject.DeserializePropertiesTagged(properties, Ar);
			}
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("StructFlags");
		writer.WriteValue(StructFlags);
		List<FPropertyTag> defaultProperties = DefaultProperties;
		if (defaultProperties == null || defaultProperties.Count <= 0)
		{
			return;
		}
		writer.WritePropertyName("DefaultProperties");
		writer.WriteStartObject();
		foreach (FPropertyTag defaultProperty in DefaultProperties)
		{
			writer.WritePropertyName(defaultProperty.Name.Text);
			serializer.Serialize(writer, defaultProperty.Tag);
		}
		writer.WriteEndObject();
	}
}
