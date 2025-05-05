namespace CUE4Parse.MappingsProvider.Usmap;

public enum EUsmapCompressionMethod : byte
{
	None = 0,
	Oodle = 1,
	Brotli = 2,
	ZStandard = 3,
	Unknown = byte.MaxValue
}
