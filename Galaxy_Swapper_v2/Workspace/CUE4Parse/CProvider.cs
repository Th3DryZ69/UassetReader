using System;
using System.IO;
using System.Linq;
using CUE4Parse.FileProvider;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Versions;
using Serilog;
using Uasset_Reader.Workspace.Other;

namespace Galaxy_Swapper_v2.Workspace.CUE4Parse;

public static class CProvider
{
	public class ExportData
	{
		public byte[] Buffer { get; set; }

		public byte[] CompressedBuffer { get; set; }

		public string Ucas { get; set; }

		public string Utoc { get; set; }

		public FIoStoreTocCompressedBlockEntry CompressionBlock { get; set; }

		public FIoOffsetAndLength ChunkOffsetLengths { get; set; }

		public long Offset { get; set; }

		public ExportData Clone()
		{
			return new ExportData
			{
				Buffer = Buffer,
				ChunkOffsetLengths = ChunkOffsetLengths,
				CompressedBuffer = CompressedBuffer,
				CompressionBlock = CompressionBlock,
				Offset = Offset,
				Ucas = Ucas,
				Utoc = Utoc
			};
		}
	}

	public static bool SaveExport;

	private static DefaultFileProvider Provider { get; set; }

	public static ExportData Export { get; set; }

	public static bool Initialize(string Path)
	{
		if (Provider != null)
		{
			Log.Information("Provider was already loaded skipping.");
			return true;
		}
		try
		{
			Provider = new DefaultFileProvider(Path, SearchOption.TopDirectoryOnly, isCaseInsensitive: false, new VersionContainer(EGame.GAME_UE5_3));
			Provider.Initialize();
			Provider.SubmitKeys(AES.Keys);
			Log.Information($"Loaded Provider with version: {Provider.Versions.Game} to path ({Path}) with {AES.Keys.Count()} AES keys.");
			return true;
		}
		catch (Exception ex)
		{
			Log.Error("Failed to initialize Provider! " + ex.Message);
			return false;
		}
	}

	public static bool Save(string Path)
	{
		Log.Information("Exporting " + Path);
		SaveExport = true;
		Export = null;
		byte[] data;
		bool result = Provider.TrySaveAsset(Path, out data);
		if (data != null)
		{
			Export.Buffer = data;
		}
		return result;
	}
}
