using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Exceptions;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(FByteBulkDataConverter))]
public class FByteBulkData
{
	public readonly FByteBulkDataHeader Header;

	public readonly EBulkDataFlags BulkDataFlags;

	public readonly byte[]? Data;

	public FByteBulkData(FAssetArchive Ar)
	{
		Header = new FByteBulkDataHeader(Ar);
		BulkDataFlags = Header.BulkDataFlags;
		if (Header.ElementCount == 0)
		{
			return;
		}
		if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_Unused))
		{
			Log.Warning("Bulk with no data");
		}
		else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_ForceInlinePayload))
		{
			Data = new byte[Header.ElementCount];
			Ar.Read(Data, 0, Header.ElementCount);
		}
		else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_OptionalPayload))
		{
			if (Ar.TryGetPayload(PayloadType.UPTNL, out FAssetArchive ar) && ar != null)
			{
				Data = new byte[Header.ElementCount];
				ar.Position = Header.OffsetInFile;
				ar.Read(Data, 0, Header.ElementCount);
			}
		}
		else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_PayloadInSeperateFile))
		{
			if (Ar.TryGetPayload(PayloadType.UBULK, out FAssetArchive ar2) && ar2 != null)
			{
				Data = new byte[Header.ElementCount];
				ar2.Position = Header.OffsetInFile;
				ar2.Read(Data, 0, Header.ElementCount);
			}
		}
		else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_PayloadAtEndOfFile))
		{
			long position = Ar.Position;
			if (Header.OffsetInFile + Header.ElementCount > Ar.Length)
			{
				throw new ParserException(Ar, $"Failed to read PayloadAtEndOfFile, {Header.OffsetInFile} is out of range");
			}
			Data = new byte[Header.ElementCount];
			Ar.Position = Header.OffsetInFile;
			Ar.Read(Data, 0, Header.ElementCount);
			Ar.Position = position;
		}
		else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_SerializeCompressedZLIB))
		{
			throw new ParserException(Ar, "TODO: CompressedZlib");
		}
	}

	protected FByteBulkData(FAssetArchive Ar, bool skip = false)
	{
		Header = new FByteBulkDataHeader(Ar);
		EBulkDataFlags bulkDataFlags = Header.BulkDataFlags;
		if (!bulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_PayloadAtEndOfFile | EBulkDataFlags.BULKDATA_Unused | EBulkDataFlags.BULKDATA_PayloadInSeperateFile) && (bulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_ForceInlinePayload) || Header.OffsetInFile == Ar.Position))
		{
			Ar.Position += Header.SizeOnDisk;
		}
	}
}
