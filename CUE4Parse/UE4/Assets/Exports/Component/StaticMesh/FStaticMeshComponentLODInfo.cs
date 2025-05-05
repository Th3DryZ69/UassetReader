using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.Meshes;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;

[JsonConverter(typeof(FStaticMeshComponentLODInfoConverter))]
public class FStaticMeshComponentLODInfo
{
	private const byte OverrideColorsStripFlag = 1;

	public readonly FGuid MapBuildDataId;

	public readonly FPaintedVertex[] PaintedVertices;

	public readonly FColorVertexBuffer? OverrideVertexColors;

	public FStaticMeshComponentLODInfo(FArchive Ar)
	{
		FStripDataFlags fStripDataFlags = new FStripDataFlags(Ar);
		if (!fStripDataFlags.IsDataStrippedForServer())
		{
			MapBuildDataId = Ar.Read<FGuid>();
		}
		if (!fStripDataFlags.IsClassDataStripped(1) && Ar.Read<byte>() == 1)
		{
			OverrideVertexColors = new FColorVertexBuffer(Ar);
		}
		if (!fStripDataFlags.IsEditorDataStripped())
		{
			PaintedVertices = Ar.ReadArray(() => new FPaintedVertex(Ar));
		}
		if (Ar.Game == EGame.GAME_StarWarsJediSurvivor)
		{
			Ar.Position += 20L;
		}
	}
}
