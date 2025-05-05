using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Material;

[SkipObjectRegistration]
public class UMaterialInterface : UUnrealMaterial
{
	public bool bUseMobileSpecular;

	public float MobileSpecularPower = 16f;

	public EMobileSpecularMask MobileSpecularMask;

	public UTexture? FlattenedTexture;

	public UTexture? MobileBaseTexture;

	public UTexture? MobileNormalTexture;

	public UTexture? MobileMaskTexture;

	public FStructFallback? CachedExpressionData;

	public FMaterialTextureInfo[] TextureStreamingData = Array.Empty<FMaterialTextureInfo>();

	public List<FMaterialResource> LoadedMaterialResources = new List<FMaterialResource>();

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		bUseMobileSpecular = GetOrDefault("bUseMobileSpecular", defaultValue: false);
		MobileSpecularPower = GetOrDefault("MobileSpecularPower", 0f);
		MobileSpecularMask = GetOrDefault("MobileSpecularMask", EMobileSpecularMask.MSM_Constant);
		FlattenedTexture = GetOrDefault<UTexture>("FlattenedTexture");
		MobileBaseTexture = GetOrDefault<UTexture>("MobileBaseTexture");
		MobileNormalTexture = GetOrDefault<UTexture>("MobileNormalTexture");
		MobileMaskTexture = GetOrDefault<UTexture>("MobileMaskTexture");
		TextureStreamingData = GetOrDefault("TextureStreamingData", Array.Empty<FMaterialTextureInfo>());
		if (FUE5ReleaseStreamObjectVersion.Get(Ar) >= FUE5ReleaseStreamObjectVersion.Type.MaterialInterfaceSavedCachedData && Ar.ReadBoolean())
		{
			CachedExpressionData = new FStructFallback(Ar, "MaterialCachedExpressionData");
		}
		if (Ar.Game == EGame.GAME_HogwartsLegacy)
		{
			Ar.Position += 20L;
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		if (LoadedMaterialResources.Count > 0)
		{
			writer.WritePropertyName("LoadedMaterialResources");
			serializer.Serialize(writer, LoadedMaterialResources);
		}
	}

	public override void GetParams(CMaterialParams parameters)
	{
		if (FlattenedTexture != null)
		{
			parameters.Diffuse = FlattenedTexture;
		}
		if (MobileBaseTexture != null)
		{
			parameters.Diffuse = MobileBaseTexture;
		}
		if (MobileNormalTexture != null)
		{
			parameters.Normal = MobileNormalTexture;
		}
		if (MobileMaskTexture != null)
		{
			parameters.Opacity = MobileMaskTexture;
		}
		parameters.UseMobileSpecular = bUseMobileSpecular;
		parameters.MobileSpecularPower = MobileSpecularPower;
		parameters.MobileSpecularMask = MobileSpecularMask;
	}

	public override void GetParams(CMaterialParams2 parameters, EMaterialFormat format)
	{
		for (int i = 0; i < TextureStreamingData.Length; i++)
		{
			string text = TextureStreamingData[i].TextureName.Text;
			if (parameters.TryGetTexture2d(out UTexture2D texture, text))
			{
				parameters.VerifyTexture(text, texture, appendToDictionary: false);
			}
		}
		if (CachedExpressionData == null || !CachedExpressionData.TryGetValue<FStructFallback>(out var obj, "Parameters") || !obj.TryGetAllValues<FStructFallback>(out FStructFallback[] obj2, "RuntimeEntries"))
		{
			return;
		}
		if (obj.TryGetValue<float[]>(out var obj3, "ScalarValues") && obj2[0].TryGetValue<FMaterialParameterInfo[]>(out var obj4, "ParameterInfos"))
		{
			for (int j = 0; j < obj4.Length; j++)
			{
				parameters.Scalars[obj4[j].Name.Text] = obj3[j];
			}
		}
		if (obj.TryGetValue<FLinearColor[]>(out var obj5, "VectorValues") && obj2[1].TryGetValue<FMaterialParameterInfo[]>(out var obj6, "ParameterInfos"))
		{
			for (int k = 0; k < obj6.Length; k++)
			{
				parameters.Colors[obj6[k].Name.Text] = obj5[k];
			}
		}
		if (!obj.TryGetValue<FPackageIndex[]>(out var obj7, "TextureValues") || !obj2[2].TryGetValue<FMaterialParameterInfo[]>(out var obj8, "ParameterInfos"))
		{
			return;
		}
		for (int l = 0; l < obj8.Length; l++)
		{
			string text2 = obj8[l].Name.Text;
			if (obj7[l].TryLoad(out UTexture export))
			{
				parameters.VerifyTexture(text2, export);
			}
		}
	}

	public void DeserializeInlineShaderMaps(FArchive Ar, ICollection<FMaterialResource> loadedResources)
	{
		int num = Ar.Read<int>();
		if (num > 0)
		{
			FMaterialResourceProxyReader ar = new FMaterialResourceProxyReader(Ar);
			for (int i = 0; i < num; i++)
			{
				FMaterialResource fMaterialResource = new FMaterialResource();
				fMaterialResource.DeserializeInlineShaderMap(ar);
				loadedResources.Add(fMaterialResource);
			}
		}
	}
}
