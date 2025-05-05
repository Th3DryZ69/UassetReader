using CUE4Parse.UE4.Assets.Exports.Material.Parameters;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class UMaterialInstance : UMaterialInterface
{
	private ResolvedObject? _parent;

	public bool bHasStaticPermutationResource;

	public FMaterialInstanceBasePropertyOverrides? BasePropertyOverrides;

	public FStaticParameterSet? StaticParameters;

	public FStructFallback? CachedData;

	public UUnrealMaterial? Parent => _parent?.Load<UUnrealMaterial>();

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		_parent = GetOrDefault<ResolvedObject>("Parent");
		bHasStaticPermutationResource = GetOrDefault("bHasStaticPermutationResource", defaultValue: false);
		BasePropertyOverrides = GetOrDefault<FMaterialInstanceBasePropertyOverrides>("BasePropertyOverrides");
		StaticParameters = GetOrDefault("StaticParameters", GetOrDefault<FStaticParameterSet>("StaticParametersRuntime"));
		if (FUE5MainStreamObjectVersion.Get(Ar) >= FUE5MainStreamObjectVersion.Type.MaterialSavedCachedData && Ar.ReadBoolean())
		{
			CachedData = new FStructFallback(Ar, "MaterialInstanceCachedData");
		}
		if (bHasStaticPermutationResource && Ar.Ver >= EUnrealEngineObjectUE4Version.PURGED_FMATERIAL_COMPILE_OUTPUTS)
		{
			if (FRenderingObjectVersion.Get(Ar) < FRenderingObjectVersion.Type.MaterialAttributeLayerParameters)
			{
				StaticParameters = new FStaticParameterSet(Ar);
			}
			Ar.Position = validPos;
		}
		Ar.Position = validPos;
	}

	public override void GetParams(CMaterialParams2 parameters, EMaterialFormat format)
	{
		base.GetParams(parameters, format);
		if (StaticParameters != null)
		{
			FStaticSwitchParameter[] staticSwitchParameters = StaticParameters.StaticSwitchParameters;
			foreach (FStaticSwitchParameter fStaticSwitchParameter in staticSwitchParameters)
			{
				parameters.Switches[fStaticSwitchParameter.Name] = fStaticSwitchParameter.Value;
			}
		}
		if (BasePropertyOverrides != null)
		{
			parameters.BlendMode = BasePropertyOverrides.BlendMode;
			parameters.ShadingModel = BasePropertyOverrides.ShadingModel;
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		if (CachedData != null)
		{
			writer.WritePropertyName("CachedData");
			serializer.Serialize(writer, CachedData);
		}
	}
}
