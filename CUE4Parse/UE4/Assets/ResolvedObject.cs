using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets;

[JsonConverter(typeof(ResolvedObjectConverter))]
public abstract class ResolvedObject : IObject
{
	public readonly IPackage Package;

	public int ExportIndex { get; }

	public abstract FName Name { get; }

	public virtual ResolvedObject? Outer => null;

	public virtual ResolvedObject? Class => null;

	public virtual ResolvedObject? Super => null;

	public virtual Lazy<UObject>? Object => null;

	public ResolvedObject(IPackage package, int exportIndex = -1)
	{
		Package = package;
		ExportIndex = exportIndex;
	}

	public string GetFullName(bool includeOuterMostName = true, bool includeClassPackage = false)
	{
		StringBuilder stringBuilder = new StringBuilder(128);
		GetFullName(includeOuterMostName, stringBuilder, includeClassPackage);
		return stringBuilder.ToString();
	}

	public void GetFullName(bool includeOuterMostName, StringBuilder resultString, bool includeClassPackage = false)
	{
		resultString.Append(includeClassPackage ? new FName?(Class?.GetPathName()) : Class?.Name);
		resultString.Append('\'');
		GetPathName(includeOuterMostName, resultString);
		resultString.Append('\'');
	}

	public string GetPathName(bool includeOuterMostName = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		GetPathName(includeOuterMostName, stringBuilder);
		return stringBuilder.ToString();
	}

	public void GetPathName(bool includeOuterMostName, StringBuilder resultString)
	{
		ResolvedObject outer = Outer;
		if (outer != null)
		{
			ResolvedObject outer2 = outer.Outer;
			if (outer2 != null || includeOuterMostName)
			{
				outer.GetPathName(includeOuterMostName, resultString);
				resultString.Append((outer2 != null && outer2.Outer == null) ? ':' : '.');
			}
		}
		resultString.Append(Name);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T? Load<T>() where T : UObject
	{
		return Object?.Value as T;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public UObject? Load()
	{
		return Object?.Value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryLoad(out UObject export)
	{
		try
		{
			export = Object?.Value;
			return export != null;
		}
		catch
		{
			export = null;
			return false;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<UObject?> LoadAsync()
	{
		return await Task.FromResult(Object?.Value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<UObject?> TryLoadAsync()
	{
		UObject result = default(UObject);
		int num;
		try
		{
			result = await Task.FromResult(Object?.Value);
			return result;
		}
		catch
		{
			num = 1;
		}
		if (num != 1)
		{
			return result;
		}
		return await Task.FromResult<UObject>(null);
	}

	public override string ToString()
	{
		return GetFullName();
	}
}
