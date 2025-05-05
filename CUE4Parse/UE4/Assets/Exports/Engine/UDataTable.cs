using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Engine;

public class UDataTable : UObject
{
	public Dictionary<FName, FStructFallback> RowMap { get; private set; }

	protected string? RowStructName { get; set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		UStruct uStruct = null;
		if (string.IsNullOrEmpty(RowStructName))
		{
			uStruct = GetOrDefault<FPackageIndex>("RowStruct").Load<UStruct>();
		}
		int num = Ar.Read<int>();
		RowMap = new Dictionary<FName, FStructFallback>(num);
		for (int i = 0; i < num; i++)
		{
			FName key = Ar.ReadFName();
			RowMap[key] = ((uStruct != null) ? new FStructFallback(Ar, uStruct) : new FStructFallback(Ar, RowStructName));
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("Rows");
		serializer.Serialize(writer, RowMap);
	}
}
