using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(FStructFallbackConverter))]
[SkipObjectRegistration]
public class FStructFallback : IUStruct, IPropertyHolder
{
	public List<FPropertyTag> Properties { get; }

	public FStructFallback()
	{
		Properties = new List<FPropertyTag>();
	}

	public FStructFallback(FAssetArchive Ar, string? structType)
		: this(Ar, (structType != null) ? new UScriptClass(structType) : null)
	{
	}

	public FStructFallback(FAssetArchive Ar, UStruct? structType = null)
	{
		if (Ar.HasUnversionedProperties)
		{
			if (structType == null)
			{
				throw new ArgumentException("For unversioned struct fallback the struct type cannot be null", "structType");
			}
			UObject.DeserializePropertiesUnversioned(Properties = new List<FPropertyTag>(), Ar, structType);
		}
		else
		{
			UObject.DeserializePropertiesTagged(Properties = new List<FPropertyTag>(), Ar);
		}
	}

	public T GetOrDefault<T>(string name, T defaultValue = default(T), StringComparison comparisonType = StringComparison.Ordinal)
	{
		return PropertyUtil.GetOrDefault(this, name, defaultValue, comparisonType);
	}

	public T Get<T>(string name, StringComparison comparisonType = StringComparison.Ordinal)
	{
		return PropertyUtil.Get<T>(this, name, comparisonType);
	}

	public bool TryGetValue<T>(out T obj, params string[] names)
	{
		foreach (string name in names)
		{
			T orDefault = GetOrDefault(name, default(T), StringComparison.OrdinalIgnoreCase);
			if (orDefault != null)
			{
				obj = orDefault;
				return true;
			}
		}
		obj = default(T);
		return false;
	}

	public bool TryGetAllValues<T>(out T[] obj, string name)
	{
		int num = -1;
		List<FPropertyTag> list = new List<FPropertyTag>();
		foreach (FPropertyTag property in Properties)
		{
			if (!(property.Name.Text != name))
			{
				list.Add(property);
				num = Math.Max(num, property.ArrayIndex);
			}
		}
		obj = new T[num + 1];
		foreach (FPropertyTag item in list)
		{
			obj[item.ArrayIndex] = (T)item.Tag.GetValue(typeof(T));
		}
		return obj.Length != 0;
	}
}
