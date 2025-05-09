using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Core.Math;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class CMaterialParams2
{
	public const string FallbackDiffuse = "PM_Diffuse";

	public const string FallbackNormals = "PM_Normals";

	public const string FallbackSpecularMasks = "PM_SpecularMasks";

	public const string FallbackEmissive = "PM_Emissive";

	public const string RegexDiffuse = ".*(?:Diff|_Tex|_?Albedo|_?Base_?Color).*|(?:_D|_DIF|_DM|_C|_CM)$";

	public const string RegexNormals = "^NO_|.*Norm.*|(?:_N|_NM|_NRM)$";

	public const string RegexSpecularMasks = "^SP_|.*(?:Specu|_S_|MR|(?<!no)RM).*|(?:_S|_LP|_PAK)$";

	public const string RegexEmissive = ".*Emiss.*|(?:_E|_EM)$";

	public EBlendMode BlendMode;

	public EMaterialShadingModel ShadingModel;

	public static readonly string[][] Diffuse = new string[8][]
	{
		new string[44]
		{
			"Trunk_BaseColor", "ShadedDiffuse", "Background Diffuse", "BG Diffuse Texture", "Diffuse", "Diffuse_1", "DiffuseTexture", "DiffuseMap", "Diffuse A", "Diffuse A Map",
			"Diffuse Top", "Diffuse Side", "Base Diffuse", "Diffuse Base", "Diffuse Base Map", "Diffuse Color Map", "DiffuseLayer1", "Albedo", "ALB", "TextureAlbedo",
			"Base Color Texture", "BaseColorTexture", "Base_Color", "Base Color", "BaseColor", "Base Texture Color", "BaseColorA", "BC", "Color", "CO",
			"CO_", "CO_1", "Base_CO", "Tex", "Tex_Color", "TexColor", "Tex_BaseColor", "AlbedMap", "Tex_Colormap", "Decal_Texture",
			"PetalDetailMap", "CliffTexture", "M1_T_BC", "Skin Diffuse"
		},
		new string[8] { "Background Diffuse 2", "Diffuse_Texture_2", "DiffuseLayer2", "Diffuse B", "Diffuse B Map", "BaseColorB", "CO_2", "M2_T_BC" },
		new string[8] { "Background Diffuse 3", "Diffuse_Texture_3", "DiffuseLayer3", "Diffuse C", "Diffuse C Map", "BaseColorC", "CO_3", "M3_T_BC" },
		new string[8] { "Background Diffuse 4", "Diffuse_Texture_4", "DiffuseLayer4", "Diffuse D", "Diffuse D Map", "BaseColorD", "CO_4", "M4_T_BC" },
		new string[8] { "Background Diffuse 5", "Diffuse_Texture_5", "DiffuseLayer5", "Diffuse E", "Diffuse E Map", "BaseColorE", "CO_5", "M5_T_BC" },
		new string[8] { "Background Diffuse 6", "Diffuse_Texture_6", "DiffuseLayer6", "Diffuse F", "Diffuse F Map", "BaseColorF", "CO_6", "M6_T_BC" },
		new string[8] { "Background Diffuse 7", "Diffuse_Texture_7", "DiffuseLayer7", "Diffuse G", "Diffuse G Map", "BaseColorG", "CO_7", "M7_T_BC" },
		new string[8] { "Background Diffuse 8", "Diffuse_Texture_8", "DiffuseLayer8", "Diffuse H", "Diffuse H Map", "BaseColorH", "CO_8", "M8_T_BC" }
	};

	public static readonly string[][] Normals = new string[8][]
	{
		new string[32]
		{
			"Trunk_Normal", "Normals", "Normal", "NormalA", "NormalTexture", "Normal Texture", "NormalMap", "Normal A Map", "T_Normal", "Normals Top",
			"Normals Side", "Fallback Normal", "Base_Normal", "Base Normal", "Normal Base", "TextureNormal", "Tex_BakedNormal", "TexNor", "BakedNormalMap", "Base Texture Normal",
			"Normal Base Map", "NM", "NM_1", "Base_NM", "NRM", "T_NRM", "M1_T_NRM", "Base NRM", "NRM Base", "Texture A Normal",
			"CliffNormal", "Skin Normal"
		},
		new string[6] { "Normals_Texture_2", "Texture B Normal", "NormalB", "Normal B Map", "NM_2", "M2_T_NRM" },
		new string[6] { "Normals_Texture_3", "Texture C Normal", "NormalC", "Normal C Map", "NM_3", "M3_T_NRM" },
		new string[6] { "Normals_Texture_4", "Texture D Normal", "NormalD", "Normal D Map", "NM_4", "M4_T_NRM" },
		new string[6] { "Normals_Texture_5", "Texture E Normal", "NormalE", "Normal E Map", "NM_5", "M5_T_NRM" },
		new string[6] { "Normals_Texture_6", "Texture F Normal", "NormalF", "Normal F Map", "NM_6", "M6_T_NRM" },
		new string[6] { "Normals_Texture_7", "Texture G Normal", "NormalG", "Normal G Map", "NM_7", "M7_T_NRM" },
		new string[6] { "Normals_Texture_8", "Texture H Normal", "NormalH", "Normal H Map", "NM_8", "M8_T_NRM" }
	};

	public static readonly string[][] SpecularMasks = new string[8][]
	{
		new string[43]
		{
			"Trunk_Specular", "PackedTexture", "SpecularMasks", "Specular", "SpecMap", "T_Specular", "Specular Top", "Specular Side", "MG", "ORM",
			"MRAE", "MRAS", "MRAO", "MRA", "MRA A", "MRS", "LP", "LP_1", "Base_LP", "TextureRMA",
			"Tex_MultiMask", "Tex_Multi", "TexMRC", "TexMRA", "TexRCN", "MultiMaskMap", "MRO Map", "MROA Map", "Base_SRO", "Base Texture RMAO",
			"Skin SRXO", "SRXO_Mask", "SRXO", "SROA", "SR", "SRO Map", "Pack", "PAK", "T_PAK", "M1_T_PAK",
			"Cliff Spec Texture", "PhysicalMap", "KizokMap"
		},
		new string[4] { "SpecularMasks_2", "MRA B", "LP_2", "M2_T_PAK" },
		new string[4] { "SpecularMasks_3", "MRA C", "LP_3", "M3_T_PAK" },
		new string[4] { "SpecularMasks_4", "MRA D", "LP_4", "M4_T_PAK" },
		new string[4] { "SpecularMasks_5", "MRA E", "LP_5", "M5_T_PAK" },
		new string[4] { "SpecularMasks_6", "MRA F", "LP_6", "M6_T_PAK" },
		new string[4] { "SpecularMasks_7", "MRA G", "LP_7", "M7_T_PAK" },
		new string[4] { "SpecularMasks_8", "MRA H", "LP_8", "M8_T_PAK" }
	};

	public static readonly string[][] Emissive = new string[8][]
	{
		new string[8] { "Emissive", "EmissiveTexture", "EmissiveColorTexture", "EmissiveColor", "EmissiveMask", "EmmisiveColor_A", "TextureEmissive", "TexEm" },
		new string[2] { "L1_Emissive", "EmmisiveColor_B" },
		new string[2] { "L2_Emissive", "EmmisiveColor_C" },
		new string[2] { "L3_Emissive", "EmmisiveColor_D" },
		new string[2] { "L4_Emissive", "EmmisiveColor_E" },
		new string[2] { "L5_Emissive", "EmmisiveColor_F" },
		new string[2] { "L6_Emissive", "EmmisiveColor_G" },
		new string[2] { "L7_Emissive", "EmmisiveColor_H" }
	};

	public static readonly string[][] DiffuseColors = new string[8][]
	{
		new string[9] { "ColorMult", "Color_mul", "Base Color", "BaseColor", "Color", "tex1_CO", "ColorA", "ALB", "AlbedoColor" },
		new string[2] { "tex2_CO", "ColorB" },
		new string[2] { "tex3_CO", "ColorC" },
		new string[2] { "tex4_CO", "ColorD" },
		new string[2] { "tex5_CO", "ColorE" },
		new string[2] { "tex6_CO", "ColorF" },
		new string[2] { "tex7_CO", "ColorG" },
		new string[2] { "tex8_CO", "ColorH" }
	};

	public static readonly string[][] EmissiveColors = new string[8][]
	{
		new string[6] { "Emissive", "Emissive Color", "EmissiveColor", "EMI", "EmColor", "Color" },
		new string[2] { "Emissive1", "Color01" },
		new string[2] { "Emissive2", "Color02" },
		new string[2] { "Emissive3", "Color03" },
		new string[2] { "Emissive4", "Color04" },
		new string[2] { "Emissive5", "Color05" },
		new string[2] { "Emissive6", "Color06" },
		new string[2] { "Emissive7", "Color07" }
	};

	[JsonIgnore]
	public readonly Dictionary<string, UUnrealMaterial> Textures = new Dictionary<string, UUnrealMaterial>();

	public readonly Dictionary<string, FLinearColor> Colors = new Dictionary<string, FLinearColor>();

	public readonly Dictionary<string, float> Scalars = new Dictionary<string, float>();

	public readonly Dictionary<string, bool> Switches = new Dictionary<string, bool>();

	public readonly Dictionary<string, object?> Properties = new Dictionary<string, object>();

	public bool HasTopDiffuse => HasTopTexture(Diffuse[0]);

	public bool HasTopNormals => HasTopTexture(Normals[0]);

	public bool HasTopSpecularMasks => HasTopTexture(SpecularMasks[0]);

	public bool HasTopEmissive => HasTopTexture(Emissive[0]);

	public bool IsTranslucent => BlendMode == EBlendMode.BLEND_Translucent;

	public bool IsNull => Textures.Count == 0;

	public IEnumerable<UUnrealMaterial> GetTextures(IEnumerable<string> names)
	{
		foreach (string name in names)
		{
			if (Textures.TryGetValue(name, out UUnrealMaterial value))
			{
				yield return value;
			}
		}
	}

	public IEnumerable<UUnrealMaterial?> GetTexturesOrNull(IEnumerable<string> names)
	{
		foreach (string name in names)
		{
			if (Textures.TryGetValue(name, out UUnrealMaterial value))
			{
				yield return value;
			}
			else
			{
				yield return null;
			}
		}
	}

	public IEnumerable<UUnrealMaterial> GetTexturesByRegex(Regex regex)
	{
		foreach (var (input, uUnrealMaterial2) in Textures)
		{
			if (regex.IsMatch(input))
			{
				yield return uUnrealMaterial2;
			}
		}
	}

	public bool TryGetFirstTexture2d(out UTexture2D? texture)
	{
		if (Textures.First().Value is UTexture2D uTexture2D)
		{
			texture = uTexture2D;
			return true;
		}
		texture = null;
		return false;
	}

	public TextureMapping[] GetTextureMapping(int numTexCoords, params string[][] names)
	{
		int i = 0;
		int j = 0;
		TextureMapping[] array = new TextureMapping[numTexCoords];
		for (int k = 0; k < array.Length; k++)
		{
			for (; i < names.Length; i++)
			{
				bool flag = false;
				for (; j < names[i].Length; j++)
				{
					if (Textures.ContainsKey(names[i][j]))
					{
						array[k] = new TextureMapping
						{
							UVSet = i,
							Index = j
						};
						flag = true;
						break;
					}
				}
				j++;
				if (flag)
				{
					break;
				}
				j = 0;
			}
		}
		return array;
	}

	public bool TryGetTexture2d(out UTexture2D? texture, params string[] names)
	{
		for (int i = 0; i < names.Length; i++)
		{
			if (Textures.TryGetValue(names[i], out UUnrealMaterial value) && value is UTexture2D uTexture2D)
			{
				texture = uTexture2D;
				return true;
			}
		}
		texture = null;
		return false;
	}

	public bool TryGetLinearColor(out FLinearColor linearColor, params string[] names)
	{
		foreach (string key in names)
		{
			if (Colors.TryGetValue(key, out linearColor))
			{
				return true;
			}
		}
		linearColor = default(FLinearColor);
		return false;
	}

	public bool TryGetScalar(out float scalar, params string[] names)
	{
		foreach (string key in names)
		{
			if (Scalars.TryGetValue(key, out scalar))
			{
				return true;
			}
		}
		scalar = 0f;
		return false;
	}

	public void AppendAllProperties(IList<FPropertyTag> properties)
	{
		foreach (FPropertyTag property in properties)
		{
			switch (property.Name.Text)
			{
			case "Parent":
			case "TextureParameterValues":
			case "VectorParameterValues":
			case "ScalarParameterValues":
			case "StaticParameters":
			case "CachedReferencedTextures":
			case "TextureStreamingData":
			case "BlendMode":
			case "ShadingModel":
				continue;
			}
			Properties[property.Name.Text] = property.Tag?.GenericValue;
		}
	}

	public bool VerifyTexture(string name, UTexture texture, bool appendToDictionary = true, EMaterialSamplerType samplerType = EMaterialSamplerType.SAMPLERTYPE_Color)
	{
		string text = "";
		if (Regex.IsMatch(name, ".*(?:Diff|_Tex|_?Albedo|_?Base_?Color).*|(?:_D|_DIF|_DM|_C|_CM)$", RegexOptions.IgnoreCase))
		{
			text = "PM_Diffuse";
		}
		else if (samplerType == EMaterialSamplerType.SAMPLERTYPE_Normal || Regex.IsMatch(name, "^NO_|.*Norm.*|(?:_N|_NM|_NRM)$", RegexOptions.IgnoreCase))
		{
			text = "PM_Normals";
		}
		else if (Regex.IsMatch(name, "^SP_|.*(?:Specu|_S_|MR|(?<!no)RM).*|(?:_S|_LP|_PAK)$", RegexOptions.IgnoreCase))
		{
			text = "PM_SpecularMasks";
		}
		else if (Regex.IsMatch(name, ".*Emiss.*|(?:_E|_EM)$", RegexOptions.IgnoreCase))
		{
			text = "PM_Emissive";
		}
		bool num = !string.IsNullOrEmpty(text);
		if (num)
		{
			Textures[text] = texture;
		}
		if (appendToDictionary)
		{
			Textures[name] = texture;
		}
		return num;
	}

	private bool HasTopTexture(params string[] names)
	{
		foreach (string key in names)
		{
			if (Textures.ContainsKey(key))
			{
				return true;
			}
		}
		return false;
	}
}
