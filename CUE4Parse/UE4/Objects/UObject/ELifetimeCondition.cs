namespace CUE4Parse.UE4.Objects.UObject;

public enum ELifetimeCondition
{
	COND_None,
	COND_InitialOnly,
	COND_OwnerOnly,
	COND_SkipOwner,
	COND_SimulatedOnly,
	COND_AutonomousOnly,
	COND_SimulatedOrPhysics,
	COND_InitialOrOwner,
	COND_Custom,
	COND_ReplayOrOwner,
	COND_ReplayOnly,
	COND_SimulatedOnlyNoReplay,
	COND_SimulatedOrPhysicsNoReplay,
	COND_SkipReplay,
	COND_Max
}
