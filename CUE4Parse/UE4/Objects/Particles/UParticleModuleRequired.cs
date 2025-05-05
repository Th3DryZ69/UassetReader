using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Particles;

public class UParticleModuleRequired : CUE4Parse.UE4.Assets.Exports.UObject
{
	public FVector2D[]? BoundingGeometry;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.MovedParticleCutoutsToRequiredModule && Ar.ReadBoolean())
		{
			BoundingGeometry = Ar.ReadArray<FVector2D>();
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		FVector2D[] boundingGeometry = BoundingGeometry;
		if (boundingGeometry != null && boundingGeometry.Length > 0)
		{
			writer.WritePropertyName("BoundingGeometry");
			serializer.Serialize(writer, BoundingGeometry);
		}
	}
}
