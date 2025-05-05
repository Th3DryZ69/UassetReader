using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class UMapBuildDataRegistry : UObject
{
	public Dictionary<FGuid, FMeshMapBuildData>? MeshBuildData;

	public Dictionary<FGuid, FPrecomputedLightVolumeData>? LevelPrecomputedLightVolumeBuildData;

	public Dictionary<FGuid, FPrecomputedVolumetricLightmapData>? LevelPrecomputedVolumetricLightmapBuildData;

	public Dictionary<FGuid, FLightComponentMapBuildData>? LightBuildData;

	public Dictionary<FGuid, FReflectionCaptureMapBuildData>? ReflectionCaptureBuildData;

	public Dictionary<FGuid, FSkyAtmosphereMapBuildData>? SkyAtmosphereBuildData;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		if (new FStripDataFlags(Ar).IsDataStrippedForServer())
		{
			return;
		}
		int num = Ar.Read<int>();
		MeshBuildData = new Dictionary<FGuid, FMeshMapBuildData>(num);
		for (int i = 0; i < num; i++)
		{
			MeshBuildData[Ar.Read<FGuid>()] = new FMeshMapBuildData(Ar);
		}
		num = Ar.Read<int>();
		LevelPrecomputedLightVolumeBuildData = new Dictionary<FGuid, FPrecomputedLightVolumeData>(num);
		for (int j = 0; j < num; j++)
		{
			LevelPrecomputedLightVolumeBuildData[Ar.Read<FGuid>()] = new FPrecomputedLightVolumeData(Ar);
		}
		if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.VolumetricLightmaps)
		{
			num = Ar.Read<int>();
			LevelPrecomputedVolumetricLightmapBuildData = new Dictionary<FGuid, FPrecomputedVolumetricLightmapData>(num);
			for (int k = 0; k < num; k++)
			{
				LevelPrecomputedVolumetricLightmapBuildData[Ar.Read<FGuid>()] = new FPrecomputedVolumetricLightmapData(Ar);
			}
		}
		num = Ar.Read<int>();
		LightBuildData = new Dictionary<FGuid, FLightComponentMapBuildData>(num);
		for (int l = 0; l < num; l++)
		{
			LightBuildData[Ar.Read<FGuid>()] = new FLightComponentMapBuildData(Ar);
		}
		if (FReflectionCaptureObjectVersion.Get(Ar) >= FReflectionCaptureObjectVersion.Type.MoveReflectionCaptureDataToMapBuildData)
		{
			num = Ar.Read<int>();
			ReflectionCaptureBuildData = new Dictionary<FGuid, FReflectionCaptureMapBuildData>(num);
			for (int m = 0; m < num; m++)
			{
				ReflectionCaptureBuildData[Ar.Read<FGuid>()] = new FReflectionCaptureMapBuildData(Ar);
			}
		}
		if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.SkyAtmosphereStaticLightingVersioning)
		{
			num = Ar.Read<int>();
			SkyAtmosphereBuildData = new Dictionary<FGuid, FSkyAtmosphereMapBuildData>(num);
			for (int n = 0; n < num; n++)
			{
				SkyAtmosphereBuildData[Ar.Read<FGuid>()] = new FSkyAtmosphereMapBuildData(Ar);
			}
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		Dictionary<FGuid, FMeshMapBuildData>? meshBuildData = MeshBuildData;
		if (meshBuildData != null && meshBuildData.Count > 0)
		{
			writer.WritePropertyName("MeshBuildData");
			serializer.Serialize(writer, MeshBuildData);
		}
		Dictionary<FGuid, FPrecomputedLightVolumeData>? levelPrecomputedLightVolumeBuildData = LevelPrecomputedLightVolumeBuildData;
		if (levelPrecomputedLightVolumeBuildData != null && levelPrecomputedLightVolumeBuildData.Count > 0)
		{
			writer.WritePropertyName("LevelPrecomputedLightVolumeBuildData");
			serializer.Serialize(writer, LevelPrecomputedLightVolumeBuildData);
		}
		Dictionary<FGuid, FPrecomputedVolumetricLightmapData>? levelPrecomputedVolumetricLightmapBuildData = LevelPrecomputedVolumetricLightmapBuildData;
		if (levelPrecomputedVolumetricLightmapBuildData != null && levelPrecomputedVolumetricLightmapBuildData.Count > 0)
		{
			writer.WritePropertyName("LevelPrecomputedVolumetricLightmapBuildData");
			serializer.Serialize(writer, LevelPrecomputedVolumetricLightmapBuildData);
		}
		Dictionary<FGuid, FLightComponentMapBuildData>? lightBuildData = LightBuildData;
		if (lightBuildData != null && lightBuildData.Count > 0)
		{
			writer.WritePropertyName("LightBuildData");
			serializer.Serialize(writer, LightBuildData);
		}
		Dictionary<FGuid, FReflectionCaptureMapBuildData>? reflectionCaptureBuildData = ReflectionCaptureBuildData;
		if (reflectionCaptureBuildData != null && reflectionCaptureBuildData.Count > 0)
		{
			writer.WritePropertyName("ReflectionCaptureBuildData");
			serializer.Serialize(writer, ReflectionCaptureBuildData);
		}
		Dictionary<FGuid, FSkyAtmosphereMapBuildData>? skyAtmosphereBuildData = SkyAtmosphereBuildData;
		if (skyAtmosphereBuildData != null && skyAtmosphereBuildData.Count > 0)
		{
			writer.WritePropertyName("SkyAtmosphereBuildData");
			serializer.Serialize(writer, SkyAtmosphereBuildData);
		}
	}
}
