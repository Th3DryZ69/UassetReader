namespace CUE4Parse.UE4.Objects.Core.Serialization;

public enum ECustomVersionSerializationFormat : byte
{
	Unknown = 0,
	Guids = 1,
	Enums = 2,
	Optimized = 3,
	CustomVersion_Automatic_Plus_One = 4,
	Latest = 3
}
