namespace CUE4Parse.UE4.Objects.Engine.Curves;

internal readonly struct WeightedKeyDataAdapter : IKeyDataAdapter
{
	private unsafe readonly byte* _interpModes;

	private unsafe readonly float* _keyData;

	public unsafe WeightedKeyDataAdapter(byte* basePtr, int interpModesOffset, IKeyTimeAdapter keyTimeAdapter)
	{
		_interpModes = basePtr + interpModesOffset;
		_keyData = (float*)(basePtr + keyTimeAdapter.KeyDataOffset);
	}

	public int GetKeyDataHandle(int keyIndexToQuery)
	{
		return keyIndexToQuery * 5;
	}

	public unsafe float GetKeyValue(int handle)
	{
		return _keyData[handle];
	}

	public unsafe float GetKeyArriveTangent(int handle)
	{
		return _keyData[handle + 1];
	}

	public unsafe float GetKeyLeaveTangent(int handle)
	{
		return _keyData[handle + 2];
	}

	public unsafe float GetKeyArriveTangentWeight(int handle)
	{
		return _keyData[handle + 3];
	}

	public unsafe float GetKeyLeaveTangentWeight(int handle)
	{
		return _keyData[handle + 4];
	}

	public unsafe ERichCurveCompressionFormat GetKeyInterpMode(int keyIndex)
	{
		return (ERichCurveCompressionFormat)_interpModes[keyIndex];
	}

	public unsafe ERichCurveTangentWeightMode GetKeyTangentWeightMode(int keyIndex)
	{
		return (ERichCurveTangentWeightMode)_interpModes[keyIndex + 1];
	}
}
