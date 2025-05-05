using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Niagara;

public class FNiagaraDataInterfaceGPUParamInfo : IUStruct
{
	public string DataInterfaceHLSLSymbol;

	public string DIClassName;

	public FNiagaraDataInterfaceGeneratedFunction[] GeneratedFunctions;

	public FNiagaraDataInterfaceGPUParamInfo(FArchive Ar)
	{
		DataInterfaceHLSLSymbol = Ar.ReadFString();
		DIClassName = Ar.ReadFString();
		if (FNiagaraCustomVersion.Get(Ar) >= FNiagaraCustomVersion.Type.AddGeneratedFunctionsToGPUParamInfo)
		{
			GeneratedFunctions = Ar.ReadArray(() => new FNiagaraDataInterfaceGeneratedFunction(Ar));
		}
	}
}
