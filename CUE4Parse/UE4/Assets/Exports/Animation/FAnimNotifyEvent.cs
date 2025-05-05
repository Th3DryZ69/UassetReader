using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

[StructFallback]
public class FAnimNotifyEvent : FAnimLinkableElement
{
	public float TriggerTimeOffset;

	public float EndTriggerTimeOffset;

	public float TriggerWeightThreshold;

	public FName NotifyName;

	public FPackageIndex Notify;

	public FPackageIndex NotifyStateClass;

	public float Duration;

	public FAnimLinkableElement EndLink;

	public bool bConvertedFromBranchingPoint;

	public EMontageNotifyTickType MontageTickType;

	public float NotifyTriggerChance;

	public EMontageNotifyTickType NotifyFilterType;

	public int NotifyFilterLOD;

	public bool bTriggerOnDedicatedServer;

	public bool bTriggerOnFollower;

	public int TrackIndex;

	public FAnimNotifyEvent(FStructFallback fallback)
		: base(fallback)
	{
		TriggerTimeOffset = fallback.GetOrDefault("TriggerTimeOffset", 0f);
		EndTriggerTimeOffset = fallback.GetOrDefault("EndTriggerTimeOffset", 0f);
		TriggerWeightThreshold = fallback.GetOrDefault("TriggerWeightThreshold", 0f);
		NotifyName = fallback.GetOrDefault<FName>("NotifyName");
		Notify = fallback.GetOrDefault<FPackageIndex>("Notify");
		NotifyStateClass = fallback.GetOrDefault<FPackageIndex>("NotifyStateClass");
		Duration = fallback.GetOrDefault("Duration", 0f);
		EndLink = fallback.GetOrDefault<FAnimLinkableElement>("EndLink");
		bConvertedFromBranchingPoint = fallback.GetOrDefault("bConvertedFromBranchingPoint", defaultValue: false);
		MontageTickType = fallback.GetOrDefault("MontageTickType", EMontageNotifyTickType.Queued);
		NotifyTriggerChance = fallback.GetOrDefault("NotifyTriggerChance", 0f);
		NotifyFilterType = fallback.GetOrDefault("NotifyFilterType", EMontageNotifyTickType.Queued);
		NotifyFilterLOD = fallback.GetOrDefault("NotifyFilterLOD", 0);
		bTriggerOnDedicatedServer = fallback.GetOrDefault("bTriggerOnDedicatedServer", defaultValue: false);
		bTriggerOnFollower = fallback.GetOrDefault("bTriggerOnFollower", defaultValue: false);
		TrackIndex = fallback.GetOrDefault("TrackIndex", 0);
	}
}
