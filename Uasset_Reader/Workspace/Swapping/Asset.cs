using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Galaxy_Swapper_v2.Workspace.CUE4Parse;
using Newtonsoft.Json.Linq;
using Serilog;
using Uasset_Reader.Workspace.Swapping.Compression;
using Uasset_Reader.Workspace.Swapping.Sterilization;
using Uasset_Reader.Workspace.Utilities;

namespace Uasset_Reader.Workspace.Swapping;

public class Asset
{
	public string ObjectPath;

	public CProvider.ExportData Export;

	public Deserializer Deserializer = new Deserializer();

	public int Size;

	public Asset(string objectpath, CProvider.ExportData export)
	{
		ObjectPath = objectpath;
		Export = export;
		Size = Export.Buffer.Length;
		Deserializer.Deserialize(Export.Buffer);
		Presence.Update("Viewing: " + Path.GetFileNameWithoutExtension(ObjectPath));
	}

	public List<string> Read()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> NameMaps = Deserializer.NameMap.ToList();
		foreach (string item in NameMaps)
		{
			if (list2.Contains(item))
			{
				continue;
			}
			if (!item.StartsWith("/"))
			{
				list.Add(item);
				list2.Add(item);
				continue;
			}
			if (Miscellaneous.MatchObjects(ref NameMaps, out string Match, Path.GetFileNameWithoutExtension(item)))
			{
				list.Add(item + "." + Match);
				list2.Add(Match);
			}
			else
			{
				list.Add(item);
			}
			list2.Add(item);
		}
		return list;
	}

	public void ReplaceNameMap(string Search, string Replace)
	{
		int num = Deserializer.NameMap.ToList().FindIndex((string x) => x == Search);
		if (!Deserializer.NameMap.ToList().Contains(Search) || num < 0)
		{
			Log.Warning("Failed to replace " + Search + " to " + Replace);
			throw new Exception("Could not find " + Search + " in asset!");
		}
		if (Replace.Length > 255)
		{
			Log.Warning("Failed to replace " + Search + " to " + Replace);
			throw new Exception(Replace + "\nCan not be over 255 characters long!");
		}
		Deserializer.NameMap[num] = Replace;
		Log.Information("Replaced " + Search + " to " + Replace);
	}

	public void ReplaceNameMapAndHashes(Deserializer AssetDeserializer, ref Deserializer AssetDeserializerTo, string Search, string Replace)
	{
		try
		{
			int num = AssetDeserializerTo.NameMap.ToList().FindIndex((string x) => x == Search);
			int num2 = AssetDeserializer.NameMap.ToList().FindIndex((string x) => x == Replace);
			if (num < 0)
			{
				Log.Error("Failed to find " + Search + " namemap");
				throw new Exception("Failed to find " + Search + " namemap!");
			}
			if (num2 < 0)
			{
				Log.Error("Failed to find " + Replace + " namemap");
				throw new Exception("Failed to find " + Replace + " namemap!");
			}
			AssetDeserializerTo.NameMapHashes[num] = AssetDeserializer.NameMapHashes[num2];
			AssetDeserializerTo.NameMap[num] = Replace;
			Log.Information($"Replaced {Search} to {Replace} with hash");
		}
		catch (Exception ex)
		{
			Log.Error("Failed to replace namemap and hashes " + Search + " To " + Replace);
			Log.Error(ex.Message);
			throw new Exception("Failed to replace namemap and hashes " + Search + " To " + Replace);
		}
	}

	public void ReplaceHex(string Search, string Replace)
	{
		try
		{
			List<byte> list = new List<byte>(Deserializer.RestOfData);
			byte[] array = Search.HexToByte();
			byte[] collection = Replace.HexToByte();
			int num = Deserializer.RestOfData.IndexOfSequence(array, 0);
			if (num < 0)
			{
				Log.Warning("Failed to replace hex " + Search + " to " + Replace);
				throw new Exception("Could not find " + Search + " in asset!");
			}
			list.RemoveRange(num, array.Length);
			list.InsertRange(num, collection);
			Deserializer.RestOfData = list.ToArray();
		}
		catch (Exception ex)
		{
			Log.Error("Failed to replace hex " + Search + " To " + Replace);
			Log.Error(ex.Message);
			throw new Exception("Failed to replace hex " + Search + " To " + Replace);
		}
	}

	public void ImportObjects(string NewObjectPath, CProvider.ExportData NewExport)
	{
		Deserializer AssetDeserializerTo = new Deserializer();
		AssetDeserializerTo.Deserialize(NewExport.Buffer);
		ReplaceNameMapAndHashes(Deserializer, ref AssetDeserializerTo, NewObjectPath, ObjectPath);
		ReplaceNameMapAndHashes(Deserializer, ref AssetDeserializerTo, Path.GetFileNameWithoutExtension(NewObjectPath), Path.GetFileNameWithoutExtension(ObjectPath));
		ulong publicExportHash = Deserializer.ExportMap[Deserializer.ExportMap.Length - 1].PublicExportHash;
		AssetDeserializerTo.ExportMap[AssetDeserializerTo.ExportMap.Length - 1].PublicExportHash = publicExportHash;
		Size = NewExport.Buffer.Length;
		Deserializer = AssetDeserializerTo;
		Log.Information("Imported all strings from " + NewObjectPath);
	}

	public bool SaveAsset(out byte[] Uncompressed, out byte[] Compressed)
	{
		Deserializer deserializer = Deserializer.Clone();
		CProvider.ExportData export = CProvider.Export.Clone();
		Uncompressed = new Serializer(Deserializer).Serialize(Size);
		Compressed = null;
		if (Uncompressed.Length >= 1024)
		{
			Buffer.BlockCopy(Miscellaneous.MatchToByte(new byte[0], 25), 0, Uncompressed, 60, 25);
		}
		else
		{
			Buffer.BlockCopy(Miscellaneous.MatchToByte(new byte[0], 25), 0, Uncompressed, 60, 15);
		}
		if (Settings.Read(Settings.Type.UtocModification).Value<bool>())
		{
			Compressed = Oodle.Compress(Uncompressed, Oodle.OodleCompressionLevel.Level5);
			Deserializer = deserializer;
			CProvider.Export = export;
			return true;
		}
		if (Uncompressed.Length > Export.Buffer.Length)
		{
			MessageBox.Show(Memory.MainView, $"Modified asset is longer then orignal! To fix this shorten string lengths.\nModified Length:{Uncompressed.Length}\nOrignal Length:{Export.Buffer.Length}", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
			Deserializer = deserializer;
			CProvider.Export = export;
			return false;
		}
		Uncompressed = Miscellaneous.MatchToByte(Uncompressed, Export.Buffer.Length);
		Compressed = Oodle.Compress(Uncompressed, Oodle.OodleCompressionLevel.Level5);
		if (Compressed.Length > Export.CompressedBuffer.Length)
		{
			MessageBox.Show(Memory.MainView, $"Modified compressed asset is longer then orignal! To fix this shorten string lengths or enable utoc modification!\nModified Length:{Compressed.Length}\nOrignal Length:{Export.CompressedBuffer.Length}", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
			Deserializer = deserializer;
			CProvider.Export = export;
			return false;
		}
		Deserializer = deserializer;
		CProvider.Export = export;
		return true;
	}
}
