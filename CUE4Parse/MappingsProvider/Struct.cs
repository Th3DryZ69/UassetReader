using System;
using System.Collections.Generic;

namespace CUE4Parse.MappingsProvider;

public class Struct
{
	public readonly TypeMappings? Context;

	public string Name;

	public string? SuperType;

	public Lazy<Struct?> Super;

	public Dictionary<int, PropertyInfo> Properties;

	public int PropertyCount;

	public Struct(TypeMappings? context, string name, int propertyCount)
	{
		Context = context;
		Name = name;
		PropertyCount = propertyCount;
	}

	public Struct(TypeMappings? context, string name, string? superType, Dictionary<int, PropertyInfo> properties, int propertyCount)
		: this(context, name, propertyCount)
	{
		SuperType = superType;
		Super = new Lazy<Struct>(() => (SuperType != null && Context != null && Context.Types.TryGetValue(SuperType, out Struct value)) ? value : null);
		Properties = properties;
	}

	public bool TryGetValue(int i, out PropertyInfo info)
	{
		if (!Properties.TryGetValue(i, out info))
		{
			if (i >= PropertyCount && Super.Value != null)
			{
				return Super.Value.TryGetValue(i - PropertyCount, out info);
			}
			return false;
		}
		return true;
	}
}
