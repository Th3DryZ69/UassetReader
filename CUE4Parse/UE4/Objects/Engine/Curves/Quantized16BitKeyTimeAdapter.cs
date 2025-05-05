using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Engine.Curves;

internal readonly struct Quantized16BitKeyTimeAdapter : IKeyTimeAdapter
{
	public const float QuantizationScale = 1.5259022E-05f;

	public const int KeySize = 2;

	public const int RangeDataSize = 8;

	public unsafe readonly ushort* KeyTimes;

	public readonly float MinTime;

	public readonly float DeltaTime;

	public int KeyDataOffset { get; }

	public unsafe Quantized16BitKeyTimeAdapter(byte* basePtr, int keyTimesOffset, int numKeys)
	{
		int num = (keyTimesOffset + numKeys * 2).Align(4);
		KeyDataOffset = num + 8;
		float* ptr = (float*)(basePtr + num);
		KeyTimes = (ushort*)(basePtr + keyTimesOffset);
		MinTime = *ptr;
		DeltaTime = ptr[1];
	}

	public unsafe float GetTime(int keyIndex)
	{
		return (float)(int)KeyTimes[keyIndex] * 1.5259022E-05f * DeltaTime + MinTime;
	}
}
