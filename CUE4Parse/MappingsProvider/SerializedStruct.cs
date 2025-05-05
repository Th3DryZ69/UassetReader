using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Objects.UObject;
using Serilog;

namespace CUE4Parse.MappingsProvider;

public class SerializedStruct : Struct
{
	public SerializedStruct(TypeMappings? context, UStruct struc)
		: base(context, struc.Name, struc.ChildProperties.Length)
	{
		SerializedStruct serializedStruct = this;
		Super = new Lazy<Struct>(delegate
		{
			UStruct uStruct = struc.SuperStruct.Load<UStruct>();
			if (uStruct != null)
			{
				if (uStruct is UScriptClass)
				{
					if (serializedStruct.Context != null && serializedStruct.Context.Types.TryGetValue(uStruct.Name, out Struct value2))
					{
						return value2;
					}
					Log.Warning("Missing prop mappings for type {0}", uStruct.Name);
					return (Struct?)null;
				}
				return new SerializedStruct(serializedStruct.Context, uStruct);
			}
			return (Struct?)null;
		});
		Properties = new Dictionary<int, PropertyInfo>();
		for (int num = 0; num < struc.ChildProperties.Length; num++)
		{
			FProperty fProperty = (FProperty)struc.ChildProperties[num];
			PropertyInfo value = new PropertyInfo(num, fProperty.Name.Text, new PropertyType(fProperty), fProperty.ArrayDim);
			for (int num2 = 0; num2 < fProperty.ArrayDim; num2++)
			{
				Properties[num + num2] = value;
			}
		}
	}
}
