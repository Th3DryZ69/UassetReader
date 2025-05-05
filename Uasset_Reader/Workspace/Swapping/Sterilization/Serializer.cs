using System.Linq;
using System.Text;
using CUE4Parse.UE4.IO.Objects;
using Uasset_Reader.Workspace.Swapping.Other;
using Uasset_Reader.Workspace.Swapping.Utilities;

namespace Uasset_Reader.Workspace.Swapping.Sterilization;

public class Serializer
{
	private Deserializer Deserializer { get; set; }

	public Serializer(Deserializer deserializer)
	{
		Deserializer = deserializer;
	}

	public byte[] Serialize(int originalSize)
	{
		Writer writer = new Writer(Enumerable.Repeat((byte)0, Deserializer.RestOfData.Length + 65536).ToArray());
		writer.Write(Deserializer.Summary);
		SerializeNameBatch(writer, Deserializer.NameMap);
		if (Deserializer.ExtraOfData != null)
		{
			writer.WriteBytes(Deserializer.ExtraOfData);
		}
		Deserializer.Summary.ImportedPublicExportHashesOffset = (int)writer.Position;
		writer.WriteBytes(Deserializer.RestOfData);
		long position = writer.Position;
		int num = (int)(position - originalSize);
		Deserializer.Summary.ImportMapOffset += num;
		Deserializer.Summary.ExportMapOffset += num;
		Deserializer.Summary.ExportBundleEntriesOffset += num;
		Deserializer.Summary.DependencyBundleHeadersOffset += num;
		Deserializer.Summary.DependencyBundleEntriesOffset += num;
		Deserializer.Summary.ImportedPackageNamesOffset += num;
		writer.Position = Deserializer.Summary.ExportMapOffset;
		SerializeExportMap(writer);
		writer.Position = 0L;
		Deserializer.Summary.HeaderSize += (uint)num;
		Deserializer.Summary.CookedHeaderSize += (uint)num;
		writer.Write(Deserializer.Summary);
		return writer.ToByteArray(position);
	}

	private void SerializeExportMap(Writer Ar)
	{
		FExportMapEntry[] exportMap = Deserializer.ExportMap;
		for (int i = 0; i < exportMap.Length; i++)
		{
			FExportMapEntry fExportMapEntry = exportMap[i];
			Ar.Write(fExportMapEntry.CookedSerialOffset);
			Ar.Write(fExportMapEntry.CookedSerialSize);
			Ar.Write(fExportMapEntry.ObjectName.NameIndex);
			Ar.Write(fExportMapEntry.ObjectName.ExtraIndex);
			Ar.Write(fExportMapEntry.OuterIndex);
			Ar.Write(fExportMapEntry.ClassIndex);
			Ar.Write(fExportMapEntry.SuperIndex);
			Ar.Write(fExportMapEntry.TemplateIndex);
			Ar.Write(fExportMapEntry.PublicExportHash);
			Ar.Write(fExportMapEntry.ObjectFlags);
			Ar.WriteBytes(new byte[4] { fExportMapEntry.FilterFlags, 0, 0, 0 });
		}
	}

	public void SerializeNameBatch(Writer Ar, string[] names)
	{
		Ar.Write(names.Length);
		if (names.Length != 0)
		{
			Ar.Write(names.Sum((string x) => x.Length));
			Ar.Write(Deserializer.HashVersion);
			string[] array = names;
			foreach (string text in array)
			{
				Ar.Write(CityHash.Hash(Encoding.ASCII.GetBytes(text.ToLower())));
			}
			array = names;
			foreach (string text2 in array)
			{
				Ar.WriteBytes(new byte[2]
				{
					0,
					(byte)text2.Length
				});
			}
			array = names;
			foreach (string s in array)
			{
				Ar.WriteBytes(Encoding.ASCII.GetBytes(s));
			}
		}
	}
}
