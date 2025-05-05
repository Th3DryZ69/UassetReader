using System;
using CUE4Parse.UE4.Objects.Core.Serialization;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

public class FAssetPackageDataConverter : JsonConverter<FAssetPackageData>
{
	public override void WriteJson(JsonWriter writer, FAssetPackageData value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("PackageName");
		serializer.Serialize(writer, value.PackageName);
		writer.WritePropertyName("DiskSize");
		serializer.Serialize(writer, value.DiskSize);
		writer.WritePropertyName("PackageGuid");
		serializer.Serialize(writer, value.PackageGuid);
		if (value.CookedHash != null)
		{
			writer.WritePropertyName("CookedHash");
			serializer.Serialize(writer, value.CookedHash);
		}
		if (value.FileVersionUE.FileVersionUE4 != 0 || value.FileVersionUE.FileVersionUE5 != 0)
		{
			writer.WritePropertyName("FileVersionUE");
			serializer.Serialize(writer, value.FileVersionUE);
		}
		if (value.FileVersionLicenseeUE != -1)
		{
			writer.WritePropertyName("FileVersionLicenseeUE");
			serializer.Serialize(writer, value.FileVersionLicenseeUE);
		}
		if (value.Flags != 0)
		{
			writer.WritePropertyName("Flags");
			serializer.Serialize(writer, value.Flags);
		}
		FCustomVersion[] versions = value.CustomVersions.Versions;
		if (versions != null && versions.Length > 0)
		{
			writer.WritePropertyName("CustomVersions");
			serializer.Serialize(writer, value.CustomVersions);
		}
		FName[] importedClasses = value.ImportedClasses;
		if (importedClasses != null && importedClasses.Length > 0)
		{
			writer.WritePropertyName("ImportedClasses");
			serializer.Serialize(writer, value.ImportedClasses);
		}
		writer.WriteEndObject();
	}

	public override FAssetPackageData ReadJson(JsonReader reader, Type objectType, FAssetPackageData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
