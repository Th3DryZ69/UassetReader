using System;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public record FPixelFormatInfo(EPixelFormat UnrealFormat, string Name, int BlockSizeX, int BlockSizeY, int BlockSizeZ, int BlockBytes, int NumComponents, bool Supported)
{
	public int GetBlockCountForWidth(int width)
	{
		if (BlockSizeX > 0)
		{
			return (width + BlockSizeX - 1) / BlockSizeX;
		}
		return 0;
	}

	public int GetBlockCountForHeight(int height)
	{
		if (BlockSizeY > 0)
		{
			return (height + BlockSizeY - 1) / BlockSizeY;
		}
		return 0;
	}

	public int GetBlockCountForDepth(int depth)
	{
		if (BlockSizeZ > 0)
		{
			return (depth + BlockSizeZ - 1) / BlockSizeZ;
		}
		return 0;
	}

	public int Get2DImageSizeInBytes(int width, int height)
	{
		int blockCountForWidth = GetBlockCountForWidth(width);
		int blockCountForHeight = GetBlockCountForHeight(height);
		return blockCountForWidth * blockCountForHeight * BlockBytes;
	}

	public int Get2DTextureMipSizeInBytes(int width, int height, int mipIdx)
	{
		int width2 = Math.Max(width >> mipIdx, 1);
		int height2 = Math.Max(height >> mipIdx, 1);
		return Get2DImageSizeInBytes(width2, height2);
	}

	public int Get2DTextureSizeInBytes(int width, int height, int mipCount)
	{
		int num = 0;
		int num2 = width;
		int num3 = height;
		for (int i = 0; i < mipCount; i++)
		{
			num += Get2DImageSizeInBytes(num2, num3);
			num2 = Math.Max(num2 >> 1, 1);
			num3 = Math.Max(num3 >> 1, 1);
		}
		return num;
	}
}
