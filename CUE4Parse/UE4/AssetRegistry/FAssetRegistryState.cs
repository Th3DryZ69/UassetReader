using CUE4Parse.UE4.AssetRegistry.Objects;
using CUE4Parse.UE4.AssetRegistry.Readers;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.AssetRegistry;

[JsonConverter(typeof(FAssetRegistryStateConverter))]
public class FAssetRegistryState
{
	public FAssetData[] PreallocatedAssetDataBuffers;

	public FDependsNode[] PreallocatedDependsNodeDataBuffers;

	public FAssetPackageData[] PreallocatedPackageDataBuffers;

	public FAssetRegistryState(FArchive Ar)
	{
		FAssetRegistryHeader fAssetRegistryHeader = new FAssetRegistryHeader(Ar);
		FAssetRegistryVersionType version = fAssetRegistryHeader.Version;
		if (version >= FAssetRegistryVersionType.AddAssetRegistryState)
		{
			if (version < FAssetRegistryVersionType.FixedTags)
			{
				FNameTableArchiveReader ar = new FNameTableArchiveReader(Ar, fAssetRegistryHeader);
				Load(ar);
			}
			else
			{
				FAssetRegistryReader ar2 = new FAssetRegistryReader(Ar, fAssetRegistryHeader);
				Load(ar2);
			}
		}
		else
		{
			Log.Warning("Cannot read registry state before {Version}", version);
		}
	}

	private void Load(FAssetRegistryArchive Ar)
	{
		PreallocatedAssetDataBuffers = Ar.ReadArray(() => new FAssetData(Ar));
		if (Ar.Header.Version < FAssetRegistryVersionType.RemovedMD5Hash)
		{
			return;
		}
		if (Ar.Header.Version < FAssetRegistryVersionType.AddedDependencyFlags)
		{
			int num = Ar.Read<int>();
			PreallocatedDependsNodeDataBuffers = new FDependsNode[num];
			for (int num2 = 0; num2 < num; num2++)
			{
				PreallocatedDependsNodeDataBuffers[num2] = new FDependsNode(num2);
			}
			if (num > 0)
			{
				LoadDependencies_BeforeFlags(Ar);
			}
		}
		else
		{
			long num3 = Ar.Read<long>();
			long position = Ar.Position + num3;
			int num4 = Ar.Read<int>();
			PreallocatedDependsNodeDataBuffers = new FDependsNode[num4];
			for (int num5 = 0; num5 < num4; num5++)
			{
				PreallocatedDependsNodeDataBuffers[num5] = new FDependsNode(num5);
			}
			if (num4 > 0)
			{
				LoadDependencies(Ar);
			}
			Ar.Position = position;
		}
		PreallocatedPackageDataBuffers = Ar.ReadArray(() => new FAssetPackageData(Ar));
	}

	private void LoadDependencies_BeforeFlags(FAssetRegistryArchive Ar)
	{
		FDependsNode[] preallocatedDependsNodeDataBuffers = PreallocatedDependsNodeDataBuffers;
		for (int i = 0; i < preallocatedDependsNodeDataBuffers.Length; i++)
		{
			preallocatedDependsNodeDataBuffers[i].SerializeLoad_BeforeFlags(Ar, PreallocatedDependsNodeDataBuffers);
		}
	}

	private void LoadDependencies(FAssetRegistryArchive Ar)
	{
		FDependsNode[] preallocatedDependsNodeDataBuffers = PreallocatedDependsNodeDataBuffers;
		for (int i = 0; i < preallocatedDependsNodeDataBuffers.Length; i++)
		{
			preallocatedDependsNodeDataBuffers[i].SerializeLoad(Ar, PreallocatedDependsNodeDataBuffers);
		}
	}
}
