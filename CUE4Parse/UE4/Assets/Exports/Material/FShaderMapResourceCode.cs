using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FShaderMapResourceCode
{
	public FSHAHash ResourceHash;

	public FSHAHash[] ShaderHashes;

	public FShaderEntry[] ShaderEntries;

	public FShaderMapResourceCode(FArchive Ar)
	{
		ResourceHash = new FSHAHash(Ar);
		ShaderHashes = Ar.ReadArray(() => new FSHAHash(Ar));
		ShaderEntries = Ar.ReadArray(() => new FShaderEntry(Ar));
	}
}
