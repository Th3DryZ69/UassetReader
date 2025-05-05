using System;
using System.Collections.Generic;
using System.Reflection;
using CUE4Parse.UE4.Assets.Exports;

namespace CUE4Parse.UE4.Assets;

public static class ObjectTypeRegistry
{
	private static readonly Type _propertyHolderType;

	private static readonly Dictionary<string, Type> _classes;

	static ObjectTypeRegistry()
	{
		_propertyHolderType = typeof(IPropertyHolder);
		_classes = new Dictionary<string, Type>();
		RegisterEngine(_propertyHolderType.Assembly);
	}

	public static void RegisterEngine(Assembly assembly)
	{
		Type typeFromHandle = typeof(SkipObjectRegistrationAttribute);
		foreach (TypeInfo definedType in assembly.DefinedTypes)
		{
			if (!definedType.IsAbstract && !definedType.IsInterface && _propertyHolderType.IsAssignableFrom(definedType) && definedType.GetCustomAttributes(typeFromHandle, inherit: false).Length == 0)
			{
				RegisterClass(definedType);
			}
		}
	}

	public static void RegisterClass(Type type)
	{
		string text = type.Name;
		if ((text[0] == 'U' || text[0] == 'A') && char.IsUpper(text[1]))
		{
			string text2 = text;
			text = text2.Substring(1, text2.Length - 1);
		}
		RegisterClass(text, type);
	}

	public static void RegisterClass(string serializedName, Type type)
	{
		lock (_classes)
		{
			_classes[serializedName] = type;
		}
	}

	public static Type? GetClass(string serializedName)
	{
		lock (_classes)
		{
			if (!_classes.TryGetValue(serializedName, out Type value) && serializedName.EndsWith("_C"))
			{
				_classes.TryGetValue(serializedName.Substring(0, serializedName.Length - 2), out value);
			}
			return value;
		}
	}

	public static Type? Get(string serializedName)
	{
		return GetClass(serializedName);
	}
}
