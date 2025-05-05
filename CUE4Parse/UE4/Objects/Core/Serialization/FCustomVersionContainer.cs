using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Core.Serialization;

[JsonConverter(typeof(FCustomVersionContainerConverter))]
public class FCustomVersionContainer
{
	public readonly FCustomVersion[] Versions;

	public FCustomVersionContainer()
	{
		Versions = Array.Empty<FCustomVersion>();
	}

	public FCustomVersionContainer(IEnumerable<FCustomVersion>? versions)
	{
		Versions = (versions ?? Array.Empty<FCustomVersion>()).ToArray();
	}

	public FCustomVersionContainer(FArchive Ar, ECustomVersionSerializationFormat format = ECustomVersionSerializationFormat.Optimized)
		: this()
	{
		switch (format)
		{
		case ECustomVersionSerializationFormat.Enums:
		{
			FEnumCustomVersion_DEPRECATED[] array2 = Ar.ReadArray<FEnumCustomVersion_DEPRECATED>();
			Versions = new FCustomVersion[array2.Length];
			for (int num2 = 0; num2 < Versions.Length; num2++)
			{
				Versions[num2] = array2[num2].ToCustomVersion();
			}
			break;
		}
		case ECustomVersionSerializationFormat.Guids:
		{
			FGuidCustomVersion_DEPRECATED[] array = Ar.ReadArray(() => new FGuidCustomVersion_DEPRECATED(Ar));
			Versions = new FCustomVersion[array.Length];
			for (int num = 0; num < Versions.Length; num++)
			{
				Versions[num] = array[num].ToCustomVersion();
			}
			break;
		}
		case ECustomVersionSerializationFormat.Optimized:
			Versions = Ar.ReadArray<FCustomVersion>();
			break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetVersion(FGuid customKey)
	{
		for (int i = 0; i < Versions.Length; i++)
		{
			if (Versions[i].Key == customKey)
			{
				return Versions[i].Version;
			}
		}
		return -1;
	}
}
