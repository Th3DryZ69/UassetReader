namespace CUE4Parse.UE4.IO.Objects;

public readonly struct FScriptObjectEntry
{
	public readonly FMappedName ObjectName;

	public readonly FPackageObjectIndex GlobalIndex;

	public readonly FPackageObjectIndex OuterIndex;

	public readonly FPackageObjectIndex CDOClassIndex;
}
