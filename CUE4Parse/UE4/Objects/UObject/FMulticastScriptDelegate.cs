using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Objects.UObject;

public class FMulticastScriptDelegate
{
	public FScriptDelegate[] InvocationList;

	public FMulticastScriptDelegate(FAssetArchive Ar)
	{
		InvocationList = Ar.ReadArray(() => new FScriptDelegate(Ar));
	}

	public FMulticastScriptDelegate(FScriptDelegate[] invocationList)
	{
		InvocationList = invocationList;
	}
}
