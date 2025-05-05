using System;

namespace CUE4Parse.UE4.Wwise.Enums;

[Flags]
public enum ESoundConversion : byte
{
	PCM = 1,
	ADPCM = 2,
	Vorbis = 4
}
