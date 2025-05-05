using System.Linq;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public struct FVirtualTextureTileOffsetData : IUStruct
{
	public uint Width;

	public uint Height;

	public uint MaxAddress;

	public uint[] Addresses;

	public uint[] Offsets;

	public FVirtualTextureTileOffsetData(FArchive Ar)
	{
		Width = Ar.Read<uint>();
		Height = Ar.Read<uint>();
		MaxAddress = Ar.Read<uint>();
		Addresses = Ar.ReadArray<uint>();
		Offsets = Ar.ReadArray<uint>();
	}

	public uint GetTileOffset(uint InAddress)
	{
		uint num = Addresses.FirstOrDefault((uint x) => x >= InAddress);
		uint num2 = Offsets[num];
		if (num2 == uint.MaxValue)
		{
			return uint.MaxValue;
		}
		uint num3 = Addresses[num];
		uint num4 = InAddress - num3;
		return num2 + num4;
	}
}
