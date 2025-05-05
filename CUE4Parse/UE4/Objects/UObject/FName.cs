using System;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.IO.Objects;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

[JsonConverter(typeof(FNameConverter))]
public readonly struct FName : IComparable<FName>
{
	private readonly FNameEntrySerialized _name;

	public readonly int Index;

	public readonly int Number;

	public readonly FNameComparisonMethod ComparisonMethod;

	public string Text
	{
		get
		{
			if (Number != 0)
			{
				return $"{PlainText}_{Number - 1}";
			}
			return PlainText;
		}
	}

	public string PlainText
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _name.Name ?? "None";
		}
	}

	public bool IsNone => Text == "None";

	public FName(string name, int index = 0, int number = 0, FNameComparisonMethod compare = FNameComparisonMethod.Text)
	{
		_name = new FNameEntrySerialized(name, 0uL);
		Index = index;
		Number = number;
		ComparisonMethod = compare;
	}

	public FName(FNameEntrySerialized name, int index, int number, FNameComparisonMethod compare = FNameComparisonMethod.Index)
	{
		_name = name;
		Index = index;
		Number = number;
		ComparisonMethod = compare;
	}

	public FName(FNameEntrySerialized[] nameMap, int index, int number, FNameComparisonMethod compare = FNameComparisonMethod.Index)
		: this(nameMap[index], index, number, compare)
	{
	}

	public FName(FMappedName mappedName, FNameEntrySerialized[] nameMap, FNameComparisonMethod compare = FNameComparisonMethod.Index)
		: this(nameMap, (int)mappedName.NameIndex, (int)mappedName.ExtraIndex, compare)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator FName(string s)
	{
		return new FName(s);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(FName a, FName b)
	{
		return a.ComparisonMethod switch
		{
			FNameComparisonMethod.Index => a.Index == b.Index && a.Number == b.Number, 
			FNameComparisonMethod.Text => string.Equals(a.Text, b.Text, StringComparison.OrdinalIgnoreCase), 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(FName a, FName b)
	{
		return !(a == b);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(FName a, int b)
	{
		return a.Index == b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(FName a, int b)
	{
		return a.Index != b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(FName a, uint b)
	{
		return a.Index == b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(FName a, uint b)
	{
		return a.Index != b;
	}

	public override bool Equals(object? obj)
	{
		if (obj is FName fName)
		{
			return this == fName;
		}
		return false;
	}

	public override int GetHashCode()
	{
		if (ComparisonMethod != FNameComparisonMethod.Text)
		{
			return HashCode.Combine(Index, Number);
		}
		return StringComparer.OrdinalIgnoreCase.GetHashCode(Text.GetHashCode());
	}

	public int CompareTo(FName other)
	{
		return string.Compare(Text, other.Text, StringComparison.OrdinalIgnoreCase);
	}

	public override string ToString()
	{
		return Text;
	}
}
