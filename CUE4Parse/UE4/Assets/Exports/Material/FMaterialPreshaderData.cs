using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialPreshaderData
{
	public FName[]? Names;

	public uint[]? NamesOffset;

	public FPreshaderStructType[]? StructTypes;

	public EValueComponentType[]? StructComponentTypes;

	public byte[] Data;

	public FMaterialPreshaderData(FMemoryImageArchive Ar)
	{
		if (Ar.Game >= EGame.GAME_UE4_26)
		{
			Names = Ar.ReadArray(Ar.ReadFName);
		}
		if (Ar.Game == EGame.GAME_UE5_0)
		{
			NamesOffset = Ar.ReadArray<uint>();
		}
		else if (Ar.Game >= EGame.GAME_UE5_1)
		{
			StructTypes = Ar.ReadArray<FPreshaderStructType>();
			StructComponentTypes = Ar.ReadArray<EValueComponentType>();
		}
		Data = Ar.ReadArray<byte>();
	}
}
