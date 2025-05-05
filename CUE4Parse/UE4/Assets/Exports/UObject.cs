using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Objects.Unversioned;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Assets.Exports;

[JsonConverter(typeof(UObjectConverter))]
[SkipObjectRegistration]
public class UObject : IPropertyHolder
{
	public UObject? Outer;

	public UStruct? Class;

	public ResolvedObject? Super;

	public ResolvedObject? Template;

	public EObjectFlags Flags;

	public string Name { get; set; }

	public List<FPropertyTag> Properties { get; private set; }

	public FGuid? ObjectGuid { get; private set; }

	public IPackage? Owner
	{
		get
		{
			UObject uObject = this;
			while (true)
			{
				UObject outer = uObject.Outer;
				if (outer == null)
				{
					break;
				}
				uObject = outer;
			}
			return uObject as IPackage;
		}
	}

	public virtual string ExportType => Class?.Name ?? GetType().Name;

	public UObject()
	{
		Properties = new List<FPropertyTag>();
	}

	public UObject(List<FPropertyTag> properties)
	{
		Properties = properties;
	}

	public virtual void Deserialize(FAssetArchive Ar, long validPos)
	{
		if (Ar.HasUnversionedProperties)
		{
			if (Class == null)
			{
				throw new ParserException(Ar, "Found unversioned properties but object does not have a class");
			}
			List<FPropertyTag> properties = (Properties = new List<FPropertyTag>());
			DeserializePropertiesUnversioned(properties, Ar, Class);
		}
		else
		{
			List<FPropertyTag> properties = (Properties = new List<FPropertyTag>());
			DeserializePropertiesTagged(properties, Ar);
		}
		if (!Flags.HasFlag(EObjectFlags.RF_ClassDefaultObject) && Ar.ReadBoolean() && Ar.Position + 16 <= validPos)
		{
			ObjectGuid = Ar.Read<FGuid>();
		}
		if (Ar.Game >= EGame.GAME_UE5_0 && Flags.HasFlag(EObjectFlags.RF_ClassDefaultObject))
		{
			Ar.Position += 4L;
		}
	}

	public string GetFullName(UObject? stopOuter = null, bool includeClassPackage = false)
	{
		StringBuilder stringBuilder = new StringBuilder(128);
		GetFullName(stopOuter, stringBuilder, includeClassPackage);
		return stringBuilder.ToString();
	}

	public void GetFullName(UObject? stopOuter, StringBuilder resultString, bool includeClassPackage = false)
	{
		resultString.Append((!includeClassPackage) ? ExportType : Class?.GetPathName());
		resultString.Append('\'');
		GetPathName(stopOuter, resultString);
		resultString.Append('\'');
	}

	public string GetPathName(UObject? stopOuter = null)
	{
		StringBuilder stringBuilder = new StringBuilder();
		GetPathName(stopOuter, stringBuilder);
		return stringBuilder.ToString();
	}

	public void GetPathName(UObject? stopOuter, StringBuilder resultString)
	{
		if (this != stopOuter)
		{
			UObject outer = Outer;
			if (outer != null && outer != stopOuter)
			{
				outer.GetPathName(stopOuter, resultString);
				resultString.Append((outer.Outer is IPackage) ? ':' : '.');
			}
			resultString.Append(Name);
		}
		else
		{
			resultString.Append("None");
		}
	}

	public UObject? GetTypedOuter(Type target)
	{
		UObject uObject = null;
		UObject outer = Outer;
		while (uObject == null && outer != null)
		{
			if (target.IsInstanceOfType(outer))
			{
				uObject = outer;
			}
			outer = outer.Outer;
		}
		return uObject;
	}

	public T? GetTypedOuter<T>() where T : UObject
	{
		return GetTypedOuter(typeof(T)) as T;
	}

	public virtual void PostLoad()
	{
	}

	internal static void DeserializePropertiesUnversioned(List<FPropertyTag> properties, FAssetArchive Ar, UStruct struc)
	{
		FUnversionedHeader fUnversionedHeader = new FUnversionedHeader(Ar);
		if (!fUnversionedHeader.HasValues)
		{
			return;
		}
		string name = struc.Name;
		Struct value = null;
		if (struc is UScriptClass)
		{
			Ar.Owner.Mappings?.Types.TryGetValue(name, out value);
		}
		else
		{
			value = new SerializedStruct(Ar.Owner.Mappings, struc);
		}
		if (value == null)
		{
			throw new ParserException(Ar, "Missing prop mappings for type " + name);
		}
		using FIterator fIterator = new FIterator(fUnversionedHeader);
		do
		{
			(int Val, bool IsNonZero) current = fIterator.Current;
			var (num, _) = current;
			PropertyInfo info2;
			if (current.IsNonZero)
			{
				if (!value.TryGetValue(num, out PropertyInfo info))
				{
					throw new ParserException(Ar, $"{name}: Unknown property with value {num}. Can't proceed with serialization (Serialized {properties.Count} properties until now)");
				}
				FPropertyTag fPropertyTag = new FPropertyTag(Ar, info, ReadType.NORMAL);
				if (fPropertyTag.Tag == null)
				{
					throw new ParserException(Ar, $"{name}: Failed to serialize property {info.MappingType.Type} {info.Name}. Can't proceed with serialization (Serialized {properties.Count} properties until now)");
				}
				properties.Add(fPropertyTag);
			}
			else if (value.TryGetValue(num, out info2))
			{
				properties.Add(new FPropertyTag(Ar, info2, ReadType.ZERO));
			}
			else
			{
				Log.Warning("{0}: Unknown property with value {1} but it's zero so we are good", name, num);
			}
		}
		while (fIterator.MoveNext());
	}

	internal static void DeserializePropertiesTagged(List<FPropertyTag> properties, FAssetArchive Ar)
	{
		while (true)
		{
			FPropertyTag fPropertyTag = new FPropertyTag(Ar, readData: true);
			if (!fPropertyTag.Name.IsNone)
			{
				properties.Add(fPropertyTag);
				continue;
			}
			break;
		}
	}

	protected internal virtual void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		IPackage owner = Owner;
		writer.WritePropertyName("Type");
		writer.WriteValue(ExportType);
		writer.WritePropertyName("Name");
		writer.WriteValue(Name);
		if (Outer != null && Outer != owner)
		{
			writer.WritePropertyName("Outer");
			writer.WriteValue(Outer.Name);
		}
		if (Super != null)
		{
			writer.WritePropertyName("Super");
			writer.WriteValue(Super.Name.Text);
		}
		if (Template != null)
		{
			writer.WritePropertyName("Template");
			writer.WriteValue(Template.Name.Text);
		}
		if (Class != null)
		{
			writer.WritePropertyName("Class");
			serializer.Serialize(writer, Class.GetFullName());
		}
		if (Properties.Count <= 0)
		{
			return;
		}
		writer.WritePropertyName("Properties");
		writer.WriteStartObject();
		foreach (FPropertyTag property in Properties)
		{
			writer.WritePropertyName(property.Name.Text);
			serializer.Serialize(writer, property.Tag);
		}
		writer.WriteEndObject();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetOrDefault<T>(string name, T defaultValue = default(T), StringComparison comparisonType = StringComparison.Ordinal)
	{
		return PropertyUtil.GetOrDefault(this, name, defaultValue, comparisonType);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Lazy<T> GetOrDefaultLazy<T>(string name, T defaultValue = default(T), StringComparison comparisonType = StringComparison.Ordinal)
	{
		return PropertyUtil.GetOrDefaultLazy(this, name, defaultValue, comparisonType);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T Get<T>(string name, StringComparison comparisonType = StringComparison.Ordinal)
	{
		return PropertyUtil.Get<T>(this, name, comparisonType);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Lazy<T> GetLazy<T>(string name, StringComparison comparisonType = StringComparison.Ordinal)
	{
		return PropertyUtil.GetLazy<T>(this, name, comparisonType);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetByIndex<T>(int index)
	{
		return PropertyUtil.GetByIndex<T>(this, index);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryGetValue<T>(out T obj, params string[] names)
	{
		foreach (string name in names)
		{
			T val = default(T);
			T orDefault = GetOrDefault(name, val, StringComparison.OrdinalIgnoreCase);
			if (orDefault != null)
			{
				ref T reference = ref orDefault;
				val = default(T);
				if (val == null)
				{
					val = reference;
					reference = ref val;
				}
				object obj2 = default(T);
				if (!reference.Equals(obj2))
				{
					obj = orDefault;
					return true;
				}
			}
		}
		obj = default(T);
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

	public virtual void GetLifetimeReplicatedProps(List<FLifetimeProperty> outLifetimeProps)
	{
	}

	public virtual void PreNetReceive()
	{
	}

	public virtual void PostNetReceive()
	{
	}

	public virtual void PostRepNotifies()
	{
	}

	public virtual void PreDestroyFromReplication()
	{
	}

	public virtual bool IsNameStableForNetworking()
	{
		if (!Flags.HasFlag(EObjectFlags.RF_WasLoaded))
		{
			return Flags.HasFlag(EObjectFlags.RF_DefaultSubObject);
		}
		return true;
	}

	public virtual bool IsFullNameStableForNetworking()
	{
		if (Outer != null && !Outer.IsNameStableForNetworking())
		{
			return false;
		}
		return IsNameStableForNetworking();
	}

	public virtual bool IsSupportedForNetworking()
	{
		return IsFullNameStableForNetworking();
	}

	public override string ToString()
	{
		return Name;
	}
}
