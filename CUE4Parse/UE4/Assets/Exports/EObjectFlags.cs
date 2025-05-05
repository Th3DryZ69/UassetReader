using System;

namespace CUE4Parse.UE4.Assets.Exports;

[Flags]
public enum EObjectFlags
{
	RF_NoFlags = 0,
	RF_Public = 1,
	RF_Standalone = 2,
	RF_MarkAsNative = 4,
	RF_Transactional = 8,
	RF_ClassDefaultObject = 0x10,
	RF_ArchetypeObject = 0x20,
	RF_Transient = 0x40,
	RF_MarkAsRootSet = 0x80,
	RF_TagGarbageTemp = 0x100,
	RF_NeedInitialization = 0x200,
	RF_NeedLoad = 0x400,
	RF_KeepForCooker = 0x800,
	RF_NeedPostLoad = 0x1000,
	RF_NeedPostLoadSubobjects = 0x2000,
	RF_NewerVersionExists = 0x4000,
	RF_BeginDestroyed = 0x8000,
	RF_FinishDestroyed = 0x10000,
	RF_BeingRegenerated = 0x20000,
	RF_DefaultSubObject = 0x40000,
	RF_WasLoaded = 0x80000,
	RF_TextExportTransient = 0x100000,
	RF_LoadCompleted = 0x200000,
	RF_InheritableComponentTemplate = 0x400000,
	RF_DuplicateTransient = 0x800000,
	RF_StrongRefOnFrame = 0x1000000,
	RF_NonPIEDuplicateTransient = 0x2000000,
	RF_Dynamic = 0x4000000,
	RF_WillBeLoaded = 0x8000000
}
