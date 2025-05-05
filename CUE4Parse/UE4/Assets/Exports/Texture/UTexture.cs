using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Assets.Exports.Material;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public abstract class UTexture : UUnrealMaterial
{
	public FGuid LightingGuid { get; private set; }

	public TextureCompressionSettings CompressionSettings { get; private set; }

	public bool SRGB { get; private set; }

	public EPixelFormat Format { get; protected set; }

	public FTexturePlatformData PlatformData { get; private set; } = new FTexturePlatformData();

	public bool IsVirtual => PlatformData.VTData != null;

	public bool IsNormalMap => CompressionSettings == TextureCompressionSettings.TC_Normalmap;

	public bool RenderNearestNeighbor { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		LightingGuid = GetOrDefault("LightingGuid", new FGuid((uint)GetFullName().GetHashCode()));
		CompressionSettings = GetOrDefault("CompressionSettings", TextureCompressionSettings.TC_Default);
		SRGB = GetOrDefault("SRGB", defaultValue: true);
		if (TryGetValue<FName>(out var obj, "LODGroup", "Filter") && !obj.IsNone)
		{
			RenderNearestNeighbor = obj.Text.EndsWith("TEXTUREGROUP_Pixels2D", StringComparison.OrdinalIgnoreCase) || obj.Text.EndsWith("TF_Nearest", StringComparison.OrdinalIgnoreCase);
		}
		Ar.Read<FStripDataFlags>().IsEditorDataStripped();
	}

	protected void DeserializeCookedPlatformData(FAssetArchive Ar, bool bSerializeMipData = true)
	{
		FName fName = Ar.ReadFName();
		while (!fName.IsNone)
		{
			Enum.TryParse<EPixelFormat>(fName.Text, out var result);
			EGame game = Ar.Game;
			long num = ((game >= EGame.GAME_UE5_0) ? (Ar.AbsolutePosition + Ar.Read<long>()) : ((game < EGame.GAME_UE4_20) ? Ar.Read<int>() : Ar.Read<long>()));
			long num2 = num;
			if (Format == EPixelFormat.PF_Unknown)
			{
				PlatformData = new FTexturePlatformData(Ar, this);
				if (Ar.Game == EGame.GAME_SeaOfThieves)
				{
					Ar.Position += 4L;
				}
				if (Ar.AbsolutePosition != num2)
				{
					Log.Warning($"Texture2D read incorrectly. Offset {Ar.AbsolutePosition}, Skip Offset {num2}, Bytes remaining {num2 - Ar.AbsolutePosition}");
					Ar.SeekAbsolute(num2, SeekOrigin.Begin);
				}
				Format = result;
			}
			else
			{
				Ar.SeekAbsolute(num2, SeekOrigin.Begin);
			}
			fName = Ar.ReadFName();
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("SizeX");
		writer.WriteValue(PlatformData.SizeX);
		writer.WritePropertyName("SizeY");
		writer.WriteValue(PlatformData.SizeY);
		writer.WritePropertyName("PackedData");
		writer.WriteValue(PlatformData.PackedData);
		writer.WritePropertyName("PixelFormat");
		writer.WriteValue(Format.ToString());
		if (PlatformData.OptData.ExtData != 0 && PlatformData.OptData.NumMipsInTail != 0)
		{
			writer.WritePropertyName("OptData");
			serializer.Serialize(writer, PlatformData.OptData);
		}
		writer.WritePropertyName("FirstMipToSerialize");
		writer.WriteValue(PlatformData.FirstMipToSerialize);
		FTexture2DMipMap[] mips = PlatformData.Mips;
		if (mips != null && mips.Length > 0)
		{
			writer.WritePropertyName("Mips");
			serializer.Serialize(writer, PlatformData.Mips);
		}
		if (PlatformData.VTData != null)
		{
			writer.WritePropertyName("VTData");
			serializer.Serialize(writer, PlatformData.VTData);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FTexture2DMipMap? GetFirstMip()
	{
		return PlatformData.Mips.FirstOrDefault((FTexture2DMipMap x) => x.BulkData.Data != null);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FTexture2DMipMap? GetMipByMaxSize(int maxSize)
	{
		FTexture2DMipMap[] mips = PlatformData.Mips;
		foreach (FTexture2DMipMap fTexture2DMipMap in mips)
		{
			if ((fTexture2DMipMap.SizeX <= maxSize || fTexture2DMipMap.SizeY <= maxSize) && fTexture2DMipMap.BulkData.Data != null)
			{
				return fTexture2DMipMap;
			}
		}
		return GetFirstMip();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FTexture2DMipMap? GetMipBySize(int sizeX, int sizeY)
	{
		FTexture2DMipMap[] mips = PlatformData.Mips;
		foreach (FTexture2DMipMap fTexture2DMipMap in mips)
		{
			if (fTexture2DMipMap.SizeX == sizeX && fTexture2DMipMap.SizeY == sizeY && fTexture2DMipMap.BulkData.Data != null)
			{
				return fTexture2DMipMap;
			}
		}
		return GetFirstMip();
	}

	public override void GetParams(CMaterialParams parameters)
	{
	}

	public override void GetParams(CMaterialParams2 parameters, EMaterialFormat format)
	{
	}
}
