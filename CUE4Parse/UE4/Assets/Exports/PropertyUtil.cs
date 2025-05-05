using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Assets.Objects;

namespace CUE4Parse.UE4.Assets.Exports;

public static class PropertyUtil
{
	public static T GetOrDefault<T>(IPropertyHolder holder, string name, T defaultValue = default(T), StringComparison comparisonType = StringComparison.Ordinal)
	{
		foreach (FPropertyTag property in holder.Properties)
		{
			if (property.Name.Text.Equals(name, comparisonType))
			{
				object obj = property.Tag?.GetValue(typeof(T));
				if (obj is T)
				{
					return (T)obj;
				}
			}
		}
		return defaultValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Lazy<T> GetOrDefaultLazy<T>(IPropertyHolder holder, string name, T defaultValue = default(T), StringComparison comparisonType = StringComparison.Ordinal)
	{
		return new Lazy<T>(() => GetOrDefault(holder, name, defaultValue, comparisonType));
	}

	public static T Get<T>(IPropertyHolder holder, string name, StringComparison comparisonType = StringComparison.Ordinal)
	{
		object value = (holder.Properties.FirstOrDefault((FPropertyTag it) => it.Name.Text.Equals(name, comparisonType))?.Tag ?? throw new NullReferenceException(holder.GetType().Name + " does not have a property '" + name + "'")).GetValue(typeof(T));
		if (value is T)
		{
			return (T)value;
		}
		throw new NullReferenceException($"Couldn't get property '{name}' of type {typeof(T).Name} in {holder.GetType().Name}");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Lazy<T> GetLazy<T>(IPropertyHolder holder, string name, StringComparison comparisonType = StringComparison.Ordinal)
	{
		return new Lazy<T>(() => Get<T>(holder, name, comparisonType));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T GetByIndex<T>(IPropertyHolder holder, int index)
	{
		object value = (holder.Properties[index]?.Tag ?? throw new NullReferenceException($"{holder.GetType().Name} does not have a property at index '{index}'")).GetValue(typeof(T));
		if (value is T)
		{
			return (T)value;
		}
		throw new NullReferenceException($"Couldn't get property of type {typeof(T).Name} at index '{index}' in {holder.GetType().Name}");
	}
}
