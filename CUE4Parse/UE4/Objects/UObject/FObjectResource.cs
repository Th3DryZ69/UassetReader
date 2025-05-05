using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

[JsonConverter(typeof(FObjectResourceConverter))]
public abstract class FObjectResource : IObject
{
	public FName ObjectName;

	public FPackageIndex OuterIndex;

	public override string ToString()
	{
		return ObjectName.Text;
	}
}
