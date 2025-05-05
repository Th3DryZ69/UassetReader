using System;
using System.Linq;
using System.Text;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Objects.UObject;
using Uasset_Reader.Workspace.Swapping.Structs;
using Uasset_Reader.Workspace.Swapping.Utilities;

namespace Uasset_Reader.Workspace.Swapping.Sterilization;

public class Deserializer
{
	public Uasset_Reader.Workspace.Swapping.Structs.FZenPackageSummary Summary;

	public FPackageObjectIndex[] ImportMap;

	public FExportMapEntry[] ExportMap;

	public string[] NameMap;

	public ulong[] NameMapHashes;

	public byte[] RestOfData;

	public int ExtraOfDataSize;

	public byte[] ExtraOfData;

	public ulong HashVersion;

	public void Deserialize(byte[] Asset)
	{
		Reader reader = new Reader(Asset);
		reader.Position = 0L;
		Summary = reader.Read<Uasset_Reader.Workspace.Swapping.Structs.FZenPackageSummary>();
		NameMap = DeserializeNameBatch(reader);
		if (Summary.ImportedPublicExportHashesOffset - reader.Position > 0)
		{
			ExtraOfDataSize = (int)(Summary.ImportedPublicExportHashesOffset - reader.Position);
			ExtraOfData = reader.ReadBytes(ExtraOfDataSize);
		}
		reader.Position = Summary.ImportedPublicExportHashesOffset;
		RestOfData = reader.ReadBytes((int)(reader.Length - reader.Position));
		reader.Position = Summary.ImportMapOffset;
		ImportMap = reader.ReadArray<FPackageObjectIndex>((Summary.ExportMapOffset - Summary.ImportMapOffset) / 8);
		reader.Position = Summary.ExportMapOffset;
		ExportMap = new FExportMapEntry[(Summary.ExportBundleEntriesOffset - Summary.ExportMapOffset) / 72];
		DeserializeExportMap(reader);
	}

	public string[] DeserializeNameBatch(Reader Reader)
	{
		int num = Reader.Read<int>();
		ulong[] array;
		if (num == 0)
		{
			array = Array.Empty<ulong>();
			return Array.Empty<string>();
		}
		Reader.Position += 4L;
		HashVersion = Reader.Read<ulong>();
		array = Reader.ReadArray<ulong>(num);
		uint[] array2 = new uint[num];
		string[] array3 = new string[num];
		array2 = Reader.ReadArray<FSerializedNameHeader>(num).Select(delegate(FSerializedNameHeader x)
		{
			FSerializedNameHeader fSerializedNameHeader = x;
			return fSerializedNameHeader.Length;
		}).ToArray();
		for (int num2 = 0; num2 < array3.Length; num2++)
		{
			byte[] bytes = Reader.ReadBytes((int)array2[num2]);
			array3[num2] = Encoding.ASCII.GetString(bytes);
		}
		NameMapHashes = array;
		ExtraOfDataSize = (int)Reader.Position;
		return array3;
	}

	public void DeserializeExportMap(Reader Reader)
	{
		for (int i = 0; i < ExportMap.Length; i++)
		{
			long position = Reader.Position;
			ExportMap[i].CookedSerialOffset = Reader.Read<ulong>();
			ExportMap[i].CookedSerialSize = Reader.Read<ulong>();
			ExportMap[i].ObjectName = Reader.Read<FMappedName>();
			ExportMap[i].OuterIndex = Reader.Read<FPackageObjectIndex>();
			ExportMap[i].ClassIndex = Reader.Read<FPackageObjectIndex>();
			ExportMap[i].SuperIndex = Reader.Read<FPackageObjectIndex>();
			ExportMap[i].TemplateIndex = Reader.Read<FPackageObjectIndex>();
			ExportMap[i].PublicExportHash = Reader.Read<ulong>();
			ExportMap[i].ObjectFlags = Reader.Read<EObjectFlags>();
			ExportMap[i].FilterFlags = Reader.Read<byte>();
			Reader.Position = position + 72;
		}
	}

	public Deserializer Clone()
	{
		return new Deserializer
		{
			ExportMap = ExportMap,
			ImportMap = ImportMap,
			NameMap = NameMap,
			NameMapHashes = NameMapHashes,
			RestOfData = RestOfData,
			Summary = Summary,
			ExtraOfData = ExtraOfData,
			ExtraOfDataSize = ExtraOfDataSize
		};
	}
}
