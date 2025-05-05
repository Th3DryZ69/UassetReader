using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Objects.UObject;

public class FScriptDelegate
{
	public FPackageIndex Object;

	public FName FunctionName;

	public FScriptDelegate(FAssetArchive Ar)
	{
		Object = new FPackageIndex(Ar);
		FunctionName = Ar.ReadFName();
	}

	public FScriptDelegate(FPackageIndex obj, FName functionName)
	{
		Object = obj;
		FunctionName = functionName;
	}
}
