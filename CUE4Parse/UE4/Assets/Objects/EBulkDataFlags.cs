using System;

namespace CUE4Parse.UE4.Assets.Objects;

[Flags]
public enum EBulkDataFlags : uint
{
	BULKDATA_None = 0u,
	BULKDATA_PayloadAtEndOfFile = 1u,
	BULKDATA_SerializeCompressedZLIB = 2u,
	BULKDATA_ForceSingleElementSerialization = 4u,
	BULKDATA_SingleUse = 8u,
	BULKDATA_Unused = 0x20u,
	BULKDATA_ForceInlinePayload = 0x40u,
	BULKDATA_SerializeCompressed = 2u,
	BULKDATA_ForceStreamPayload = 0x80u,
	BULKDATA_PayloadInSeperateFile = 0x100u,
	BULKDATA_SerializeCompressedBitWindow = 0x200u,
	BULKDATA_Force_NOT_InlinePayload = 0x400u,
	BULKDATA_OptionalPayload = 0x800u,
	BULKDATA_MemoryMappedPayload = 0x1000u,
	BULKDATA_Size64Bit = 0x2000u,
	BULKDATA_DuplicateNonOptionalPayload = 0x4000u,
	BULKDATA_BadDataVersion = 0x8000u,
	BULKDATA_NoOffsetFixUp = 0x10000u,
	BULKDATA_WorkspaceDomainPayload = 0x20000u,
	BULKDATA_LazyLoadable = 0x40000u,
	BULKDATA_DataIsMemoryMapped = 0x40000000u,
	BULKDATA_HasAsyncReadPending = 0x20000000u,
	BULKDATA_AlwaysAllowDiscard = 0x10000000u
}
