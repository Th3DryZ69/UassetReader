using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(FByteBulkDataHeaderConverter))]
public readonly struct FByteBulkDataHeader
{
	public readonly EBulkDataFlags BulkDataFlags;

	public readonly int ElementCount;

	public readonly uint SizeOnDisk;

	public readonly long OffsetInFile;

	public FByteBulkDataHeader(FAssetArchive Ar)
	{
		if (Ar.Owner is IoPackage ioPackage)
		{
			FBulkDataMapEntry[] bulkDataMap = ioPackage.BulkDataMap;
			if (bulkDataMap != null && bulkDataMap.Length > 0)
			{
				int num = Ar.Read<int>();
				if (num >= 0 && num < ioPackage.BulkDataMap.Length)
				{
					FBulkDataMapEntry fBulkDataMapEntry = ioPackage.BulkDataMap[num];
					BulkDataFlags = (EBulkDataFlags)fBulkDataMapEntry.Flags;
					ElementCount = (int)fBulkDataMapEntry.SerialSize;
					OffsetInFile = (long)fBulkDataMapEntry.SerialOffset;
					SizeOnDisk = (uint)fBulkDataMapEntry.SerialSize;
					return;
				}
				Ar.Position -= 4L;
			}
		}
		BulkDataFlags = Ar.Read<EBulkDataFlags>();
		ElementCount = (int)(BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_Size64Bit) ? Ar.Read<long>() : Ar.Read<int>());
		SizeOnDisk = (uint)(BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_Size64Bit) ? Ar.Read<long>() : Ar.Read<uint>());
		OffsetInFile = ((Ar.Ver >= EUnrealEngineObjectUE4Version.BULKDATA_AT_LARGE_OFFSETS) ? Ar.Read<long>() : Ar.Read<int>());
		if (!BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_NoOffsetFixUp))
		{
			OffsetInFile += Ar.Owner.Summary.BulkDataStartOffset;
		}
		if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_BadDataVersion))
		{
			Ar.Position += 2L;
			BulkDataFlags &= ~EBulkDataFlags.BULKDATA_BadDataVersion;
		}
		if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_DuplicateNonOptionalPayload))
		{
			Ar.Position += 4L;
			Ar.Position += (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_Size64Bit) ? 8 : 4);
			Ar.Position += ((Ar.Ver >= EUnrealEngineObjectUE4Version.BULKDATA_AT_LARGE_OFFSETS) ? 8 : 4);
		}
	}
}
