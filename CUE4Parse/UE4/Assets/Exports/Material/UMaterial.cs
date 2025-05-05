using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class UMaterial : UMaterialInterface
{
	private readonly List<IObject> _displayedReferencedTextures = new List<IObject>();

	private bool _shouldDisplay;

	public bool TwoSided { get; private set; }

	public bool bDisableDepthTest { get; private set; }

	public bool bIsMasked { get; private set; }

	public FPackageIndex[] Expressions { get; private set; }

	public EBlendMode BlendMode { get; private set; }

	public EMaterialShadingModel ShadingModel { get; private set; }

	public float OpacityMaskClipValue { get; private set; } = 0.333f;

	public List<UTexture> ReferencedTextures { get; } = new List<UTexture>();

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		TwoSided = GetOrDefault("TwoSided", defaultValue: false);
		bDisableDepthTest = GetOrDefault("bDisableDepthTest", defaultValue: false);
		bIsMasked = GetOrDefault("bIsMasked", defaultValue: false);
		Expressions = GetOrDefault("Expressions", Array.Empty<FPackageIndex>());
		BlendMode = GetOrDefault("BlendMode", EBlendMode.BLEND_Opaque);
		ShadingModel = GetOrDefault("ShadingModel", EMaterialShadingModel.MSM_Unlit);
		OpacityMaskClipValue = GetOrDefault("OpacityMaskClipValue", 0.333f);
		if (Ar.Game >= EGame.GAME_UE4_25)
		{
			CachedExpressionData = GetOrDefault<FStructFallback>("CachedExpressionData");
			if (CachedExpressionData != null && CachedExpressionData.TryGetValue<UTexture[]>(out var obj, "ReferencedTextures"))
			{
				ReferencedTextures.AddRange(obj);
			}
			if (TryGetValue<UTexture[]>(out obj, "ReferencedTextures"))
			{
				ReferencedTextures.AddRange(obj);
			}
		}
		if (Ar.Game >= EGame.GAME_UE5_0)
		{
			ScanForTextures(Ar);
		}
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.PURGED_FMATERIAL_COMPILE_OUTPUTS)
		{
			Ar.Position = validPos;
		}
	}

	public UTexture? GetFirstTexture()
	{
		if (ReferencedTextures.Count <= 0)
		{
			return null;
		}
		return ReferencedTextures[0];
	}

	public UTexture? GetTextureAtIndex(int index)
	{
		if (ReferencedTextures.Count < index)
		{
			return null;
		}
		return ReferencedTextures[index];
	}

	private void ScanForTextures(FAssetArchive Ar)
	{
		IPackage owner = Ar.Owner;
		if (!(owner is IoPackage ioPackage))
		{
			if (owner is Package package)
			{
				for (int i = 0; i < package.ImportMap.Length; i++)
				{
					if (package.ImportMap[i].ClassName.Text.StartsWith("Texture", StringComparison.OrdinalIgnoreCase))
					{
						ResolvedObject resolvedObject = package.ResolvePackageIndex(new FPackageIndex(Ar, -i - 1));
						if (resolvedObject?.Class != null && resolvedObject.TryLoad(out UObject export) && export is UTexture item)
						{
							_displayedReferencedTextures.Add(resolvedObject);
							ReferencedTextures.Add(item);
						}
					}
				}
			}
		}
		else
		{
			FPackageObjectIndex[] importMap = ioPackage.ImportMap;
			foreach (FPackageObjectIndex index in importMap)
			{
				ResolvedObject resolvedObject2 = ioPackage.ResolveObjectIndex(index);
				if (resolvedObject2?.Class != null && resolvedObject2.Class.Name.Text.StartsWith("Texture", StringComparison.OrdinalIgnoreCase) && resolvedObject2.TryLoad(out UObject export2) && export2 is UTexture item2)
				{
					_displayedReferencedTextures.Add(resolvedObject2);
					ReferencedTextures.Add(item2);
				}
			}
		}
		_shouldDisplay = _displayedReferencedTextures.Count > 0;
	}

	public override void GetParams(CMaterialParams parameters)
	{
		base.GetParams(parameters);
		int diffWeight = 0;
		int normWeight = 0;
		int specWeight = 0;
		int specPowWeight = 0;
		int opWeight = 0;
		int emWeight = 0;
		for (int i = 0; i < ReferencedTextures.Count; i++)
		{
			UTexture uTexture = ReferencedTextures[i];
			if (uTexture == null)
			{
				continue;
			}
			string name = uTexture.Name;
			if (!name.Contains("noise", StringComparison.CurrentCultureIgnoreCase) && !name.Contains("detail", StringComparison.CurrentCultureIgnoreCase))
			{
				Diffuse(name.Contains("diff", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				Normal(name.Contains("norm", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				Diffuse(name.EndsWith("_Tex", StringComparison.CurrentCultureIgnoreCase), 80, uTexture);
				Diffuse(name.Contains("_Tex", StringComparison.CurrentCultureIgnoreCase), 60, uTexture);
				Diffuse(name.Contains("_D", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				Opacity(name.Contains("_OM", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				Diffuse(name.Contains("_DI", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				Diffuse(name.Contains("_D", StringComparison.CurrentCultureIgnoreCase), 11, uTexture);
				Diffuse(name.Contains("_Albedo", StringComparison.CurrentCultureIgnoreCase), 19, uTexture);
				Diffuse(name.EndsWith("_C", StringComparison.CurrentCultureIgnoreCase), 10, uTexture);
				Diffuse(name.EndsWith("_CM", StringComparison.CurrentCultureIgnoreCase), 12, uTexture);
				Normal(name.EndsWith("_N", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				Normal(name.EndsWith("_NM", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				Normal(name.Contains("_N", StringComparison.CurrentCultureIgnoreCase), 9, uTexture);
				Specular(name.EndsWith("_S", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				Specular(name.Contains("_S_", StringComparison.CurrentCultureIgnoreCase), 15, uTexture);
				SpecPower(name.EndsWith("_SP", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				SpecPower(name.EndsWith("_SM", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				SpecPower(name.Contains("_SP", StringComparison.CurrentCultureIgnoreCase), 9, uTexture);
				Emissive(name.EndsWith("_E", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				Emissive(name.EndsWith("_EM", StringComparison.CurrentCultureIgnoreCase), 21, uTexture);
				Opacity(name.EndsWith("_A", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				if (bIsMasked)
				{
					Opacity(name.EndsWith("_Mask", StringComparison.CurrentCultureIgnoreCase), 2, uTexture);
				}
				Diffuse(name.StartsWith("df_", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				Specular(name.StartsWith("sp_", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				Normal(name.StartsWith("no_", StringComparison.CurrentCultureIgnoreCase), 20, uTexture);
				Normal(name.Contains("Norm", StringComparison.CurrentCultureIgnoreCase), 80, uTexture);
				Emissive(name.Contains("Emis", StringComparison.CurrentCultureIgnoreCase), 80, uTexture);
				Specular(name.Contains("Specular", StringComparison.CurrentCultureIgnoreCase), 80, uTexture);
				Opacity(name.Contains("Opac", StringComparison.CurrentCultureIgnoreCase), 80, uTexture);
				Opacity(name.Contains("Alpha", StringComparison.CurrentCultureIgnoreCase), 100, uTexture);
				Diffuse(i == 0, 1, uTexture);
			}
		}
		if (parameters.Diffuse != parameters.Normal || diffWeight >= normWeight)
		{
			UUnrealMaterial diffuse = parameters.Diffuse;
			if (diffuse == null || !diffuse.IsTextureCube)
			{
				return;
			}
		}
		parameters.Diffuse = null;
		void Diffuse(bool check, int weight, UTexture tex)
		{
			if (check && weight > diffWeight)
			{
				parameters.Diffuse = tex;
				diffWeight = weight;
			}
		}
		void Emissive(bool check, int weight, UTexture tex)
		{
			if (check && weight > emWeight)
			{
				parameters.Emissive = tex;
				emWeight = weight;
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
	}

	public override void GetParams(CMaterialParams2 parameters, EMaterialFormat format)
	{
		parameters.BlendMode = BlendMode;
		parameters.ShadingModel = ShadingModel;
		parameters.AppendAllProperties(base.Properties);
		FPackageIndex[] expressions = Expressions;
		for (int i = 0; i < expressions.Length; i++)
		{
			if (!expressions[i].TryLoad(out UMaterialExpression export))
			{
				continue;
			}
			if (!(export is UMaterialExpressionTextureSampleParameter uMaterialExpressionTextureSampleParameter))
			{
				if (!(export is UMaterialExpressionTextureBase uMaterialExpressionTextureBase))
				{
					if (!(export is UMaterialExpressionVectorParameter uMaterialExpressionVectorParameter))
					{
						if (!(export is UMaterialExpressionScalarParameter uMaterialExpressionScalarParameter))
						{
							if (export is UMaterialExpressionStaticBoolParameter uMaterialExpressionStaticBoolParameter)
							{
								parameters.Switches[uMaterialExpressionStaticBoolParameter.ParameterName.Text] = uMaterialExpressionStaticBoolParameter.DefaultValue;
							}
						}
						else
						{
							parameters.Scalars[uMaterialExpressionScalarParameter.ParameterName.Text] = uMaterialExpressionScalarParameter.DefaultValue;
						}
					}
					else
					{
						parameters.Colors[uMaterialExpressionVectorParameter.ParameterName.Text] = uMaterialExpressionVectorParameter.DefaultValue;
					}
				}
				else
				{
					parameters.VerifyTexture(uMaterialExpressionTextureBase.Texture.Name, uMaterialExpressionTextureBase.Texture, appendToDictionary: true, uMaterialExpressionTextureBase.SamplerType);
				}
			}
			else
			{
				parameters.VerifyTexture(uMaterialExpressionTextureSampleParameter.ParameterName.Text, uMaterialExpressionTextureSampleParameter.Texture, appendToDictionary: true, uMaterialExpressionTextureSampleParameter.SamplerType);
			}
		}
		if (format != EMaterialFormat.AllLayersNoRef)
		{
			for (int j = 0; j < ReferencedTextures.Count; j++)
			{
				UTexture uTexture = ReferencedTextures[j];
				if (uTexture != null)
				{
					parameters.Textures[uTexture.Name] = uTexture;
				}
			}
		}
		base.GetParams(parameters, format);
		if (format == EMaterialFormat.AllLayersNoRef)
		{
			return;
		}
		if (ReferencedTextures.Count == 1)
		{
			UTexture uTexture2 = ReferencedTextures[0];
			if (uTexture2 != null)
			{
				parameters.Textures["PM_Diffuse"] = uTexture2;
				return;
			}
		}
		int num = ReferencedTextures.Count;
		while ((!parameters.Textures.ContainsKey("PM_Diffuse") || !parameters.Textures.ContainsKey("PM_Normals") || !parameters.Textures.ContainsKey("PM_SpecularMasks") || !parameters.Textures.ContainsKey("PM_Emissive")) && num > 0)
		{
			num--;
			UTexture uTexture3 = ReferencedTextures[num];
			if (uTexture3 != null)
			{
				if (!parameters.Textures.ContainsKey("PM_Diffuse") && Regex.IsMatch(uTexture3.Name, ".*(?:Diff|_Tex|_?Albedo|_?Base_?Color).*|(?:_D|_DIF|_DM|_C|_CM)$", RegexOptions.IgnoreCase))
				{
					parameters.Textures["PM_Diffuse"] = uTexture3;
				}
				else if (!parameters.Textures.ContainsKey("PM_Normals") && Regex.IsMatch(uTexture3.Name, "^NO_|.*Norm.*|(?:_N|_NM|_NRM)$", RegexOptions.IgnoreCase))
				{
					parameters.Textures["PM_Normals"] = uTexture3;
				}
				else if (!parameters.Textures.ContainsKey("PM_SpecularMasks") && Regex.IsMatch(uTexture3.Name, "^SP_|.*(?:Specu|_S_|MR|(?<!no)RM).*|(?:_S|_LP|_PAK)$", RegexOptions.IgnoreCase))
				{
					parameters.Textures["PM_SpecularMasks"] = uTexture3;
				}
				else if (!parameters.Textures.ContainsKey("PM_Emissive") && Regex.IsMatch(uTexture3.Name, ".*Emiss.*|(?:_E|_EM)$", RegexOptions.IgnoreCase))
				{
					parameters.Textures["PM_Emissive"] = uTexture3;
				}
			}
		}
	}

	public override void AppendReferencedTextures(IList<UUnrealMaterial> outTextures, bool onlyRendered)
	{
		if (onlyRendered)
		{
			base.AppendReferencedTextures(outTextures, onlyRendered);
			return;
		}
		foreach (UTexture item in ReferencedTextures.Where((UTexture texture) => !outTextures.Contains(texture)))
		{
			if (item != null)
			{
				outTextures.Add(item);
			}
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		if (_shouldDisplay)
		{
			writer.WritePropertyName("ReferencedTextures");
			serializer.Serialize(writer, _displayedReferencedTextures);
		}
	}
}
