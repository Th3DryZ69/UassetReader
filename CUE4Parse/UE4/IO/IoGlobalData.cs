using System.Collections.Generic;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.IO;

public class IoGlobalData
{
	public readonly FNameEntrySerialized[] GlobalNameMap;

	public readonly Dictionary<FPackageObjectIndex, FScriptObjectEntry> ScriptObjectEntriesMap = new Dictionary<FPackageObjectIndex, FScriptObjectEntry>();

	public IoGlobalData(IoStoreReader globalReader)
	{
		FByteArchive fByteArchive;
		if (globalReader.Game >= EGame.GAME_UE5_0)
		{
			fByteArchive = new FByteArchive("ScriptObjects", globalReader.Read(new FIoChunkId(0uL, 0, EIoChunkType5.ScriptObjects)));
			GlobalNameMap = FNameEntrySerialized.LoadNameBatch(fByteArchive);
		}
		else
		{
			if (!globalReader.TryResolve(new FIoChunkId(0uL, 0, EIoChunkType.LoaderGlobalNameHashes), out var outOffsetLength))
			{
				throw new KeyNotFoundException("Couldn't find LoaderGlobalNameHashes chunk in IoStore " + globalReader.Name);
			}
			int nameCount = (int)(outOffsetLength.Length / 8 - 1);
			FByteArchive nameAr = new FByteArchive("LoaderGlobalNames", globalReader.Read(new FIoChunkId(0uL, 0, EIoChunkType.LoaderGlobalNames)));
			GlobalNameMap = FNameEntrySerialized.LoadNameBatch(nameAr, nameCount);
			fByteArchive = new FByteArchive("LoaderInitialLoadMeta", globalReader.Read(new FIoChunkId(0uL, 0, EIoChunkType.LoaderInitialLoadMeta)));
		}
		int length = fByteArchive.Read<int>();
		FScriptObjectEntry[] array = fByteArchive.ReadArray<FScriptObjectEntry>(length);
		for (int i = 0; i < array.Length; i++)
		{
			FScriptObjectEntry value = array[i];
			ScriptObjectEntriesMap[value.GlobalIndex] = value;
		}
	}
}
