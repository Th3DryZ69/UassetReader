using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialUniformPreshaderHeader
{
	public readonly uint OpcodeOffset;

	public readonly uint OpcodeSize;

	public readonly uint? BufferOffset;

	public readonly EValueComponentType? ComponentType;

	public readonly byte? NumComponents;

	public readonly uint? FieldIndex;

	public readonly uint? NumFields;

	public FMaterialUniformPreshaderHeader(FMemoryImageArchive Ar)
	{
		OpcodeOffset = Ar.Read<uint>();
		OpcodeSize = Ar.Read<uint>();
		if (Ar.Game == EGame.GAME_UE5_0)
		{
			BufferOffset = Ar.Read<uint>();
			ComponentType = Ar.Read<EValueComponentType>();
			NumComponents = Ar.Read<byte>();
			Ar.Position += 2L;
		}
		else if (Ar.Game >= EGame.GAME_UE5_1)
		{
			FieldIndex = Ar.Read<uint>();
			NumFields = Ar.Read<uint>();
		}
	}
}
