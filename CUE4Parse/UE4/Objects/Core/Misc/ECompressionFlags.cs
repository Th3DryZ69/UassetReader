using System;

namespace CUE4Parse.UE4.Objects.Core.Misc;

[Flags]
public enum ECompressionFlags
{
	COMPRESS_None = 0,
	COMPRESS_ZLIB = 1,
	COMPRESS_GZIP = 2,
	COMPRESS_Custom = 4,
	COMPRESS_DeprecatedFormatFlagsMask = 0xF,
	COMPRESS_NoFlags = 0,
	COMPRESS_BiasMemory = 0x10,
	COMPRESS_BiasSize = 0x10,
	COMPRESS_BiasSpeed = 0x20,
	COMPRESS_SourceIsPadded = 0x80,
	COMPRESS_OptionsFlagsMask = 0xF0,
	COMPRESS_ForPackaging = 0x100,
	COMPRESS_ForPurposeMask = 0x100
}
