namespace CUE4Parse.UE4.Objects.Engine.Curves;

internal interface IKeyDataAdapter
{
	int GetKeyDataHandle(int keyIndexToQuery);

	float GetKeyValue(int handle);

	float GetKeyArriveTangent(int handle);

	float GetKeyLeaveTangent(int handle);

	ERichCurveCompressionFormat GetKeyInterpMode(int keyIndex);

	ERichCurveTangentWeightMode GetKeyTangentWeightMode(int keyIndex);

	float GetKeyArriveTangentWeight(int handle);

	float GetKeyLeaveTangentWeight(int handle);
}
