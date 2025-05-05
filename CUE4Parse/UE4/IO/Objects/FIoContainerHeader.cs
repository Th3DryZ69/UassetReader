using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.IO.Objects;

public class FIoContainerHeader
{
	private const int Signature = 1232028526;

	public FIoContainerId ContainerId;

	public FPackageId[] PackageIds;

	public FFilePackageStoreEntry[] StoreEntries;

	public FFilePackageStoreEntry[] OptionalSegmentStoreEntries;

	public FPackageId[] OptionalSegmentPackageIds;

	public FNameEntrySerialized[]? ContainerNameMap;

	public FIoContainerHeader(FArchive Ar)
	{
		EIoContainerHeaderVersion eIoContainerHeaderVersion = ((Ar.Game < EGame.GAME_UE5_0) ? EIoContainerHeaderVersion.BeforeVersionWasAdded : EIoContainerHeaderVersion.Initial);
		if (eIoContainerHeaderVersion == EIoContainerHeaderVersion.Initial)
		{
			uint num = Ar.Read<uint>();
			if (num != 1232028526)
			{
				throw new ParserException(Ar, $"Invalid container header signature: 0x{num:X8} != 0x{1232028526:X8}");
			}
			eIoContainerHeaderVersion = Ar.Read<EIoContainerHeaderVersion>();
		}
		ContainerId = Ar.Read<FIoContainerId>();
		if (eIoContainerHeaderVersion < EIoContainerHeaderVersion.OptionalSegmentPackages)
		{
			Ar.Read<uint>();
		}
		if (eIoContainerHeaderVersion == EIoContainerHeaderVersion.BeforeVersionWasAdded)
		{
			Ar.Read<int>();
			long position = Ar.Position;
			int num2 = Ar.Read<int>();
			long position2 = Ar.Position + num2;
			Ar.Position = position;
			ContainerNameMap = FNameEntrySerialized.LoadNameBatch(Ar, num2 / 8 - 1);
			Ar.Position = position2;
		}
		ReadPackageIdsAndEntries(Ar, out PackageIds, out StoreEntries);
		if (eIoContainerHeaderVersion >= EIoContainerHeaderVersion.OptionalSegmentPackages)
		{
			ReadPackageIdsAndEntries(Ar, out OptionalSegmentPackageIds, out OptionalSegmentStoreEntries);
		}
		if (eIoContainerHeaderVersion >= EIoContainerHeaderVersion.Initial)
		{
			ContainerNameMap = FNameEntrySerialized.LoadNameBatch(Ar);
		}
	}

	private void ReadPackageIdsAndEntries(FArchive Ar, out FPackageId[] packageIds, out FFilePackageStoreEntry[] storeEntries)
	{
		packageIds = Ar.ReadArray<FPackageId>();
		int num = Ar.Read<int>();
		long position = Ar.Position + num;
		storeEntries = Ar.ReadArray(packageIds.Length, () => new FFilePackageStoreEntry(Ar));
		Ar.Position = position;
	}
}
