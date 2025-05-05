using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine.GameFramework;

[JsonConverter(typeof(FUniqueNetIdReplConverter))]
public class FUniqueNetIdRepl : IUStruct
{
	public readonly FUniqueNetId? UniqueNetId;

	public FUniqueNetIdRepl(FArchive Ar)
	{
		if (Ar.Read<int>() > 0)
		{
			FName fName = Ar.ReadFName();
			string contents = Ar.ReadString();
			UniqueNetId = new FUniqueNetId(fName.Text, contents);
		}
		else
		{
			UniqueNetId = null;
		}
	}
}
