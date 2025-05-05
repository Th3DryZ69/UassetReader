using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.RenderCore;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FMorphTargetDelta
{
	public readonly FVector PositionDelta;

	public readonly FVector TangentZDelta;

	public readonly uint SourceIdx;

	public FMorphTargetDelta(FArchive Ar)
	{
		PositionDelta = Ar.Read<FVector>();
		if (Ar.Ver < EUnrealEngineObjectUE4Version.MORPHTARGET_CPU_TANGENTZDELTA_FORMATCHANGE)
		{
			TangentZDelta = (FVector)Ar.Read<FDeprecatedSerializedPackedNormal>();
		}
		else
		{
			TangentZDelta = Ar.Read<FVector>();
		}
		SourceIdx = Ar.Read<uint>();
	}
}
