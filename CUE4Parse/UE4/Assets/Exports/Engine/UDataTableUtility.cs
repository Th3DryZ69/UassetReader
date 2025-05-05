using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Engine;

public static class UDataTableUtility
{
	public static bool TryGetDataTableRow(this UDataTable dataTable, string rowKey, StringComparison comparisonType, out FStructFallback rowValue)
	{
		foreach (KeyValuePair<FName, FStructFallback> item in dataTable.RowMap)
		{
			if (!item.Key.IsNone && item.Key.Text.Equals(rowKey, comparisonType))
			{
				rowValue = item.Value;
				return true;
			}
		}
		rowValue = null;
		return false;
	}
}
