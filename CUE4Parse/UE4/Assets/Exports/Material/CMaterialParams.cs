using System.Collections.Generic;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class CMaterialParams
{
	public UUnrealMaterial? Diffuse;

	public UUnrealMaterial? Normal;

	public UUnrealMaterial? Specular;

	public UUnrealMaterial? SpecPower;

	public UUnrealMaterial? Opacity;

	public UUnrealMaterial? Emissive;

	public UUnrealMaterial? Cube;

	public UUnrealMaterial? Mask;

	public UUnrealMaterial? Misc;

	public bool IsTransparent;

	public bool HasTopDiffuseTexture;

	public bool HasTopEmissiveTexture;

	public float RoughnessValue = 1f;

	public float MetallicValue;

	public float SpecularValue;

	public ETextureChannel EmissiveChannel;

	public ETextureChannel SpecularMaskChannel;

	public ETextureChannel SpecularPowerChannel;

	public ETextureChannel CubemapMaskChannel;

	public FLinearColor? DiffuseColor;

	public FLinearColor? EmissiveColor;

	public bool UseMobileSpecular;

	public float MobileSpecularPower;

	public EMobileSpecularMask MobileSpecularMask;

	public bool SpecularFromAlpha;

	public bool OpacityFromAlpha;

	public bool IsNull
	{
		get
		{
			if (Diffuse == null && Normal == null && Specular == null && SpecPower == null && Opacity == null && Emissive == null && Cube == null && Mask == null)
			{
				return Misc == null;
			}
			return false;
		}
	}

	public void AppendAllTextures(IList<UUnrealMaterial> outTextures)
	{
		if (Diffuse != null)
		{
			outTextures.Add(Diffuse);
		}
		if (Normal != null)
		{
			outTextures.Add(Normal);
		}
		if (Specular != null)
		{
			outTextures.Add(Specular);
		}
		if (SpecPower != null)
		{
			outTextures.Add(SpecPower);
		}
		if (Opacity != null)
		{
			outTextures.Add(Opacity);
		}
		if (Emissive != null)
		{
			outTextures.Add(Emissive);
		}
		if (Cube != null)
		{
			outTextures.Add(Cube);
		}
		if (Mask != null)
		{
			outTextures.Add(Mask);
		}
		if (Misc != null)
		{
			outTextures.Add(Misc);
		}
	}
}
