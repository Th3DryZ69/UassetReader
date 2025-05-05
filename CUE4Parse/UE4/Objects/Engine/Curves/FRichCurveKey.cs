using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CUE4Parse.UE4.Objects.Engine.Curves;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FRichCurveKey : IUStruct
{
	[JsonConverter(typeof(StringEnumConverter))]
	public ERichCurveInterpMode InterpMode;

	[JsonConverter(typeof(StringEnumConverter))]
	public ERichCurveTangentMode TangentMode;

	[JsonConverter(typeof(StringEnumConverter))]
	public ERichCurveTangentWeightMode TangentWeightMode;

	public float Time;

	public float Value;

	public float ArriveTangent;

	public float ArriveTangentWeight;

	public float LeaveTangent;

	public float LeaveTangentWeight;

	public FRichCurveKey(float time, float value)
	{
		InterpMode = ERichCurveInterpMode.RCIM_Linear;
		TangentMode = ERichCurveTangentMode.RCTM_Auto;
		TangentWeightMode = ERichCurveTangentWeightMode.RCTWM_WeightedNone;
		Time = time;
		Value = value;
		ArriveTangent = 0f;
		ArriveTangentWeight = 0f;
		LeaveTangent = 0f;
		LeaveTangentWeight = 0f;
	}

	public FRichCurveKey(float time, float value, float arriveTangent, float leaveTangent, ERichCurveInterpMode interpMode)
	{
		InterpMode = interpMode;
		TangentMode = ERichCurveTangentMode.RCTM_Auto;
		TangentWeightMode = ERichCurveTangentWeightMode.RCTWM_WeightedNone;
		Time = time;
		Value = value;
		ArriveTangent = arriveTangent;
		ArriveTangentWeight = 0f;
		LeaveTangent = leaveTangent;
		LeaveTangentWeight = 0f;
	}
}
