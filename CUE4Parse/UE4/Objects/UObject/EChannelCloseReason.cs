namespace CUE4Parse.UE4.Objects.UObject;

public enum EChannelCloseReason : byte
{
	Destroyed = 0,
	Dormancy = 1,
	LevelUnloaded = 2,
	Relevancy = 3,
	TearOff = 4,
	MAX = 15
}
