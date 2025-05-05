namespace CUE4Parse.UE4.Assets.Exports.Material;

public enum EMaterialParameterType : byte
{
	Scalar = 0,
	Vector = 1,
	DoubleVector = 2,
	Texture = 3,
	Font = 4,
	RuntimeVirtualTexture = 5,
	NumRuntime = 6,
	StaticSwitch = 6,
	StaticComponentMask = 7,
	Num = 8,
	None = byte.MaxValue
}
