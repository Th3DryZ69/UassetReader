using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.LevelSequence;

public readonly struct FLevelSequenceObjectReferenceMap : IUStruct, IReadOnlyDictionary<FGuid, FLevelSequenceLegacyObjectReference>, IEnumerable<KeyValuePair<FGuid, FLevelSequenceLegacyObjectReference>>, IEnumerable, IReadOnlyCollection<KeyValuePair<FGuid, FLevelSequenceLegacyObjectReference>>
{
	public readonly IDictionary<FGuid, FLevelSequenceLegacyObjectReference> Map;

	public FLevelSequenceLegacyObjectReference this[FGuid key] => Map[key];

	public IEnumerable<FGuid> Keys => Map.Keys.AsEnumerable();

	public IEnumerable<FLevelSequenceLegacyObjectReference> Values => Map.Values.AsEnumerable();

	public int Count => Map.Count;

	public FLevelSequenceObjectReferenceMap(FArchive Ar)
	{
		Map = new Dictionary<FGuid, FLevelSequenceLegacyObjectReference>(Ar.Read<int>());
		for (int i = 0; i < Map.Count; i++)
		{
			Map[Ar.Read<FGuid>()] = new FLevelSequenceLegacyObjectReference(Ar);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool ContainsKey(FGuid key)
	{
		return Map.ContainsKey(key);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public IEnumerator<KeyValuePair<FGuid, FLevelSequenceLegacyObjectReference>> GetEnumerator()
	{
		return Map.GetEnumerator();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryGetValue(FGuid key, out FLevelSequenceLegacyObjectReference value)
	{
		return Map.TryGetValue(key, out value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)Map).GetEnumerator();
	}
}
