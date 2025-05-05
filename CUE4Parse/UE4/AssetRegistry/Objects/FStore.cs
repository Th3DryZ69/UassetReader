using System.Text;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

public class FStore
{
	private const uint _OLD_BEGIN_MAGIC = 305419896u;

	private const uint _BEGIN_MAGIC = 305419897u;

	private const uint _END_MAGIC = 2271560481u;

	public readonly FNumberedPair[] Pairs;

	public readonly FNumberlessPair[] NumberlessPairs;

	public readonly uint[] AnsiStringOffsets;

	public readonly byte[] AnsiStrings;

	public readonly uint[] WideStringOffsets;

	public readonly byte[] WideStrings;

	public readonly uint[] NumberlessNames;

	public readonly FName[] Names;

	public readonly FNumberlessExportPath[] NumberlessExportPaths;

	public readonly FAssetRegistryExportPath[] ExportPaths;

	public readonly string[] Texts;

	public readonly FNameEntrySerialized[] NameMap;

	public FStore(FAssetRegistryReader Ar)
	{
		NameMap = Ar.NameMap;
		uint magic = Ar.Read<uint>();
		ELoadOrder loadOrder = GetLoadOrder(magic);
		int[] array = Ar.ReadArray<int>(11);
		if (loadOrder == ELoadOrder.TextFirst)
		{
			Ar.Position += 4L;
			Texts = Ar.ReadArray(array[4], Ar.ReadFString);
		}
		NumberlessNames = Ar.ReadArray(array[0], Ar.Read<uint>);
		Names = Ar.ReadArray(array[1], Ar.ReadFName);
		NumberlessExportPaths = Ar.ReadArray(array[2], () => new FNumberlessExportPath(Ar));
		ExportPaths = Ar.ReadArray(array[3], () => new FAssetRegistryExportPath(Ar));
		if (loadOrder == ELoadOrder.Member)
		{
			Texts = Ar.ReadArray(array[4], Ar.ReadFString);
		}
		AnsiStringOffsets = Ar.ReadArray(array[5], Ar.Read<uint>);
		WideStringOffsets = Ar.ReadArray(array[6], Ar.Read<uint>);
		AnsiStrings = Ar.ReadBytes(array[7]);
		WideStrings = Ar.ReadBytes(array[8] * 2);
		NumberlessPairs = Ar.ReadArray(array[9], () => new FNumberlessPair(Ar));
		Pairs = Ar.ReadArray(array[10], () => new FNumberedPair(Ar));
		Ar.Position += 4L;
	}

	public string GetAnsiString(int index)
	{
		uint num = AnsiStringOffsets[index];
		int i;
		for (i = 0; AnsiStrings[num + i] != 0; i++)
		{
		}
		return Encoding.UTF8.GetString(AnsiStrings, (int)num, i);
	}

	public string GetWideString(int index)
	{
		uint num = WideStringOffsets[index];
		int i;
		for (i = 0; WideStrings[num + i] != 0 && WideStrings[num + i + 1] != 0; i += 2)
		{
		}
		return Encoding.Unicode.GetString(WideStrings, (int)num, i);
	}

	private ELoadOrder GetLoadOrder(uint magic)
	{
		return magic switch
		{
			305419896u => ELoadOrder.Member, 
			305419897u => ELoadOrder.TextFirst, 
			_ => throw new ParserException("Asset registry has bad magic number"), 
		};
	}
}
