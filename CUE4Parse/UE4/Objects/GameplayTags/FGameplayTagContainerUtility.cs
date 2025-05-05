using System.Collections.Generic;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Objects.GameplayTags;

public static class FGameplayTagContainerUtility
{
	public static bool TryGetGameplayTag(this IEnumerable<FName> gameplayTags, string startWith, out FName gameplayTag)
	{
		foreach (FName gameplayTag2 in gameplayTags)
		{
			if (!gameplayTag2.IsNone && gameplayTag2.Text.StartsWith(startWith))
			{
				gameplayTag = gameplayTag2;
				return true;
			}
		}
		gameplayTag = default(FName);
		return false;
	}

	public static IList<string> GetAllGameplayTags(this IEnumerable<FName> gameplayTags, params string[] startWith)
	{
		List<string> list = new List<string>();
		foreach (FName gameplayTag in gameplayTags)
		{
			if (gameplayTag.IsNone)
			{
				continue;
			}
			foreach (string value in startWith)
			{
				if (gameplayTag.Text.StartsWith(value))
				{
					list.Add(gameplayTag.Text);
				}
			}
		}
		return list;
	}
}
