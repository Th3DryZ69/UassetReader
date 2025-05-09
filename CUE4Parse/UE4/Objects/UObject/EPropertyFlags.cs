using System;

namespace CUE4Parse.UE4.Objects.UObject;

[Flags]
public enum EPropertyFlags : ulong
{
	None = 0uL,
	Edit = 1uL,
	ConstParm = 2uL,
	BlueprintVisible = 4uL,
	ExportObject = 8uL,
	BlueprintReadOnly = 0x10uL,
	Net = 0x20uL,
	EditFixedSize = 0x40uL,
	Parm = 0x80uL,
	OutParm = 0x100uL,
	ZeroConstructor = 0x200uL,
	ReturnParm = 0x400uL,
	DisableEditOnTemplate = 0x800uL,
	NonNullable = 0x1000uL,
	Transient = 0x2000uL,
	Config = 0x4000uL,
	RequiredParm = 0x8000uL,
	DisableEditOnInstance = 0x10000uL,
	EditConst = 0x20000uL,
	GlobalConfig = 0x40000uL,
	InstancedReference = 0x80000uL,
	DuplicateTransient = 0x200000uL,
	SaveGame = 0x1000000uL,
	NoClear = 0x2000000uL,
	ReferenceParm = 0x8000000uL,
	BlueprintAssignable = 0x10000000uL,
	Deprecated = 0x20000000uL,
	IsPlainOldData = 0x40000000uL,
	RepSkip = 0x80000000uL,
	RepNotify = 0x100000000uL,
	Interp = 0x200000000uL,
	NonTransactional = 0x400000000uL,
	EditorOnly = 0x800000000uL,
	NoDestructor = 0x1000000000uL,
	AutoWeak = 0x4000000000uL,
	ContainsInstancedReference = 0x8000000000uL,
	AssetRegistrySearchable = 0x10000000000uL,
	SimpleDisplay = 0x20000000000uL,
	AdvancedDisplay = 0x40000000000uL,
	Protected = 0x80000000000uL,
	BlueprintCallable = 0x100000000000uL,
	BlueprintAuthorityOnly = 0x200000000000uL,
	TextExportTransient = 0x400000000000uL,
	NonPIEDuplicateTransient = 0x800000000000uL,
	ExposeOnSpawn = 0x1000000000000uL,
	PersistentInstance = 0x2000000000000uL,
	UObjectWrapper = 0x4000000000000uL,
	HasGetValueTypeHash = 0x8000000000000uL,
	NativeAccessSpecifierPublic = 0x10000000000000uL,
	NativeAccessSpecifierProtected = 0x20000000000000uL,
	NativeAccessSpecifierPrivate = 0x40000000000000uL,
	SkipSerialization = 0x80000000000000uL,
	NativeAccessSpecifiers = 0x70000000000000uL,
	ParmFlags = 0x8008582uL,
	PropagateToArrayInner = 0x600C8200A4008uL,
	PropagateToMapValue = 0x600C8200A4009uL,
	PropagateToMapKey = 0x600C8200A4009uL,
	PropagateToSetElement = 0x600C8200A4009uL,
	InterfaceClearMask = 0x8000080008uL,
	DevelopmentAssets = 0x800000000uL,
	ComputedFlags = 0x8001040000200uL,
	AllFlags = ulong.MaxValue
}
