using System;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class UStaticMesh : UObject
{
	public bool bCooked { get; private set; }

	public FPackageIndex BodySetup { get; private set; }

	public FPackageIndex NavCollision { get; private set; }

	public FGuid LightingGuid { get; private set; }

	public FPackageIndex[] Sockets { get; private set; }

	public FStaticMeshRenderData? RenderData { get; private set; }

	public FStaticMaterial[]? StaticMaterials { get; private set; }

	public ResolvedObject?[] Materials { get; private set; }

	public int LODForCollision { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		Materials = Array.Empty<ResolvedObject>();
		LODForCollision = GetOrDefault("LODForCollision", 0);
		FStripDataFlags fStripDataFlags = Ar.Read<FStripDataFlags>();
		bCooked = Ar.ReadBoolean();
		BodySetup = new FPackageIndex(Ar);
		if (Ar.Versions["StaticMesh.HasNavCollision"])
		{
			NavCollision = new FPackageIndex(Ar);
		}
		if (!fStripDataFlags.IsEditorDataStripped())
		{
			Log.Warning("Static Mesh with Editor Data not implemented yet");
			Ar.Position = validPos;
			return;
		}
		LightingGuid = Ar.Read<FGuid>();
		Sockets = Ar.ReadArray(() => new FPackageIndex(Ar));
		RenderData = new FStaticMeshRenderData(Ar, bCooked);
		if (bCooked)
		{
			EGame game = Ar.Game;
			if (game >= EGame.GAME_UE4_20 && game < EGame.GAME_UE5_0 && Ar.ReadBoolean())
			{
				Ar.ReadArray<FVector>();
				Ar.ReadArray<ushort>();
			}
		}
		FPackageIndex[] obj;
		if (Ar.Game >= EGame.GAME_UE4_14)
		{
			bool flag = Ar.ReadBoolean();
			if (flag)
			{
				Ar.Position = validPos;
			}
			if (FEditorObjectVersion.Get(Ar) < FEditorObjectVersion.Type.RefactorMeshEditorMaterials)
			{
				return;
			}
			if (flag)
			{
				StaticMaterials = GetOrDefault("StaticMaterials", Array.Empty<FStaticMaterial>());
			}
			else
			{
				StaticMaterials = Ar.ReadArray(() => new FStaticMaterial(Ar));
			}
			Materials = new ResolvedObject[StaticMaterials.Length];
			for (int num = 0; num < Materials.Length; num++)
			{
				Materials[num] = StaticMaterials[num].MaterialInterface;
			}
		}
		else if (TryGetValue<FPackageIndex[]>(out obj, "Materials"))
		{
			Materials = new ResolvedObject[obj.Length];
			for (int num2 = 0; num2 < obj.Length; num2++)
			{
				Materials[num2] = obj[num2].ResolvedObject;
			}
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("BodySetup");
		serializer.Serialize(writer, BodySetup);
		writer.WritePropertyName("NavCollision");
		serializer.Serialize(writer, NavCollision);
		writer.WritePropertyName("LightingGuid");
		serializer.Serialize(writer, LightingGuid);
		writer.WritePropertyName("RenderData");
		serializer.Serialize(writer, RenderData);
	}
}
