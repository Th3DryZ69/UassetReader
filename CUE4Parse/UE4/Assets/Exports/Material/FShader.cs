using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FShader
{
	public FShaderParameterBindings Bindings;

	public FShaderParameterMapInfo ParameterMapInfo;

	public FHashedName[] UniformBufferParameterStructs;

	public FShaderUniformBufferParameter[] UniformBufferParameters;

	public ulong Type;

	public ulong VFType;

	public FShaderTarget Target;

	public int ResourceIndex;

	public uint NumInstructions;

	public uint SortKey;

	public FShader(FMemoryImageArchive Ar)
	{
		Bindings = new FShaderParameterBindings(Ar);
		ParameterMapInfo = new FShaderParameterMapInfo(Ar);
		UniformBufferParameterStructs = Ar.ReadArray<FHashedName>();
		UniformBufferParameters = Ar.ReadArray<FShaderUniformBufferParameter>();
		Type = Ar.Read<ulong>();
		VFType = Ar.Read<ulong>();
		Target = Ar.Read<FShaderTarget>();
		ResourceIndex = Ar.Read<int>();
		NumInstructions = Ar.Read<uint>();
		SortKey = ((Ar.Game >= EGame.GAME_UE5_0) ? Ar.Read<uint>() : 0u);
	}
}
