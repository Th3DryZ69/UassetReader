namespace CUE4Parse.UE4.Kismet;

public enum ECastToken : byte
{
	CST_ObjectToInterface = 0,
	CST_ObjectToBool = 1,
	CST_InterfaceToBool = 2,
	CST_DoubleToFloat = 3,
	CST_FloatToDouble = 4,
	CST_ObjectToBool2 = 71,
	CST_InterfaceToBool2 = 73,
	CST_Max = byte.MaxValue
}
