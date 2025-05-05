namespace CUE4Parse.UE4.Objects.Core.Math;

public readonly struct FIntVector : IUStruct
{
	public readonly int X;

	public readonly int Y;

	public readonly int Z;

	public FIntVector(int x, int y, int z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public override string ToString()
	{
		return $"{"X"}: {X}, {"Y"}: {Y}, {"Z"}: {Z}";
	}
}
