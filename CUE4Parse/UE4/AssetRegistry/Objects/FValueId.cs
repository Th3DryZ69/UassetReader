namespace CUE4Parse.UE4.AssetRegistry.Objects;

public class FValueId
{
	private const int _TYPE_BITS = 3;

	private static readonly int _INDEX_BITS = 29;

	public readonly EValueType Type;

	public readonly int Index;

	public FValueId(FAssetRegistryReader Ar)
	{
		uint num = Ar.Read<uint>();
		Type = (EValueType)(num << _INDEX_BITS >> _INDEX_BITS);
		Index = (int)num >> 3;
	}
}
