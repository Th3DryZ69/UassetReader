using System;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports;

public class FLifetimeProperty
{
	public ushort RepIndex;

	public ELifetimeCondition Condition;

	public ELifetimeRepNotifyCondition RepNotifyCondition;

	public FLifetimeProperty()
	{
		RepIndex = 0;
		Condition = ELifetimeCondition.COND_None;
		RepNotifyCondition = ELifetimeRepNotifyCondition.REPNOTIFY_OnChanged;
	}

	public FLifetimeProperty(int repIndex)
	{
		RepIndex = (ushort)repIndex;
		Condition = ELifetimeCondition.COND_None;
		RepNotifyCondition = ELifetimeRepNotifyCondition.REPNOTIFY_OnChanged;
	}

	public FLifetimeProperty(int repIndex, ELifetimeCondition condition, ELifetimeRepNotifyCondition repNotifyCondition = ELifetimeRepNotifyCondition.REPNOTIFY_OnChanged)
	{
		RepIndex = (ushort)repIndex;
		Condition = condition;
		RepNotifyCondition = repNotifyCondition;
	}

	protected bool Equals(FLifetimeProperty other)
	{
		if (RepIndex == other.RepIndex && Condition == other.Condition)
		{
			return RepNotifyCondition == other.RepNotifyCondition;
		}
		return false;
	}

	public override bool Equals(object? obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((FLifetimeProperty)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(RepIndex, (int)Condition, (int)RepNotifyCondition);
	}

	public static bool operator ==(FLifetimeProperty a, FLifetimeProperty b)
	{
		if (a.RepIndex == b.RepIndex && a.Condition == b.Condition)
		{
			return a.RepNotifyCondition == b.RepNotifyCondition;
		}
		return false;
	}

	public static bool operator !=(FLifetimeProperty a, FLifetimeProperty b)
	{
		return !(a == b);
	}
}
