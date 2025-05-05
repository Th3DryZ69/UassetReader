using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine;

public class UBlueprintGeneratedClass : UClass
{
	public Dictionary<FName, string>? EditorTags;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		if (FFortniteMainBranchObjectVersion.Get(Ar) >= FFortniteMainBranchObjectVersion.Type.BPGCCookedEditorTags && validPos - Ar.Position > 4)
		{
			int num = Ar.Read<int>();
			EditorTags = new Dictionary<FName, string>();
			for (int i = 0; i < num; i++)
			{
				EditorTags[Ar.ReadFName()] = Ar.ReadFString();
			}
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		Dictionary<FName, string> editorTags = EditorTags;
		if (editorTags != null && editorTags.Count > 0)
		{
			writer.WritePropertyName("EditorTags");
			serializer.Serialize(writer, EditorTags);
		}
	}
}
