using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Core.i18N;

public class FTextKey : IUStruct
{
	public readonly string Str;

	public readonly uint StrHash;

	public FTextKey(FArchive Ar, ELocResVersion versionNum)
	{
		StrHash = 0u;
		if ((int)versionNum >= 2)
		{
			StrHash = Ar.Read<uint>();
		}
		Str = Ar.ReadFString();
	}
}
