using System;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FMorphTargetLODModel
{
	public readonly FMorphTargetDelta[] Vertices;

	public readonly int NumBaseMeshVerts;

	public readonly int[] SectionIndices;

	public readonly bool bGeneratedByEngine;

	public FMorphTargetLODModel()
	{
		Vertices = Array.Empty<FMorphTargetDelta>();
		NumBaseMeshVerts = 0;
		SectionIndices = Array.Empty<int>();
		bGeneratedByEngine = false;
	}

	public FMorphTargetLODModel(FArchive Ar)
	{
		if (FEditorObjectVersion.Get(Ar) < FEditorObjectVersion.Type.AddedMorphTargetSectionIndices)
		{
			Vertices = Ar.ReadArray(() => new FMorphTargetDelta(Ar));
			NumBaseMeshVerts = Ar.Read<int>();
			bGeneratedByEngine = false;
			return;
		}
		if (FFortniteMainBranchObjectVersion.Get(Ar) < FFortniteMainBranchObjectVersion.Type.SaveGeneratedMorphTargetByEngine)
		{
			Vertices = Ar.ReadArray(() => new FMorphTargetDelta(Ar));
			NumBaseMeshVerts = Ar.Read<int>();
			SectionIndices = Ar.ReadArray<int>();
			bGeneratedByEngine = false;
			return;
		}
		bool flag = false;
		if (FUE5PrivateFrostyStreamObjectVersion.Get(Ar) >= FUE5PrivateFrostyStreamObjectVersion.Type.StripMorphTargetSourceDataForCookedBuilds)
		{
			flag = Ar.ReadBoolean();
		}
		if (flag)
		{
			Ar.Position += 4L;
			Vertices = Array.Empty<FMorphTargetDelta>();
		}
		else
		{
			Vertices = Ar.ReadArray(() => new FMorphTargetDelta(Ar));
		}
		NumBaseMeshVerts = Ar.Read<int>();
		SectionIndices = Ar.ReadArray<int>();
		bGeneratedByEngine = Ar.ReadBoolean();
	}
}
