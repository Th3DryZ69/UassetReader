using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FFieldPath
{
	public List<FName> Path;

	public FPackageIndex ResolvedOwner;

	public FFieldPath()
	{
		Path = new List<FName>();
		ResolvedOwner = new FPackageIndex();
	}

	public FFieldPath(FAssetArchive Ar)
	{
		int num = Ar.Read<int>();
		Path = new List<FName>(num);
		for (int i = 0; i < num; i++)
		{
			Path.Add(Ar.ReadFName());
		}
		if (Path.Count == 1 && Path[0].IsNone)
		{
			Path.Clear();
		}
		if (FFortniteMainBranchObjectVersion.Get(Ar) >= FFortniteMainBranchObjectVersion.Type.FFieldPathOwnerSerialization || FReleaseObjectVersion.Get(Ar) >= FReleaseObjectVersion.Type.FFieldPathOwnerSerialization)
		{
			ResolvedOwner = new FPackageIndex(Ar);
		}
	}

	public FFieldPath(FKismetArchive Ar)
	{
		int index = Ar.Index;
		int num = Ar.Read<int>();
		Path = new List<FName>(num);
		for (int i = 0; i < num; i++)
		{
			Path.Add(Ar.ReadFName());
		}
		if (Path.Count == 1 && Path[0].IsNone)
		{
			Path.Clear();
		}
		if (FFortniteMainBranchObjectVersion.Get(Ar) >= FFortniteMainBranchObjectVersion.Type.FFieldPathOwnerSerialization || FReleaseObjectVersion.Get(Ar) >= FReleaseObjectVersion.Type.FFieldPathOwnerSerialization)
		{
			ResolvedOwner = new FPackageIndex(Ar);
		}
		Ar.Index = index + 8;
	}

	protected internal void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		if (ResolvedOwner == null)
		{
			serializer.Serialize(writer, this);
			return;
		}
		if (ResolvedOwner.IsNull)
		{
			writer.WriteNull();
			return;
		}
		if (!ResolvedOwner.TryLoad(out UField export))
		{
			serializer.Serialize(writer, this);
			return;
		}
		UField uField = export;
		if (!(uField is UScriptClass))
		{
			if (uField is UStruct uStruct)
			{
				UStruct uStruct2 = uStruct;
				if (Path.Count > 0 && uStruct2.GetProperty(Path[0], out FField property))
				{
					writer.WriteStartObject();
					writer.WritePropertyName("Owner");
					serializer.Serialize(writer, ResolvedOwner);
					writer.WritePropertyName("Property");
					serializer.Serialize(writer, property);
					writer.WriteEndObject();
					return;
				}
			}
			serializer.Serialize(writer, this);
		}
		else
		{
			serializer.Serialize(writer, this);
		}
	}
}
