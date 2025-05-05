namespace CUE4Parse.UE4.Objects.Engine.Curves;

internal readonly struct UniformKeyDataAdapter : IKeyDataAdapter
{
	private readonly ERichCurveCompressionFormat _format;

	private unsafe readonly float* _keyData;

	public unsafe UniformKeyDataAdapter(ERichCurveCompressionFormat format, byte* basePtr, IKeyTimeAdapter keyTimeAdapter)
	{
		_format = format;
		_keyData = (float*)(basePtr + keyTimeAdapter.KeyDataOffset);
	}

	public int GetKeyDataHandle(int keyIndexToQuery)
	{
		if (_format != ERichCurveCompressionFormat.RCCF_Cubic)
		{
			return keyIndexToQuery;
		}
		return keyIndexToQuery * 3;
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

	public ERichCurveCompressionFormat GetKeyInterpMode(int keyIndex)
	{
		return _format;
	}

	public ERichCurveTangentWeightMode GetKeyTangentWeightMode(int keyIndex)
	{
		return ERichCurveTangentWeightMode.RCTWM_WeightedNone;
	}

	public float GetKeyArriveTangentWeight(int handle)
	{
		return 0f;
	}

	public float GetKeyLeaveTangentWeight(int handle)
	{
		return 0f;
	}
}
