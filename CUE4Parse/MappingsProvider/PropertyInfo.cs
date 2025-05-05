using System;

namespace CUE4Parse.MappingsProvider;

public class PropertyInfo : ICloneable
{
	public int Index;

	public string Name;

	public int? ArraySize;

	public PropertyType MappingType;

	public PropertyInfo(int index, string name, PropertyType mappingType, int? arraySize = null)
	{
		Index = index;
		Name = name;
		ArraySize = arraySize;
		MappingType = mappingType;
	}

	public override string ToString()
	{
		return $"{Index}/{ArraySize - 1} -> {Name}";
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
