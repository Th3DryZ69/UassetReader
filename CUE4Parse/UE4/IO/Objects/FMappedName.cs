namespace CUE4Parse.UE4.IO.Objects;

public readonly struct FMappedName
{
	public enum EType
	{
		Package,
		Container,
		Global
	}

	private const int IndexBits = 30;

	private const uint IndexMask = 1073741823u;

	private const uint TypeMask = 3221225472u;

	private const int TypeShift = 30;

	public readonly uint _nameIndex;

	public readonly uint ExtraIndex;

	public uint NameIndex => _nameIndex & 0x3FFFFFFF;

	public EType Type => (EType)((_nameIndex & 0xC0000000u) >> 30);

	public bool IsGlobal => (_nameIndex & 0xC0000000u) >> 30 != 0;
}
