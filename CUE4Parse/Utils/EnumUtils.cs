using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CUE4Parse.Utils;

public static class EnumUtils
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string ToStringBitfield<T>(this T inEnum) where T : Enum
	{
		List<T> list = new List<T>();
		T[] array = (T[])Enum.GetValues(typeof(T));
		foreach (T val in array)
		{
			if ((Convert.ToUInt64(val) == 0L) ? (Convert.ToUInt64(inEnum) == 0) : inEnum.HasFlag(val))
			{
				list.Add(val);
			}
		}
		if (list.Count <= 0)
		{
			return "0";
		}
		return string.Join(" | ", list);
	}
}
