namespace CUE4Parse.UE4.Objects.Engine.Curves;

internal interface IKeyTimeAdapter
{
	int KeyDataOffset { get; }

	float GetTime(int keyIndex);
}
