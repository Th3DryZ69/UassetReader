using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Niagara;

public class FNiagaraDataInterfaceGeneratedFunction
{
	public FName DefinitionName;

	public string InstanceName;

	public (FName, FName)[] Specifiers;

	public FNiagaraDataInterfaceGeneratedFunction(FArchive Ar)
	{
		DefinitionName = Ar.ReadFName();
		InstanceName = Ar.ReadFString();
		Specifiers = Ar.ReadArray<(FName, FName)>();
	}
}
