using System;
using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public abstract class UAnimSequenceBase : UAnimationAsset
{
	public float SequenceLength;

	public float RateScale;

	public FAnimNotifyEvent[] Notifies;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		SequenceLength = GetOrDefault("SequenceLength", 0f);
		RateScale = GetOrDefault("RateScale", 1f);
		Notifies = GetOrDefault("Notifies", Array.Empty<FAnimNotifyEvent>());
	}
}
