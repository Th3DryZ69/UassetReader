namespace CUE4Parse.UE4.IO.Objects;

public enum EIoContainerFlags : byte
{
	None = 0,
	Compressed = 1,
	Encrypted = 2,
	Signed = 4,
	Indexed = 8
}
