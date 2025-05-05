using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Engine.Curves;

internal readonly struct Float32BitKeyTimeAdapter : IKeyTimeAdapter
{
	public const int KeySize = 4;

	public const int RangeDataSize = 0;

	public unsafe readonly float* KeyTimes;

	public int KeyDataOffset { get; }

	public unsafe Float32BitKeyTimeAdapter(byte* basePtr, int keyTimesOffset, int numKeys)
	{
		KeyTimes = (float*)(basePtr + keyTimesOffset);
		KeyDataOffset = (keyTimesOffset + numKeys * 4).Align(4);
	}

	public unsafe float GetTime(int keyIndex)
	{
		return KeyTimes[keyIndex];
	}
}
