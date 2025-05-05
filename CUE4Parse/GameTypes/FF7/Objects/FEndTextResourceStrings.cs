using System.Collections.Generic;
using CUE4Parse.UE4;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.GameTypes.FF7.Objects;

[JsonConverter(typeof(FEndTextResourceStringsConverter))]
public class FEndTextResourceStrings : IUStruct
{
	public readonly Dictionary<string, string>? Entries;

	public FEndTextResourceStrings(FArchive Ar)
	{
		string value = Ar.ReadFString();
		int num = Ar.Read<int>();
		if (num > 0)
		{
			Entries = new Dictionary<string, string>();
			if (!string.IsNullOrWhiteSpace(value))
			{
				Entries.Add("Str", value);
			}
			for (int i = 0; i < num; i++)
			{
				FName fName = Ar.ReadFName();
				string value2 = Ar.ReadFString();
				Entries.Add(fName.Text, value2);
			}
		}
	}
}
