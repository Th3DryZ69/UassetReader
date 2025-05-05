using System;
using System.Linq;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public class FVirtualTextureBuiltData
{
	public readonly uint NumLayers;

	public readonly uint? NumMips;

	public readonly uint Width;

	public readonly uint Height;

	public readonly uint WidthInBlocks;

	public readonly uint HeightInBlocks;

	public readonly uint TileSize;

	public readonly uint TileBorderSize;

	public readonly EPixelFormat[] LayerTypes;

	public readonly FVirtualTextureDataChunk[] Chunks;

	public readonly uint[]? TileIndexPerChunk;

	public readonly uint[]? TileIndexPerMip;

	public readonly uint[]? TileOffsetInChunk;

	public readonly uint[]? ChunkIndexPerMip;

	public readonly uint[]? BaseOffsetPerMip;

	public readonly uint[]? TileDataOffsetPerLayer;

	public readonly FVirtualTextureTileOffsetData[] TileOffsetData;

	public readonly FLinearColor[] LayerFallbackColors;

	public FVirtualTextureBuiltData(FAssetArchive Ar, int firstMip)
	{
		FVirtualTextureBuiltData fVirtualTextureBuiltData = this;
		bool num = firstMip > 0;
		Ar.ReadBoolean();
		NumLayers = Ar.Read<uint>();
		WidthInBlocks = Ar.Read<uint>();
		HeightInBlocks = Ar.Read<uint>();
		TileSize = Ar.Read<uint>();
		TileBorderSize = Ar.Read<uint>();
		if (Ar.Game >= EGame.GAME_UE5_0)
		{
			TileDataOffsetPerLayer = Ar.ReadArray<uint>();
		}
		if (!num)
		{
			NumMips = Ar.Read<uint>();
			Width = Ar.Read<uint>();
			Height = Ar.Read<uint>();
			if (Ar.Game >= EGame.GAME_UE5_0)
			{
				ChunkIndexPerMip = Ar.ReadArray<uint>();
				BaseOffsetPerMip = Ar.ReadArray<uint>();
				TileOffsetData = Ar.ReadArray(() => new FVirtualTextureTileOffsetData(Ar));
			}
			TileIndexPerChunk = Ar.ReadArray<uint>();
			TileIndexPerMip = Ar.ReadArray<uint>();
			TileOffsetInChunk = Ar.ReadArray<uint>();
		}
		LayerTypes = Ar.ReadArray((int)NumLayers, () => (EPixelFormat)Enum.Parse(typeof(EPixelFormat), Ar.ReadFString()));
		if (Ar.Game >= EGame.GAME_UE5_0)
		{
			LayerFallbackColors = new FLinearColor[NumLayers];
			for (int num2 = 0; num2 < NumLayers; num2++)
			{
				LayerFallbackColors[num2] = Ar.Read<FLinearColor>();
			}
		}
		Chunks = Ar.ReadArray(() => new FVirtualTextureDataChunk(Ar, fVirtualTextureBuiltData.NumLayers));
	}

	public int GetChunkIndex(int vLevel)
	{
		if (ChunkIndexPerMip == null || vLevel >= ChunkIndexPerMip.Length)
		{
			return -1;
		}
		return (int)ChunkIndexPerMip[vLevel];
	}

	private bool IsLegacyData()
	{
		if (TileOffsetInChunk != null)
		{
			return TileOffsetInChunk.Length != 0;
		}
		return true;
	}

	public uint GetTileOffset(int vLevel, uint vAddress, uint LayerIndex)
	{
		uint result = 0u;
		if (IsLegacyData())
		{
			throw new NotImplementedException("TODO: Legacy data");
		}
		if (BaseOffsetPerMip != null && BaseOffsetPerMip.Length > vLevel && TileOffsetData.Length > vLevel)
		{
			uint num = BaseOffsetPerMip[vLevel];
			uint tileOffset = TileOffsetData[vLevel].GetTileOffset(vAddress);
			if (num != uint.MaxValue && tileOffset != uint.MaxValue)
			{
				uint num2 = TileDataOffsetPerLayer.Last();
				uint num3 = ((LayerIndex != 0) ? TileDataOffsetPerLayer[LayerIndex - 1] : 0u);
				result = num + tileOffset * num2 + num3;
			}
		}
		return result;
	}
}
