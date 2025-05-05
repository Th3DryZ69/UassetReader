namespace CUE4Parse.UE4.Objects.Engine;

public enum EBspNodeFlags : byte
{
	NF_NotCsg = 1,
	NF_NotVisBlocking = 4,
	NF_BrightCorners = 0x10,
	NF_IsNew = 0x20,
	NF_IsFront = 0x40,
	NF_IsBack = 0x80
}
