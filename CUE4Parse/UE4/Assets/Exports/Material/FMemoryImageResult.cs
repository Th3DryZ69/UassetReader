using System.Collections.Generic;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMemoryImageResult
{
	public struct FMemoryImageVTablePatch
	{
		public int VTableOffset;

		public int Offset;
	}

	public class FMemoryImageVTable
	{
		public ulong TypeNameHash;

		public FMemoryImageVTablePatch[] Patches;

		public FMemoryImageVTable(FArchive Ar)
		{
			TypeNameHash = Ar.Read<ulong>();
			Patches = Ar.ReadArray<FMemoryImageVTablePatch>();
		}
	}

	public struct FMemoryImageNamePatch
	{
		public int Offset;
	}

	public class FMemoryImageName
	{
		public FName Name;

		public FMemoryImageNamePatch[] Patches;

		public FMemoryImageName(FArchive Ar)
		{
			Name = Ar.ReadFName();
			Patches = Ar.ReadArray<FMemoryImageNamePatch>();
		}

		public override string ToString()
		{
			return $"{Name}: x{Patches.Length} Patches";
		}
	}

	public FPlatformTypeLayoutParameters LayoutParameters;

	public byte[] FrozenObject;

	public FPointerTableBase PointerTable;

	public FMemoryImageVTable[] VTables;

	public FMemoryImageName[] ScriptNames;

	public FMemoryImageName[] MinimalNames;

	public FMemoryImageResult(FPointerTableBase pointerTable)
	{
		PointerTable = pointerTable;
	}

	public void LoadFromArchive(FArchive Ar)
	{
		bool flag = Ar.Versions["ShaderMap.UseNewCookedFormat"];
		LayoutParameters = (flag ? new FPlatformTypeLayoutParameters(Ar) : new FPlatformTypeLayoutParameters());
		uint length = Ar.Read<uint>();
		FrozenObject = Ar.ReadBytes((int)length);
		if (flag)
		{
			PointerTable.LoadFromArchive(Ar, bUseNewFormat: true);
		}
		int length2 = Ar.Read<int>();
		int length3 = Ar.Read<int>();
		int length4 = ((Ar.Game >= EGame.GAME_UE4_26) ? Ar.Read<int>() : 0);
		VTables = Ar.ReadArray(length2, () => new FMemoryImageVTable(Ar));
		ScriptNames = Ar.ReadArray(length3, () => new FMemoryImageName(Ar));
		MinimalNames = Ar.ReadArray(length4, () => new FMemoryImageName(Ar));
		if (!flag)
		{
			PointerTable.LoadFromArchive(Ar, bUseNewFormat: false);
		}
	}

	internal Dictionary<int, FName> GetNames()
	{
		Dictionary<int, FName> dictionary = new Dictionary<int, FName>();
		FMemoryImageName[] scriptNames = ScriptNames;
		foreach (FMemoryImageName fMemoryImageName in scriptNames)
		{
			FMemoryImageNamePatch[] patches = fMemoryImageName.Patches;
			for (int j = 0; j < patches.Length; j++)
			{
				FMemoryImageNamePatch fMemoryImageNamePatch = patches[j];
				dictionary[fMemoryImageNamePatch.Offset] = fMemoryImageName.Name;
			}
		}
		scriptNames = MinimalNames;
		foreach (FMemoryImageName fMemoryImageName2 in scriptNames)
		{
			FMemoryImageNamePatch[] patches = fMemoryImageName2.Patches;
			for (int j = 0; j < patches.Length; j++)
			{
				FMemoryImageNamePatch fMemoryImageNamePatch2 = patches[j];
				dictionary[fMemoryImageNamePatch2.Offset] = fMemoryImageName2.Name;
			}
		}
		return dictionary;
	}
}
