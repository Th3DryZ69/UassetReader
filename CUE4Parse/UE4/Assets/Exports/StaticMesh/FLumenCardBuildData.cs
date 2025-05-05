using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FLumenCardBuildData
{
	public FLumenCardOBB OBB;

	public byte LODLevel;

	public byte AxisAlignedDirectionIndex;

	public FLumenCardBuildData(FArchive Ar)
	{
		OBB = Ar.Read<FLumenCardOBB>();
		LODLevel = (byte)((Ar.Game < EGame.GAME_UE5_1) ? Ar.Read<byte>() : 0);
		AxisAlignedDirectionIndex = Ar.Read<byte>();
	}
}
