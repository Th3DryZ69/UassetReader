using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public abstract class EX_CastBase : KismetExpression
{
	public FPackageIndex ClassPtr;

	public KismetExpression Target;

	public EX_CastBase(FKismetArchive Ar)
	{
		ClassPtr = new FPackageIndex(Ar);
		Target = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("InterfaceClass");
		serializer.Serialize(writer, ClassPtr);
		writer.WritePropertyName("Target");
		serializer.Serialize(writer, Target);
	}
}
