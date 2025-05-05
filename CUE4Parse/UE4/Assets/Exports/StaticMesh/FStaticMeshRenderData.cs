using CUE4Parse.UE4.Assets.Exports.Nanite;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

[JsonConverter(typeof(FStaticMeshRenderDataConverter))]
public class FStaticMeshRenderData
{
	private const int MAX_STATIC_UV_SETS_UE4 = 8;

	private const int MAX_STATIC_LODS_UE4 = 8;

	public readonly FStaticMeshLODResources[] LODs;

	public readonly FNaniteResources? NaniteResources;

	public readonly FBoxSphereBounds Bounds;

	public readonly bool bLODsShareStaticLighting;

	public readonly float[]? ScreenSize;

	public FStaticMeshRenderData(FAssetArchive Ar, bool bCooked)
	{
		if (!bCooked)
		{
			return;
		}
		if (Ar.Versions["StaticMesh.KeepMobileMinLODSettingOnDesktop"])
		{
			Ar.Read<int>();
		}
		if (Ar.Game == EGame.GAME_HYENAS)
		{
			Ar.Position++;
		}
		LODs = Ar.ReadArray(() => new FStaticMeshLODResources(Ar));
		if (Ar.Game >= EGame.GAME_UE4_23)
		{
			Ar.Read<byte>();
		}
		if (Ar.Game >= EGame.GAME_UE5_0)
		{
			NaniteResources = new FNaniteResources(Ar);
			SerializeInlineDataRepresentations(Ar);
		}
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.RENAME_CROUCHMOVESCHARACTERDOWN)
		{
			bool flag = false;
			if (Ar.Ver >= EUnrealEngineObjectUE4Version.RENAME_WIDGET_VISIBILITY)
			{
				FStripDataFlags fStripDataFlags = Ar.Read<FStripDataFlags>();
				flag = fStripDataFlags.IsDataStrippedForServer();
				if (Ar.Game >= EGame.GAME_UE4_21)
				{
					flag |= fStripDataFlags.IsClassDataStripped(1);
				}
			}
			if (!flag)
			{
				for (int num = 0; num < LODs.Length; num++)
				{
					if (Ar.ReadBoolean())
					{
						if (Ar.Game >= EGame.GAME_UE5_0)
						{
							new FDistanceFieldVolumeData5(Ar);
						}
						else
						{
							new FDistanceFieldVolumeData(Ar);
						}
					}
				}
			}
		}
		Bounds = new FBoxSphereBounds(Ar);
		if (Ar.Versions["StaticMesh.HasLODsShareStaticLighting"])
		{
			bLODsShareStaticLighting = Ar.ReadBoolean();
		}
		if (Ar.Game < EGame.GAME_UE4_14)
		{
			Ar.ReadBoolean();
		}
		if (FRenderingObjectVersion.Get(Ar) < FRenderingObjectVersion.Type.TextureStreamingMeshUVChannelData)
		{
			Ar.Position += 32L;
			Ar.Position += 4L;
		}
		ScreenSize = new float[(Ar.Game >= EGame.GAME_UE4_9) ? 8 : 4];
		for (int num2 = 0; num2 < ScreenSize.Length; num2++)
		{
			if (Ar.Game >= EGame.GAME_UE4_20)
			{
				Ar.ReadBoolean();
			}
			ScreenSize[num2] = Ar.Read<float>();
			if (Ar.Game == EGame.GAME_HogwartsLegacy)
			{
				Ar.Position += 8L;
			}
		}
		if (Ar.Game == EGame.GAME_Borderlands3)
		{
			int num3 = Ar.Read<int>();
			for (int num4 = 0; num4 < num3; num4++)
			{
				byte b = Ar.Read<byte>();
				Ar.Position += b * 12;
			}
		}
	}

	private void SerializeInlineDataRepresentations(FAssetArchive Ar)
	{
		FStripDataFlags fStripDataFlags = new FStripDataFlags(Ar);
		if (fStripDataFlags.IsDataStrippedForServer() || fStripDataFlags.IsClassDataStripped(2))
		{
			return;
		}
		FStaticMeshLODResources[] lODs = LODs;
		foreach (FStaticMeshLODResources fStaticMeshLODResources in lODs)
		{
			if (Ar.ReadBoolean())
			{
				fStaticMeshLODResources.CardRepresentationData = new FCardRepresentationData(Ar);
			}
		}
	}
}
