using System;
using CUE4Parse.UE4.Objects.Engine.Animation;
using CUE4Parse.UE4.Objects.Engine.Curves;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class UAnimCurveCompressionCodec_CompressedRichCurve : UAnimCurveCompressionCodec
{
	private unsafe delegate FRichCurve CompressedCurveConverter(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int constantValueNumKeys, byte* compressedKeys);

	private unsafe static readonly CompressedCurveConverter[][] ConverterMap = new CompressedCurveConverter[6][]
	{
		new CompressedCurveConverter[2]
		{
			(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int constantValue, byte* _) => new FRichCurve
			{
				DefaultValue = *(float*)(&constantValue),
				PreInfinityExtrap = preInfinityExtrap,
				PostInfinityExtrap = postInfinityExtrap,
				Keys = Array.Empty<FRichCurveKey>()
			},
			(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int constantValue, byte* _) => new FRichCurve
			{
				DefaultValue = *(float*)(&constantValue),
				PreInfinityExtrap = preInfinityExtrap,
				PostInfinityExtrap = postInfinityExtrap,
				Keys = Array.Empty<FRichCurveKey>()
			}
		},
		new CompressedCurveConverter[2]
		{
			(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int constantValue, byte* _) => new FRichCurve
			{
				DefaultValue = float.MaxValue,
				PreInfinityExtrap = preInfinityExtrap,
				PostInfinityExtrap = postInfinityExtrap,
				Keys = new FRichCurveKey[1]
				{
					new FRichCurveKey(0f, *(float*)(&constantValue))
				}
			},
			(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int constantValue, byte* _) => new FRichCurve
			{
				DefaultValue = float.MaxValue,
				PreInfinityExtrap = preInfinityExtrap,
				PostInfinityExtrap = postInfinityExtrap,
				Keys = new FRichCurveKey[1]
				{
					new FRichCurveKey(0f, *(float*)(&constantValue))
				}
			}
		},
		new CompressedCurveConverter[2]
		{
			delegate(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int numKeys, byte* compressedKeys)
			{
				int keyTimesOffset = 0;
				Quantized16BitKeyTimeAdapter quantized16BitKeyTimeAdapter = new Quantized16BitKeyTimeAdapter(compressedKeys, keyTimesOffset, numKeys);
				return ConvertToRaw(keyDataAdapter: new UniformKeyDataAdapter(ERichCurveCompressionFormat.RCCF_Linear, compressedKeys, quantized16BitKeyTimeAdapter), keyTimeAdapter: quantized16BitKeyTimeAdapter, numKeys: numKeys, preInfinityExtrap: preInfinityExtrap, postInfinityExtrap: postInfinityExtrap);
			},
			delegate(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int numKeys, byte* compressedKeys)
			{
				int keyTimesOffset = 0;
				Float32BitKeyTimeAdapter float32BitKeyTimeAdapter = new Float32BitKeyTimeAdapter(compressedKeys, keyTimesOffset, numKeys);
				return ConvertToRaw(keyDataAdapter: new UniformKeyDataAdapter(ERichCurveCompressionFormat.RCCF_Linear, compressedKeys, float32BitKeyTimeAdapter), keyTimeAdapter: float32BitKeyTimeAdapter, numKeys: numKeys, preInfinityExtrap: preInfinityExtrap, postInfinityExtrap: postInfinityExtrap);
			}
		},
		new CompressedCurveConverter[2]
		{
			delegate(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int numKeys, byte* compressedKeys)
			{
				int keyTimesOffset = 0;
				Quantized16BitKeyTimeAdapter quantized16BitKeyTimeAdapter = new Quantized16BitKeyTimeAdapter(compressedKeys, keyTimesOffset, numKeys);
				return ConvertToRaw(keyDataAdapter: new UniformKeyDataAdapter(ERichCurveCompressionFormat.RCCF_Cubic, compressedKeys, quantized16BitKeyTimeAdapter), keyTimeAdapter: quantized16BitKeyTimeAdapter, numKeys: numKeys, preInfinityExtrap: preInfinityExtrap, postInfinityExtrap: postInfinityExtrap);
			},
			delegate(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int numKeys, byte* compressedKeys)
			{
				int keyTimesOffset = 0;
				Float32BitKeyTimeAdapter float32BitKeyTimeAdapter = new Float32BitKeyTimeAdapter(compressedKeys, keyTimesOffset, numKeys);
				return ConvertToRaw(keyDataAdapter: new UniformKeyDataAdapter(ERichCurveCompressionFormat.RCCF_Cubic, compressedKeys, float32BitKeyTimeAdapter), keyTimeAdapter: float32BitKeyTimeAdapter, numKeys: numKeys, preInfinityExtrap: preInfinityExtrap, postInfinityExtrap: postInfinityExtrap);
			}
		},
		new CompressedCurveConverter[2]
		{
			delegate(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int numKeys, byte* compressedKeys)
			{
				int num = 0;
				int keyTimesOffset = num + numKeys.Align(2);
				Quantized16BitKeyTimeAdapter quantized16BitKeyTimeAdapter = new Quantized16BitKeyTimeAdapter(compressedKeys, keyTimesOffset, numKeys);
				return ConvertToRaw(keyDataAdapter: new MixedKeyDataAdapter(compressedKeys, num, quantized16BitKeyTimeAdapter), keyTimeAdapter: quantized16BitKeyTimeAdapter, numKeys: numKeys, preInfinityExtrap: preInfinityExtrap, postInfinityExtrap: postInfinityExtrap);
			},
			delegate(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int numKeys, byte* compressedKeys)
			{
				int num = 0;
				int keyTimesOffset = num + numKeys.Align(4);
				Float32BitKeyTimeAdapter float32BitKeyTimeAdapter = new Float32BitKeyTimeAdapter(compressedKeys, keyTimesOffset, numKeys);
				return ConvertToRaw(keyDataAdapter: new MixedKeyDataAdapter(compressedKeys, num, float32BitKeyTimeAdapter), keyTimeAdapter: float32BitKeyTimeAdapter, numKeys: numKeys, preInfinityExtrap: preInfinityExtrap, postInfinityExtrap: postInfinityExtrap);
			}
		},
		new CompressedCurveConverter[2]
		{
			delegate(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int numKeys, byte* compressedKeys)
			{
				int num = 0;
				int keyTimesOffset = num + (2 * numKeys).Align(2);
				Quantized16BitKeyTimeAdapter quantized16BitKeyTimeAdapter = new Quantized16BitKeyTimeAdapter(compressedKeys, keyTimesOffset, numKeys);
				return ConvertToRaw(keyDataAdapter: new WeightedKeyDataAdapter(compressedKeys, num, quantized16BitKeyTimeAdapter), keyTimeAdapter: quantized16BitKeyTimeAdapter, numKeys: numKeys, preInfinityExtrap: preInfinityExtrap, postInfinityExtrap: postInfinityExtrap);
			},
			delegate(ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap, int numKeys, byte* compressedKeys)
			{
				int num = 0;
				int keyTimesOffset = num + (2 * numKeys).Align(4);
				Float32BitKeyTimeAdapter float32BitKeyTimeAdapter = new Float32BitKeyTimeAdapter(compressedKeys, keyTimesOffset, numKeys);
				return ConvertToRaw(keyDataAdapter: new WeightedKeyDataAdapter(compressedKeys, num, float32BitKeyTimeAdapter), keyTimeAdapter: float32BitKeyTimeAdapter, numKeys: numKeys, preInfinityExtrap: preInfinityExtrap, postInfinityExtrap: postInfinityExtrap);
			}
		}
	};

	private static FRichCurve ConvertToRaw(IKeyTimeAdapter keyTimeAdapter, IKeyDataAdapter keyDataAdapter, int numKeys, ERichCurveExtrapolation preInfinityExtrap, ERichCurveExtrapolation postInfinityExtrap)
	{
		FRichCurve fRichCurve = new FRichCurve();
		fRichCurve.DefaultValue = float.MaxValue;
		fRichCurve.PreInfinityExtrap = preInfinityExtrap;
		fRichCurve.PostInfinityExtrap = postInfinityExtrap;
		fRichCurve.Keys = new FRichCurveKey[numKeys];
		for (int i = 0; i < numKeys; i++)
		{
			int keyDataHandle = keyDataAdapter.GetKeyDataHandle(i);
			ERichCurveCompressionFormat keyInterpMode = keyDataAdapter.GetKeyInterpMode(i);
			FRichCurveKey fRichCurveKey = new FRichCurveKey
			{
				InterpMode = keyInterpMode switch
				{
					ERichCurveCompressionFormat.RCCF_Linear => ERichCurveInterpMode.RCIM_Linear, 
					ERichCurveCompressionFormat.RCCF_Cubic => ERichCurveInterpMode.RCIM_Cubic, 
					ERichCurveCompressionFormat.RCCF_Constant => ERichCurveInterpMode.RCIM_Constant, 
					_ => throw new ArgumentException("Can't convert interpMode " + keyInterpMode.ToString() + " to ERichCurveInterpMode"), 
				},
				TangentMode = ERichCurveTangentMode.RCTM_Auto,
				TangentWeightMode = keyDataAdapter.GetKeyTangentWeightMode(i),
				Time = keyTimeAdapter.GetTime(i),
				Value = keyDataAdapter.GetKeyValue(keyDataHandle),
				ArriveTangent = keyDataAdapter.GetKeyArriveTangent(keyDataHandle),
				ArriveTangentWeight = keyDataAdapter.GetKeyArriveTangentWeight(keyDataHandle),
				LeaveTangent = keyDataAdapter.GetKeyLeaveTangent(keyDataHandle),
				LeaveTangentWeight = keyDataAdapter.GetKeyLeaveTangentWeight(keyDataHandle)
			};
			fRichCurve.Keys[i] = fRichCurveKey;
		}
		return fRichCurve;
	}

	public unsafe override FFloatCurve[] ConvertCurves(UAnimSequence animSeq)
	{
		if (animSeq.CompressedCurveByteStream == null || animSeq.CompressedCurveByteStream.Length == 0)
		{
			return Array.Empty<FFloatCurve>();
		}
		fixed (byte* ptr = &animSeq.CompressedCurveByteStream[0])
		{
			FCurveDesc* ptr2 = (FCurveDesc*)ptr;
			FSmartName[] compressedCurveNames = animSeq.CompressedCurveNames;
			int num = compressedCurveNames.Length;
			FFloatCurve[] array = new FFloatCurve[num];
			for (int i = 0; i < num; i++)
			{
				FSmartName name = compressedCurveNames[i];
				FCurveDesc fCurveDesc = ptr2[i];
				byte* compressedKeys = ptr + fCurveDesc.KeyDataOffset;
				FRichCurve floatCurve = ConverterMap[(uint)fCurveDesc.CompressionFormat][(uint)fCurveDesc.KeyTimeCompressionFormat](fCurveDesc.PreInfinityExtrap, fCurveDesc.PostInfinityExtrap, fCurveDesc.NumKeys, compressedKeys);
				array[i] = new FFloatCurve
				{
					Name = name,
					FloatCurve = floatCurve,
					CurveTypeFlags = 4
				};
			}
			return array;
		}
	}
}
