using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class UMaterialInstanceConstant : UMaterialInstance
{
	public FScalarParameterValue[] ScalarParameterValues = Array.Empty<FScalarParameterValue>();

	public FTextureParameterValue[] TextureParameterValues = Array.Empty<FTextureParameterValue>();

	public FVectorParameterValue[] VectorParameterValues = Array.Empty<FVectorParameterValue>();

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		ScalarParameterValues = GetOrDefault("ScalarParameterValues", Array.Empty<FScalarParameterValue>());
		TextureParameterValues = GetOrDefault("TextureParameterValues", Array.Empty<FTextureParameterValue>());
		VectorParameterValues = GetOrDefault("VectorParameterValues", Array.Empty<FVectorParameterValue>());
	}

	public override void GetParams(CMaterialParams parameters)
	{
		if (base.Parent != null && base.Parent != this)
		{
			base.Parent.GetParams(parameters);
		}
		base.GetParams(parameters);
		int diffWeight = 0;
		int normWeight = 0;
		int specWeight = 0;
		int specPowWeight = 0;
		int opWeight = 0;
		int emWeight = 0;
		int dcWeight = 0;
		int emcWeight = 0;
		int cubeWeight = 0;
		int maskWeight = 0;
		int miscWeight = 0;
		int metalWeight = 0;
		int roughWeight = 0;
		int specuWeight = 0;
		if (TextureParameterValues.Length != 0)
		{
			parameters.Opacity = null;
		}
		FTextureParameterValue[] textureParameterValues = TextureParameterValues;
		foreach (FTextureParameterValue obj in textureParameterValues)
		{
			string name = obj.Name;
			UTexture uTexture = obj.ParameterValue.Load<UTexture>();
			if (uTexture != null && !name.Contains("detail", StringComparison.CurrentCultureIgnoreCase) && !name.Contains("ws ", StringComparison.CurrentCultureIgnoreCase) && !name.Contains("_2", StringComparison.CurrentCultureIgnoreCase))
			{
				Diffuse(name.Contains("dif", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				Diffuse(name.Contains("albedo", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				Diffuse(name.Contains("color", StringComparison.CurrentCultureIgnoreCase), 80, uTexture);
				Diffuse(name.Equals("co", StringComparison.CurrentCultureIgnoreCase), 70, uTexture);
				Diffuse(name.StartsWith("co_", StringComparison.CurrentCultureIgnoreCase), 70, uTexture);
				Normal(name.Contains("norm", StringComparison.CurrentCultureIgnoreCase) && !name.Contains("fx", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				Normal(name.Equals("nm", StringComparison.CurrentCultureIgnoreCase), 70, uTexture);
				Normal(name.StartsWith("nm_", StringComparison.CurrentCultureIgnoreCase), 70, uTexture);
				SpecPower(name.Contains("specpow", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				Specular(name.Contains("spec", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				Specular(name.Contains("packed", StringComparison.CurrentCultureIgnoreCase), 80, uTexture);
				Specular(name.Contains("mrae", StringComparison.CurrentCultureIgnoreCase), 80, uTexture);
				Specular(name.Contains("mrs", StringComparison.CurrentCultureIgnoreCase), 80, uTexture);
				Specular(name.Equals("lp", StringComparison.CurrentCultureIgnoreCase), 70, uTexture);
				Specular(name.StartsWith("lp_", StringComparison.CurrentCultureIgnoreCase), 70, uTexture);
				Emissive(name.Contains("emiss", StringComparison.CurrentCultureIgnoreCase) && !name.Contains("gradient", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				BakedMask(name.Contains("fx", StringComparison.CurrentCultureIgnoreCase) && name.Contains("mask", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				CubeMap(name.Contains("cube", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				CubeMap(name.Contains("refl", StringComparison.CurrentCultureIgnoreCase), 90, uTexture);
				Opacity(name.Contains("opac", StringComparison.CurrentCultureIgnoreCase), 90, uTexture);
				Opacity(name.Contains("trans", StringComparison.CurrentCultureIgnoreCase) && !name.Contains("transm", StringComparison.CurrentCultureIgnoreCase), 80, uTexture);
				Opacity(name.Contains("opacity", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				Opacity(name.Contains("alpha", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				Misc(name.Equals("m", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
			}
		}
		FVectorParameterValue[] vectorParameterValues = VectorParameterValues;
		foreach (FVectorParameterValue obj2 in vectorParameterValues)
		{
			string name2 = obj2.Name;
			FLinearColor? parameterValue = obj2.ParameterValue;
			if (parameterValue.HasValue)
			{
				DiffuseColor(name2.Contains("color", StringComparison.CurrentCultureIgnoreCase), 100, parameterValue.Value);
				DiffuseColor(name2.Equals("co", StringComparison.CurrentCultureIgnoreCase), 80, parameterValue.Value);
				EmissiveColor(name2.Contains("emis", StringComparison.CurrentCultureIgnoreCase) && name2.Contains("color", StringComparison.CurrentCultureIgnoreCase), 100, parameterValue.Value);
				EmissiveColor(name2.Contains("emissive", StringComparison.CurrentCultureIgnoreCase), 80, parameterValue.Value);
			}
		}
		FScalarParameterValue[] scalarParameterValues = ScalarParameterValues;
		foreach (FScalarParameterValue obj3 in scalarParameterValues)
		{
			string name3 = obj3.Name;
			float parameterValue2 = obj3.ParameterValue;
			MetallicValue(name3.Contains("metallic", StringComparison.CurrentCultureIgnoreCase) && !name3.Contains("overwrite", StringComparison.CurrentCultureIgnoreCase), 100, parameterValue2);
			MetallicValue(name3.Contains("metal", StringComparison.CurrentCultureIgnoreCase), 80, parameterValue2);
			RoughnessValue(name3.Contains("roughness", StringComparison.CurrentCultureIgnoreCase) && !name3.Contains("min", StringComparison.CurrentCultureIgnoreCase), 100, parameterValue2);
			SpecularValue(name3.Contains("specular", StringComparison.CurrentCultureIgnoreCase), 100, parameterValue2);
		}
		if (BasePropertyOverrides != null)
		{
			parameters.IsTransparent = BasePropertyOverrides.BlendMode == EBlendMode.BLEND_Translucent;
		}
		if (parameters.Diffuse == null && TextureParameterValues.Length == 1)
		{
			parameters.Diffuse = TextureParameterValues[0].ParameterValue.Load<UTexture>();
		}
		void BakedMask(bool check, int weight, UTexture tex)
		{
			if (check && weight > maskWeight)
			{
				parameters.Mask = tex;
				maskWeight = weight;
			}
		}
		void CubeMap(bool check, int weight, UTexture tex)
		{
			if (check && weight > cubeWeight)
			{
				parameters.Cube = tex;
				cubeWeight = weight;
			}
		}
		void Diffuse(bool check, int weight, UTexture tex)
		{
			if (check && weight >= diffWeight)
			{
				parameters.HasTopDiffuseTexture = true;
				parameters.Diffuse = tex;
				diffWeight = weight;
			}
		}
		void DiffuseColor(bool check, int weight, FLinearColor color)
		{
			if (check && weight > dcWeight)
			{
				parameters.DiffuseColor = color.ToSRGB();
				dcWeight = weight;
			}
		}
		void Emissive(bool check, int weight, UTexture tex)
		{
			if (check && weight > emWeight)
			{
				parameters.HasTopEmissiveTexture = true;
				parameters.Emissive = tex;
				emWeight = weight;
			}
		}
		void EmissiveColor(bool check, int weight, FLinearColor color)
		{
			if (check && weight > emcWeight)
			{
				parameters.EmissiveColor = color.ToSRGB();
				emcWeight = weight;
			}
		}
		void MetallicValue(bool check, int weight, float value)
		{
			if (check && weight > metalWeight)
			{
				parameters.MetallicValue = value;
				metalWeight = weight;
			}
		}
		void Misc(bool check, int weight, UTexture tex)
		{
			if (check && weight > miscWeight)
			{
				parameters.Misc = tex;
				miscWeight = weight;
			}
		}
		void Normal(bool check, int weight, UTexture tex)
		{
			if (check && weight > normWeight)
			{
				parameters.Normal = tex;
				normWeight = weight;
			}
		}
		void Opacity(bool check, int weight, UTexture tex)
		{
			if (check && weight > opWeight)
			{
				parameters.Opacity = tex;
				opWeight = weight;
			}
		}
		void RoughnessValue(bool check, int weight, float value)
		{
			if (check && weight > roughWeight)
			{
				parameters.RoughnessValue = value;
				roughWeight = weight;
			}
		}
		void SpecPower(bool check, int weight, UTexture tex)
		{
			if (check && weight > specPowWeight)
			{
				parameters.SpecPower = tex;
				specPowWeight = weight;
			}
		}
		void Specular(bool check, int weight, UTexture tex)
		{
			if (check && weight > specWeight)
			{
				parameters.Specular = tex;
				specWeight = weight;
			}
		}
		void SpecularValue(bool check, int weight, float value)
		{
			if (check && weight > specuWeight)
			{
				parameters.SpecularValue = value;
				specuWeight = weight;
			}
		}
	}

	public override void GetParams(CMaterialParams2 parameters, EMaterialFormat format)
	{
		if (format != EMaterialFormat.FirstLayer && base.Parent != null && base.Parent != this)
		{
			base.Parent.GetParams(parameters, format);
		}
		parameters.AppendAllProperties(base.Properties);
		base.GetParams(parameters, format);
		FTextureParameterValue[] textureParameterValues = TextureParameterValues;
		foreach (FTextureParameterValue fTextureParameterValue in textureParameterValues)
		{
			if (fTextureParameterValue.ParameterValue.TryLoad(out UTexture export) && !parameters.VerifyTexture(fTextureParameterValue.Name, export))
			{
				parameters.VerifyTexture(export.Name, export);
			}
		}
		FVectorParameterValue[] vectorParameterValues = VectorParameterValues;
		foreach (FVectorParameterValue fVectorParameterValue in vectorParameterValues)
		{
			FLinearColor? parameterValue = fVectorParameterValue.ParameterValue;
			if (parameterValue.HasValue)
			{
				FLinearColor valueOrDefault = parameterValue.GetValueOrDefault();
				parameters.Colors[fVectorParameterValue.Name] = valueOrDefault;
			}
		}
		FScalarParameterValue[] scalarParameterValues = ScalarParameterValues;
		foreach (FScalarParameterValue fScalarParameterValue in scalarParameterValues)
		{
			parameters.Scalars[fScalarParameterValue.Name] = fScalarParameterValue.ParameterValue;
		}
	}

	public override void AppendReferencedTextures(IList<UUnrealMaterial> outTextures, bool onlyRendered)
	{
		if (onlyRendered)
		{
			base.AppendReferencedTextures(outTextures, onlyRendered: true);
			return;
		}
		FTextureParameterValue[] textureParameterValues = TextureParameterValues;
		for (int i = 0; i < textureParameterValues.Length; i++)
		{
			UTexture uTexture = textureParameterValues[i].ParameterValue.Load<UTexture>();
			if (uTexture != null && !outTextures.Contains(uTexture))
			{
				outTextures.Add(uTexture);
			}
		}
		if (base.Parent != null && base.Parent != this)
		{
			base.Parent.AppendReferencedTextures(outTextures, onlyRendered);
		}
	}
}
