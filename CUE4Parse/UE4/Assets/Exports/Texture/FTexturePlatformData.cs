using System;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public class FTexturePlatformData
{
	private const uint BitMask_CubeMap = 2147483648u;

	private const uint BitMask_HasOptData = 1073741824u;

	private const uint BitMask_NumSlices = 1073741823u;

	public readonly int SizeX;

	public int SizeY;

	public readonly uint PackedData;

	public readonly string PixelFormat;

	public readonly FOptTexturePlatformData OptData;

	public readonly int FirstMipToSerialize;

	public readonly FTexture2DMipMap[] Mips;

	public readonly FVirtualTextureBuiltData? VTData;

	public FTexturePlatformData()
	{
		SizeX = 0;
		SizeY = 0;
		PackedData = 0u;
		PixelFormat = string.Empty;
		OptData = default(FOptTexturePlatformData);
		FirstMipToSerialize = -1;
		Mips = Array.Empty<FTexture2DMipMap>();
		VTData = null;
	}

	public FTexturePlatformData(FAssetArchive Ar, UTexture Owner)
	{
		if (Ar != null && Ar.Game >= EGame.GAME_UE5_0 && Ar.IsFilterEditorOnly)
		{
			Ar.Position += 16L;
		}
		if (Ar.Game == EGame.GAME_PlayerUnknownsBattlegrounds)
		{
			SizeX = Ar.Read<short>();
			SizeY = Ar.Read<short>();
			byte[] array = Ar.ReadBytes(3);
			PackedData = (uint)(array[0] + (array[1] << 8) + (array[2] << 16));
		}
		else
		{
			SizeX = Ar.Read<int>();
			SizeY = Ar.Read<int>();
			PackedData = Ar.Read<uint>();
		}
		PixelFormat = ((Ar.Game == EGame.GAME_GearsOfWar4) ? Ar.ReadFName().Text : Ar.ReadFString());
		if (Ar.Game == EGame.GAME_FinalFantasy7Remake && (PackedData & 0xFFFF) == 16384)
		{
			Ar.Read<int>();
			Ar.Read<int>();
			Ar.Read<int>();
		}
		if (HasOptData())
		{
			OptData = Ar.Read<FOptTexturePlatformData>();
		}
		FirstMipToSerialize = Ar.Read<int>();
		int num = Ar.Read<int>();
		if (Ar.Game == EGame.GAME_FinalFantasy7Remake)
		{
			new FTexture2DMipMap(Ar);
			Ar.Read<int>();
			_ = PackedData;
			Ar.Position += 4L;
		}
		Mips = new FTexture2DMipMap[num];
		for (int i = 0; i < Mips.Length; i++)
		{
			Mips[i] = new FTexture2DMipMap(Ar);
			if (Owner is UVolumeTexture)
			{
				Mips[i].SizeX *= GetNumSlices();
			}
			else if (Owner is UTextureCube)
			{
				Mips[i].SizeY *= GetNumSlices();
			}
		}
		if (Ar.Versions["VirtualTextures"] && Ar.Platform != ETexturePlatform.Playstation && Ar.ReadBoolean())
		{
			VTData = new FVirtualTextureBuiltData(Ar, FirstMipToSerialize);
		}
		if (Mips.Length != 0)
		{
			SizeX = Mips[0].SizeX;
			SizeY = Mips[0].SizeY;
			if (Owner is UVolumeTexture)
			{
				PackedData = (uint)(((ulong)Mips[0].SizeZ & 0x3FFFFFFFuL) | (uint)((int)PackedData & -1073741824));
			}
		}
		else if (VTData != null)
		{
			SizeX = (int)VTData.Width;
			SizeY = (int)VTData.Height;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasOptData()
	{
		return (PackedData & 0x40000000) == 1073741824;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsCubemap()
	{
		return (PackedData & 0x80000000u) == 2147483648u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetNumSlices()
	{
		return (int)(PackedData & 0x3FFFFFFF);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetNumMipsInTail()
	{
		return (int)OptData.NumMipsInTail;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetExtData()
	{
		return (int)OptData.ExtData;
	}
}
