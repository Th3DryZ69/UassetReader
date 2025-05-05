using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.MappingsProvider;

public class PropertyType
{
	public string Type;

	public string? StructType;

	public PropertyType? InnerType;

	public PropertyType? ValueType;

	public string? EnumName;

	public bool? IsEnumAsByte;

	public bool? Bool;

	public UStruct? Struct;

	public UEnum? Enum;

	public PropertyType(string type, string? structType = null, PropertyType? innerType = null, PropertyType? valueType = null, string? enumName = null, bool? isEnumAsByte = null, bool? b = null)
	{
		Type = type;
		StructType = structType;
		InnerType = innerType;
		ValueType = valueType;
		EnumName = enumName;
		IsEnumAsByte = isEnumAsByte;
		Bool = b;
	}

	public PropertyType(FProperty prop)
	{
		string name = prop.GetType().Name;
		Type = name.Substring(1, name.Length - 1);
		if (!(prop is FArrayProperty fArrayProperty))
		{
			if (!(prop is FByteProperty fByteProperty))
			{
				if (!(prop is FEnumProperty fEnumProperty))
				{
					if (!(prop is FMapProperty fMapProperty))
					{
						if (!(prop is FSetProperty fSetProperty))
						{
							if (prop is FStructProperty fStructProperty)
							{
								ResolvedObject resolvedObject = fStructProperty.Struct.ResolvedObject;
								Struct = resolvedObject?.Object?.Value as UStruct;
								StructType = resolvedObject?.Name.Text;
							}
						}
						else
						{
							FProperty elementProp = fSetProperty.ElementProp;
							if (elementProp != null)
							{
								InnerType = new PropertyType(elementProp);
							}
						}
					}
					else
					{
						FProperty keyProp = fMapProperty.KeyProp;
						FProperty valueProp = fMapProperty.ValueProp;
						if (keyProp != null)
						{
							InnerType = new PropertyType(keyProp);
						}
						if (valueProp != null)
						{
							ValueType = new PropertyType(valueProp);
						}
					}
				}
				else
				{
					ApplyEnum(prop, fEnumProperty.Enum);
				}
			}
			else
			{
				ApplyEnum(prop, fByteProperty.Enum);
			}
		}
		else
		{
			FProperty inner = fArrayProperty.Inner;
			if (inner != null)
			{
				InnerType = new PropertyType(inner);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ApplyEnum(FProperty prop, FPackageIndex enumIndex)
	{
		ResolvedObject resolvedObject = enumIndex.ResolvedObject;
		Enum = resolvedObject?.Object?.Value as UEnum;
		EnumName = resolvedObject?.Name.Text;
		PropertyType innerType = ((prop.ElementSize != 4) ? null : new PropertyType("IntProperty"));
		InnerType = innerType;
	}
}
