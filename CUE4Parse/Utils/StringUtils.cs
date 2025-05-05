using System;
using System.Runtime.CompilerServices;
using CUE4Parse.Encryption.Aes;

namespace CUE4Parse.Utils;

public static class StringUtils
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FAesKey ParseAesKey(this string s)
	{
		return new FAesKey(s);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string SubstringBefore(this string s, char delimiter)
	{
		int num = s.IndexOf(delimiter);
		if (num != -1)
		{
			return s.Substring(0, num);
		}
		return s;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string SubstringBefore(this string s, string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
	{
		int num = s.IndexOf(delimiter, comparisonType);
		if (num != -1)
		{
			return s.Substring(0, num);
		}
		return s;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string SubstringAfter(this string s, char delimiter)
	{
		int num = s.IndexOf(delimiter);
		if (num != -1)
		{
			return s.Substring(num + 1, s.Length - num - 1);
		}
		return s;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string SubstringAfter(this string s, string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
	{
		int num = s.IndexOf(delimiter, comparisonType);
		if (num != -1)
		{
			return s.Substring(num + delimiter.Length, s.Length - num - delimiter.Length);
		}
		return s;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string SubstringBeforeLast(this string s, char delimiter)
	{
		int num = s.LastIndexOf(delimiter);
		if (num != -1)
		{
			return s.Substring(0, num);
		}
		return s;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string SubstringBeforeWithLast(this string s, char delimiter)
	{
		int num = s.LastIndexOf(delimiter);
		if (num != -1)
		{
			return s.Substring(0, num + 1);
		}
		return s;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string SubstringBeforeLast(this string s, string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
	{
		int num = s.LastIndexOf(delimiter, comparisonType);
		if (num != -1)
		{
			return s.Substring(0, num);
		}
		return s;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string SubstringAfterLast(this string s, char delimiter)
	{
		int num = s.LastIndexOf(delimiter);
		if (num != -1)
		{
			return s.Substring(num + 1, s.Length - num - 1);
		}
		return s;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string SubstringAfterWithLast(this string s, char delimiter)
	{
		int num = s.LastIndexOf(delimiter);
		if (num != -1)
		{
			return s.Substring(num, s.Length - num);
		}
		return s;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string SubstringAfterLast(this string s, string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
	{
		int num = s.LastIndexOf(delimiter, comparisonType);
		if (num != -1)
		{
			return s.Substring(num + delimiter.Length, s.Length - num - delimiter.Length);
		}
		return s;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Contains(this string orig, string value, StringComparison comparisonType)
	{
		return orig.IndexOf(value, comparisonType) >= 0;
	}
}
