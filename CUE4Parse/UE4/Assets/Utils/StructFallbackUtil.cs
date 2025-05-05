using System;
using System.Reflection;
using CUE4Parse.UE4.Assets.Objects;

namespace CUE4Parse.UE4.Assets.Utils;

public static class StructFallbackUtil
{
	public static ObjectMapper? ObjectMapper = new DefaultObjectMapper();

	public static object? MapToClass(this FStructFallback? fallback, Type type)
	{
		if (fallback == null)
		{
			return null;
		}
		ConstructorInfo constructor = type.GetConstructor(new Type[1] { typeof(FStructFallback) });
		object obj;
		if (constructor != null)
		{
			obj = constructor.Invoke(new object[1] { fallback });
		}
		else
		{
			obj = Activator.CreateInstance(type);
			ObjectMapper?.Map(fallback, obj);
		}
		return obj;
	}
}
