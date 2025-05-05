using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CUE4Parse.MappingsProvider;

public abstract class JsonTypeMappingsProvider : AbstractTypeMappingsProvider
{
	protected static JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
	{
		ContractResolver = new DefaultContractResolver
		{
			NamingStrategy = new CamelCaseNamingStrategy(processDictionaryKeys: false, overrideSpecifiedNames: false)
		}
	};

	public override TypeMappings? MappingsForGame { get; protected set; }

	protected bool AddStructs(string structsJson)
	{
		if (MappingsForGame == null)
		{
			TypeMappings typeMappings = (MappingsForGame = new TypeMappings());
		}
		foreach (JToken item in JArray.Parse(structsJson))
		{
			if (item != null)
			{
				Struct obj = ParseStruct(MappingsForGame, item);
				MappingsForGame.Types[obj.Name] = obj;
			}
		}
		return true;
	}

	private Struct ParseStruct(TypeMappings context, JToken structToken)
	{
		string name = structToken["name"].ToObject<string>();
		string superType = structToken["superType"]?.ToObject<string>();
		JArray obj = (JArray)structToken["properties"];
		Dictionary<int, PropertyInfo> dictionary = new Dictionary<int, PropertyInfo>();
		foreach (JToken item in obj)
		{
			if (item != null)
			{
				PropertyInfo propertyInfo = ParsePropertyInfo(item);
				for (int i = 0; i < propertyInfo.ArraySize; i++)
				{
					dictionary[propertyInfo.Index + i] = propertyInfo;
				}
			}
		}
		int propertyCount = structToken["propertyCount"].ToObject<int>();
		return new Struct(context, name, superType, dictionary, propertyCount);
	}

	private PropertyInfo ParsePropertyInfo(JToken propToken)
	{
		int index = propToken["index"].ToObject<int>();
		string name = propToken["name"].ToObject<string>();
		int? arraySize = propToken["arraySize"]?.ToObject<int>();
		PropertyType mappingType = ParsePropertyType(propToken["mappingType"]);
		return new PropertyInfo(index, name, mappingType, arraySize);
	}

	private PropertyType ParsePropertyType(JToken typeToken)
	{
		string? type = typeToken["type"].ToObject<string>();
		string structType = typeToken["structType"]?.ToObject<string>();
		JToken jToken = typeToken["innerType"];
		PropertyType innerType = ((jToken != null) ? ParsePropertyType(jToken) : null);
		JToken jToken2 = typeToken["valueType"];
		PropertyType valueType = ((jToken2 != null) ? ParsePropertyType(jToken2) : null);
		string enumName = typeToken["enumName"]?.ToObject<string>();
		bool? isEnumAsByte = typeToken["isEnumAsByte"]?.ToObject<bool>();
		return new PropertyType(type, structType, innerType, valueType, enumName, isEnumAsByte);
	}

	protected void AddEnums(string enumsJson)
	{
		if (MappingsForGame == null)
		{
			TypeMappings typeMappings = (MappingsForGame = new TypeMappings());
		}
		foreach (JToken item in JArray.Parse(enumsJson))
		{
			if (item != null)
			{
				string[] source = item["values"].ToObject<string[]>();
				int i = 0;
				MappingsForGame.Enums[item["name"].ToObject<string>()] = source.ToDictionary((string it) => i++);
			}
		}
	}
}
