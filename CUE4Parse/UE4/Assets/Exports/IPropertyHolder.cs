using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Objects;

namespace CUE4Parse.UE4.Assets.Exports;

public interface IPropertyHolder
{
	List<FPropertyTag> Properties { get; }
}
