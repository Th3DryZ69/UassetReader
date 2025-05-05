using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Niagara;

public class UNiagaraSystem : CUE4Parse.UE4.Assets.Exports.UObject
{
	public List<FStructFallback> NiagaraEmitterCompiledDataStructs;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		if (FNiagaraCustomVersion.Get(Ar) >= FNiagaraCustomVersion.Type.ChangeEmitterCompiledDataToSharedRefs)
		{
			int num = Ar.Read<int>();
			NiagaraEmitterCompiledDataStructs = new List<FStructFallback>();
			for (int i = 0; i < num; i++)
			{
				NiagaraEmitterCompiledDataStructs.Add(new FStructFallback(Ar, "NiagaraEmitterCompiledData"));
			}
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		List<FStructFallback> niagaraEmitterCompiledDataStructs = NiagaraEmitterCompiledDataStructs;
		if (niagaraEmitterCompiledDataStructs != null && niagaraEmitterCompiledDataStructs.Count > 0)
		{
			writer.WritePropertyName("EmitterCompiledStructs");
			serializer.Serialize(writer, NiagaraEmitterCompiledDataStructs);
		}
	}
}
