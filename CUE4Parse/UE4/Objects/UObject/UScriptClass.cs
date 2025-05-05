using CUE4Parse.UE4.Assets;

namespace CUE4Parse.UE4.Objects.UObject;

[SkipObjectRegistration]
public class UScriptClass : UClass
{
	public UScriptClass(string className)
	{
		base.Name = className;
	}
}
