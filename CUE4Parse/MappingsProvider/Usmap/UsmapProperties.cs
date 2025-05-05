using System;
using System.Collections.Generic;

namespace CUE4Parse.MappingsProvider.Usmap;

public static class UsmapProperties
{
	public static Struct ParseStruct(TypeMappings context, FUsmapReader Ar, IReadOnlyList<string> nameLut)
	{
		string name = Ar.ReadName(nameLut);
		string superType = Ar.ReadName(nameLut);
		ushort propertyCount = Ar.Read<ushort>();
		ushort num = Ar.Read<ushort>();
		Dictionary<int, PropertyInfo> dictionary = new Dictionary<int, PropertyInfo>();
		for (int i = 0; i < num; i++)
		{
			PropertyInfo propertyInfo = ParsePropertyInfo(Ar, nameLut);
			for (int j = 0; j < propertyInfo.ArraySize; j++)
			{
				PropertyInfo propertyInfo2 = (PropertyInfo)propertyInfo.Clone();
				propertyInfo2.Index = j;
				dictionary[propertyInfo.Index + j] = propertyInfo2;
			}
		}
		return new Struct(context, name, superType, dictionary, propertyCount);
	}

	public static PropertyInfo ParsePropertyInfo(FUsmapReader Ar, IReadOnlyList<string> nameLut)
	{
		ushort index = Ar.Read<ushort>();
		byte value = Ar.Read<byte>();
		string name = Ar.ReadName(nameLut);
		PropertyType mappingType = ParsePropertyType(Ar, nameLut);
		return new PropertyInfo(index, name, mappingType, value);
	}

	public static PropertyType ParsePropertyType(FUsmapReader Ar, IReadOnlyList<string> nameLut)
	{
		EPropertyType ePropertyType = Ar.Read<EPropertyType>();
		string type = Enum.GetName(ePropertyType) ?? string.Empty;
		string structType = null;
		PropertyType innerType = null;
		PropertyType valueType = null;
		string enumName = null;
		bool? isEnumAsByte = null;
		switch (ePropertyType)
		{
		case EPropertyType.EnumProperty:
			innerType = ParsePropertyType(Ar, nameLut);
			enumName = Ar.ReadName(nameLut);
			break;
		case EPropertyType.StructProperty:
			structType = Ar.ReadName(nameLut);
			break;
		case EPropertyType.ArrayProperty:
		case EPropertyType.SetProperty:
			innerType = ParsePropertyType(Ar, nameLut);
			break;
		case EPropertyType.MapProperty:
			innerType = ParsePropertyType(Ar, nameLut);
			valueType = ParsePropertyType(Ar, nameLut);
			break;
		}
		return new PropertyType(type, structType, innerType, valueType, enumName, isEnumAsByte);
	}
}
