using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine;

public class UModel : CUE4Parse.UE4.Assets.Exports.UObject
{
	public FBoxSphereBounds Bounds;

	public FVector[] Vectors;

	public FVector[] Points;

	public FBspNode[] Nodes;

	public FBspSurf[] Surfs;

	public FVert[] Verts;

	public int NumSharedSides;

	public bool RootOutside;

	public bool Linked;

	public uint NumUniqueVertices;

	public FModelVertexBuffer VertexBuffer;

	public FGuid LightingGuid;

	public FLightmassPrimitiveSettings[] LightmassSettings;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		FStripDataFlags fStripDataFlags = new FStripDataFlags(Ar);
		Bounds = new FBoxSphereBounds(Ar);
		Vectors = Ar.ReadBulkArray<FVector>();
		Points = Ar.ReadBulkArray<FVector>();
		Nodes = Ar.ReadBulkArray<FBspNode>();
		if (Ar.Ver < EUnrealEngineObjectUE4Version.BSP_UNDO_FIX)
		{
			new FPackageIndex(Ar);
			Surfs = Ar.ReadArray(() => new FBspSurf(Ar));
		}
		else
		{
			Surfs = Ar.ReadArray(() => new FBspSurf(Ar));
		}
		Verts = Ar.ReadBulkArray<FVert>();
		NumSharedSides = Ar.Read<int>();
		if (Ar.Ver < EUnrealEngineObjectUE4Version.REMOVE_ZONES_FROM_MODEL)
		{
			Ar.ReadArray<FZoneProperties>();
		}
		if (!Ar.IsFilterEditorOnly || Ar.Ver < EUnrealEngineObjectUE4Version.REMOVE_UNUSED_UPOLYS_FROM_UMODEL)
		{
			new FPackageIndex(Ar);
			Ar.SkipBulkArrayData();
			Ar.SkipBulkArrayData();
		}
		RootOutside = Ar.ReadBoolean();
		Linked = Ar.ReadBoolean();
		if (Ar.Ver < EUnrealEngineObjectUE4Version.REMOVE_ZONES_FROM_MODEL)
		{
			Ar.ReadBulkArray<int>();
		}
		NumUniqueVertices = Ar.Read<uint>();
		if (!fStripDataFlags.IsEditorDataStripped() || !fStripDataFlags.IsClassDataStripped(1))
		{
			VertexBuffer = new FModelVertexBuffer(Ar);
		}
		LightingGuid = Ar.Read<FGuid>();
		LightmassSettings = Ar.ReadArray(() => new FLightmassPrimitiveSettings(Ar));
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("Bounds");
		serializer.Serialize(writer, Bounds);
		writer.WritePropertyName("Vectors");
		serializer.Serialize(writer, Vectors);
		writer.WritePropertyName("Points");
		serializer.Serialize(writer, Points);
		writer.WritePropertyName("Nodes");
		serializer.Serialize(writer, Nodes);
		writer.WritePropertyName("Surfs");
		serializer.Serialize(writer, Surfs);
		writer.WritePropertyName("NumSharedSides");
		serializer.Serialize(writer, NumSharedSides);
		writer.WritePropertyName("VertexBuffer");
		serializer.Serialize(writer, VertexBuffer);
		writer.WritePropertyName("LightingGuid");
		serializer.Serialize(writer, LightingGuid);
		writer.WritePropertyName("LightmassSettings");
		serializer.Serialize(writer, LightmassSettings);
	}
}
