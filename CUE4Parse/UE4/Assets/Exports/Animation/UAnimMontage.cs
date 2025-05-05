using System;
using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class UAnimMontage : UAnimCompositeBase
{
	public FCompositeSection[] CompositeSections;

	public FSlotAnimationTrack[] SlotAnimTracks;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		CompositeSections = GetOrDefault("CompositeSections", Array.Empty<FCompositeSection>());
		SlotAnimTracks = GetOrDefault("SlotAnimTracks", Array.Empty<FSlotAnimationTrack>());
	}
}
