using System;

namespace CUE4Parse.UE4.Objects.UObject;

[Flags]
public enum EPackageFlags : uint
{
	PKG_None = 0u,
	PKG_NewlyCreated = 1u,
	PKG_ClientOptional = 2u,
	PKG_ServerSideOnly = 4u,
	PKG_CompiledIn = 0x10u,
	PKG_ForDiffing = 0x20u,
	PKG_EditorOnly = 0x40u,
	PKG_Developer = 0x80u,
	PKG_UncookedOnly = 0x100u,
	PKG_Cooked = 0x200u,
	PKG_ContainsNoAsset = 0x400u,
	PKG_UnversionedProperties = 0x2000u,
	PKG_ContainsMapData = 0x4000u,
	PKG_Compiling = 0x10000u,
	PKG_ContainsMap = 0x20000u,
	PKG_RequiresLocalizationGather = 0x40000u,
	PKG_PlayInEditor = 0x100000u,
	PKG_ContainsScript = 0x200000u,
	PKG_DisallowExport = 0x400000u,
	PKG_DynamicImports = 0x10000000u,
	PKG_RuntimeGenerated = 0x20000000u,
	PKG_ReloadingForCooker = 0x40000000u,
	PKG_FilterEditorOnly = 0x80000000u
}
