using System;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Objects.Engine.Curves;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

[JsonConverter(typeof(FCurveDescConverter))]
public struct FCurveDesc
{
	public ERichCurveCompressionFormat CompressionFormat;

	public ERichCurveKeyTimeCompressionFormat KeyTimeCompressionFormat;

	public ERichCurveExtrapolation PreInfinityExtrap;

	public ERichCurveExtrapolation PostInfinityExtrap;

	public int NumKeys;

	public int KeyDataOffset;

	public float ConstantValue
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return BitConverter.Int32BitsToSingle(NumKeys);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			NumKeys = BitConverter.SingleToInt32Bits(value);
		}
	}
}
