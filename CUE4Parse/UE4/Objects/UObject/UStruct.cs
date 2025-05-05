using System;
using System.Collections.Generic;
using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Kismet;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Objects.UObject;

[SkipObjectRegistration]
public class UStruct : UField
{
	public FPackageIndex SuperStruct;

	public FPackageIndex[] Children;

	public FField[] ChildProperties;

	public KismetExpression[] ScriptBytecode;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		SuperStruct = new FPackageIndex(Ar);
		if (FFrameworkObjectVersion.Get(Ar) < FFrameworkObjectVersion.Type.RemoveUField_Next)
		{
			FPackageIndex fPackageIndex = new FPackageIndex(Ar);
			Children = (fPackageIndex.IsNull ? Array.Empty<FPackageIndex>() : new FPackageIndex[1] { fPackageIndex });
		}
		else
		{
			Children = Ar.ReadArray(() => new FPackageIndex(Ar));
		}
		if (FCoreObjectVersion.Get(Ar) >= FCoreObjectVersion.Type.FProperties)
		{
			DeserializeProperties(Ar);
		}
		Ar.Read<int>();
		int num = Ar.Read<int>();
		IFileProvider? provider = Ar.Owner.Provider;
		if (provider != null && provider.ReadScriptData && num > 0)
		{
			using (FKismetArchive fKismetArchive = new FKismetArchive(base.Name, Ar.ReadBytes(num), Ar.Owner, Ar.Versions))
			{
				try
				{
					List<KismetExpression> list = new List<KismetExpression>();
					while (fKismetArchive.Position < fKismetArchive.Length)
					{
						list.Add(fKismetArchive.ReadExpression());
					}
					ScriptBytecode = list.ToArray();
					return;
				}
				catch (Exception exception)
				{
					Log.Warning(exception, "Failed to serialize script bytecode in " + base.Name);
					return;
				}
			}
		}
		Ar.Position += num;
	}

	protected void DeserializeProperties(FAssetArchive Ar)
	{
		ChildProperties = Ar.ReadArray(delegate
		{
			FField fField = FField.Construct(Ar.ReadFName());
			fField.Deserialize(Ar);
			return fField;
		});
	}

	public bool GetProperty(FName name, out FField? property)
	{
		property = null;
		if (ChildProperties == null)
		{
			return false;
		}
		FField[] childProperties = ChildProperties;
		foreach (FField fField in childProperties)
		{
			if (fField.Name.Text == name.Text)
			{
				property = fField;
				return true;
			}
		}
		return false;
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		FPackageIndex superStruct = SuperStruct;
		if (superStruct != null && !superStruct.IsNull)
		{
			writer.WritePropertyName("SuperStruct");
			serializer.Serialize(writer, SuperStruct);
		}
		FPackageIndex[] children = Children;
		if (children != null && children.Length > 0)
		{
			writer.WritePropertyName("Children");
			serializer.Serialize(writer, Children);
		}
		FField[] childProperties = ChildProperties;
		if (childProperties != null && childProperties.Length > 0)
		{
			writer.WritePropertyName("ChildProperties");
			serializer.Serialize(writer, ChildProperties);
		}
		KismetExpression[] scriptBytecode = ScriptBytecode;
		if (scriptBytecode != null && scriptBytecode.Length > 0)
		{
			writer.WritePropertyName("ScriptBytecode");
			writer.WriteStartArray();
			scriptBytecode = ScriptBytecode;
			foreach (KismetExpression obj in scriptBytecode)
			{
				writer.WriteStartObject();
				obj.WriteJson(writer, serializer, bAddIndex: true);
				writer.WriteEndObject();
			}
			writer.WriteEndArray();
		}
	}
}
