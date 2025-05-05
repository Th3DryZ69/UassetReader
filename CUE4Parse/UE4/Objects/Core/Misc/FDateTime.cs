using System;

namespace CUE4Parse.UE4.Objects.Core.Misc;

public readonly struct FDateTime : IUStruct
{
	public readonly long Ticks;

	public override string ToString()
	{
		return $"{new DateTime(Ticks):F}";
	}
}
