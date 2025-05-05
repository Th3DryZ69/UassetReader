using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CUE4Parse.GameTypes.FN.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public abstract class FPropertyTagType<T> : FPropertyTagType
{
	public T Value { get; protected set; }

	public override object? GenericValue => Value;

	public override string ToString()
	{
		if (Value == null)
		{
			return string.Empty;
		}
		return Value.ToString() + " (" + GetType().Name + ")";
	}
}
[JsonConverter(typeof(FPropertyTagTypeConverter))]
public abstract class FPropertyTagType
{
	public abstract object? GenericValue { get; }

	public object? GetValue(Type type)
	{
		object genericValue = GenericValue;
		if (type.IsInstanceOfType(genericValue))
		{
			return genericValue;
		}
		if (!(this is FPropertyTagType<UScriptStruct> fPropertyTagType))
		{
			if (!(this is FPropertyTagType<UScriptArray> fPropertyTagType2))
			{
				if (!(this is FPropertyTagType<FPackageIndex> fPropertyTagType3))
				{
					if (!(this is FPropertyTagType<FSoftObjectPath> fPropertyTagType4))
					{
						if (this is EnumProperty enumProperty && type.IsEnum)
						{
							string text = enumProperty.Value.Text;
							string search = text.SubstringAfter("::");
							int num = Array.FindIndex(type.GetEnumNames(), (string it) => it == search);
							if (num != -1)
							{
								return type.GetEnumValues().GetValue(num);
							}
							return null;
						}
					}
					else if (typeof(UObject).IsAssignableFrom(type))
					{
						if (fPropertyTagType4.Value.TryLoad(out UObject export) && type.IsInstanceOfType(export))
						{
							return export;
						}
						return null;
					}
				}
				else
				{
					if (typeof(UObject).IsAssignableFrom(type))
					{
						if (fPropertyTagType3.Value.TryLoad(out UObject export2) && type.IsInstanceOfType(export2))
						{
							return export2;
						}
						return null;
					}
					FPropertyTagType<FPackageIndex> fPropertyTagType5 = fPropertyTagType3;
					if (typeof(ResolvedObject).IsAssignableFrom(type))
					{
						return fPropertyTagType5.Value.ResolvedObject;
					}
				}
			}
			else
			{
				if (type.IsArray)
				{
					List<FPropertyTagType> properties = fPropertyTagType2.Value.Properties;
					Type elementType = type.GetElementType();
					Array array = Array.CreateInstance(elementType, properties.Count);
					for (int num2 = 0; num2 < properties.Count; num2++)
					{
						array.SetValue(properties[num2].GetValue(elementType), num2);
					}
					return array;
				}
				FPropertyTagType<UScriptArray> fPropertyTagType6 = fPropertyTagType2;
				if (typeof(IList).IsAssignableFrom(type))
				{
					List<FPropertyTagType> properties2 = fPropertyTagType6.Value.Properties;
					Type type2 = type.GenericTypeArguments[0];
					IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type2), properties2.Count);
					{
						foreach (FPropertyTagType item in properties2)
						{
							list.Add(item.GetValue(type2));
						}
						return list;
					}
				}
			}
		}
		else
		{
			if (type.IsInstanceOfType(fPropertyTagType.Value.StructType))
			{
				return fPropertyTagType.Value.StructType;
			}
			if (fPropertyTagType.Value.StructType is FStructFallback fallback && type.GetCustomAttribute<StructFallback>() != null)
			{
				return fallback.MapToClass(type);
			}
		}
		return null;
	}

	public abstract override string ToString();

	internal static FPropertyTagType? ReadPropertyTagType(FAssetArchive Ar, string propertyType, FPropertyTagData? tagData, ReadType type)
	{
		return propertyType switch
		{
			"ArrayProperty" => new ArrayProperty(Ar, tagData, type), 
			"AssetObjectProperty" => new AssetObjectProperty(Ar, type), 
			"AssetClassProperty" => new AssetObjectProperty(Ar, type), 
			"BoolProperty" => new BoolProperty(Ar, tagData, type), 
			"ByteProperty" => (tagData != null && tagData.EnumName != null && !tagData.EnumName.Equals("None", StringComparison.OrdinalIgnoreCase)) ? ((FPropertyTagType)new EnumProperty(Ar, tagData, type)) : ((FPropertyTagType)new ByteProperty(Ar, type)), 
			"ClassProperty" => new ClassProperty(Ar, type), 
			"DelegateProperty" => new DelegateProperty(Ar, type), 
			"DoubleProperty" => new DoubleProperty(Ar, type), 
			"EnumProperty" => new EnumProperty(Ar, tagData, type), 
			"FieldPathProperty" => new FieldPathProperty(Ar, type), 
			"FloatProperty" => new FloatProperty(Ar, type), 
			"Int16Property" => new Int16Property(Ar, type), 
			"Int64Property" => new Int64Property(Ar, type), 
			"Int8Property" => new Int8Property(Ar, type), 
			"IntProperty" => new IntProperty(Ar, type), 
			"InterfaceProperty" => new InterfaceProperty(Ar, type), 
			"LazyObjectProperty" => new LazyObjectProperty(Ar, type), 
			"MapProperty" => new MapProperty(Ar, tagData, type), 
			"MulticastDelegateProperty" => new MulticastDelegateProperty(Ar, type), 
			"MulticastInlineDelegateProperty" => new MulticastInlineDelegateProperty(Ar, type), 
			"MulticastSparseDelegateProperty" => new MulticastSparseDelegateProperty(Ar, type), 
			"NameProperty" => new NameProperty(Ar, type), 
			"ObjectProperty" => (Ar is FLevelSaveRecordArchive) ? ((FPropertyTagType)new AssetObjectProperty(Ar, type)) : ((FPropertyTagType)new ObjectProperty(Ar, type)), 
			"SetProperty" => new SetProperty(Ar, tagData, type), 
			"SoftClassProperty" => new SoftObjectProperty(Ar, type), 
			"SoftObjectProperty" => new SoftObjectProperty(Ar, type), 
			"StrProperty" => new StrProperty(Ar, type), 
			"StructProperty" => new StructProperty(Ar, tagData, type), 
			"TextProperty" => new TextProperty(Ar, type), 
			"UInt16Property" => new UInt16Property(Ar, type), 
			"UInt32Property" => new UInt32Property(Ar, type), 
			"UInt64Property" => new UInt64Property(Ar, type), 
			"WeakObjectProperty" => new WeakObjectProperty(Ar, type), 
			_ => null, 
		};
	}
}
