using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.i18N;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Localization;

[JsonConverter(typeof(FTextLocalizationResourceConverter))]
public class FTextLocalizationResource
{
	private readonly FGuid _locResMagic = new FGuid(1970541582u, 4228074087u, 2643465546u, 461322179u);

	public readonly Dictionary<FTextKey, Dictionary<FTextKey, FEntry>> Entries = new Dictionary<FTextKey, Dictionary<FTextKey, FEntry>>();

	public FTextLocalizationResource(FArchive Ar)
	{
		FGuid fGuid = Ar.Read<FGuid>();
		ELocResVersion eLocResVersion = ELocResVersion.Legacy;
		if (fGuid == _locResMagic)
		{
			eLocResVersion = Ar.Read<ELocResVersion>();
		}
		else
		{
			Ar.Position = 0L;
			Log.Warning("LocRes '" + Ar.Name + "' failed the magic number check! Assuming this is a legacy resource");
		}
		if ((int)eLocResVersion > 3)
		{
			throw new ParserException(Ar, $"LocRes '{Ar.Name}' is too new to be loaded (File Version: {eLocResVersion:D}, Loader Version: {3:D})");
		}
		FTextLocalizationResourceString[] array = Array.Empty<FTextLocalizationResourceString>();
		if ((int)eLocResVersion >= 1)
		{
			long num = Ar.Read<long>();
			if (num != -1)
			{
				long position = Ar.Position;
				Ar.Position = num;
				if ((int)eLocResVersion >= 2)
				{
					array = Ar.ReadArray(() => new FTextLocalizationResourceString(Ar));
				}
				else
				{
					string[] array2 = Ar.ReadArray(Ar.ReadFString);
					array = new FTextLocalizationResourceString[array2.Length];
					for (int num2 = 0; num2 < array.Length; num2++)
					{
						array[num2] = new FTextLocalizationResourceString(array2[num2], -1);
					}
				}
				Ar.Position = position;
			}
		}
		if ((int)eLocResVersion >= 2)
		{
			Ar.Position += 4L;
		}
		uint num3 = Ar.Read<uint>();
		for (int num4 = 0; num4 < num3; num4++)
		{
			FTextKey fTextKey = new FTextKey(Ar, eLocResVersion);
			uint num5 = Ar.Read<uint>();
			Dictionary<FTextKey, FEntry> dictionary = new Dictionary<FTextKey, FEntry>((int)num5);
			for (int num6 = 0; num6 < num5; num6++)
			{
				FTextKey fTextKey2 = new FTextKey(Ar, eLocResVersion);
				FEntry fEntry = new FEntry(Ar)
				{
					SourceStringHash = Ar.Read<uint>()
				};
				if ((int)eLocResVersion >= 1)
				{
					int num7 = Ar.Read<int>();
					if (array.Length > num7)
					{
						FTextLocalizationResourceString fTextLocalizationResourceString = array[num7];
						if (fTextLocalizationResourceString.RefCount == 1)
						{
							fEntry.LocalizedString = fTextLocalizationResourceString.String;
							fTextLocalizationResourceString.RefCount--;
						}
						else
						{
							fEntry.LocalizedString = fTextLocalizationResourceString.String;
							if (fTextLocalizationResourceString.RefCount != -1)
							{
								fTextLocalizationResourceString.RefCount--;
							}
						}
					}
					else
					{
						Log.Warning($"LocRes '{fEntry.LocResName}' has an invalid localized string index for namespace '{fTextKey.Str}' and key '{fTextKey2.Str}'. This entry will have no translation.");
					}
				}
				else
				{
					fEntry.LocalizedString = Ar.ReadFString();
				}
				dictionary.Add(fTextKey2, fEntry);
			}
			Entries.Add(fTextKey, dictionary);
		}
	}
}
