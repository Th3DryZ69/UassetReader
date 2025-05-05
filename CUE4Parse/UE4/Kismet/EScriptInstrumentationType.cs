namespace CUE4Parse.UE4.Kismet;

public enum EScriptInstrumentationType : byte
{
	Class,
	ClassScope,
	Instance,
	Event,
	InlineEvent,
	ResumeEvent,
	PureNodeEntry,
	NodeDebugSite,
	NodeEntry,
	NodeExit,
	PushState,
	RestoreState,
	ResetState,
	SuspendState,
	PopState,
	TunnelEndOfThread,
	Stop
}
