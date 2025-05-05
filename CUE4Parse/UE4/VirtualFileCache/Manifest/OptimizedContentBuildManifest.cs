using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CUE4Parse.UE4.Readers;
using Ionic.Zlib;

namespace CUE4Parse.UE4.VirtualFileCache.Manifest;

public sealed class OptimizedContentBuildManifest
{
	private const uint _MANIFEST_HEADER_MAGIC = 1153351692u;

	public Dictionary<string, string> HashNameMap { get; private set; }

	public TimeSpan ParseTime { get; }

	public OptimizedContentBuildManifest(byte[] data)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		if (BitConverter.ToUInt32(data, 0) == 1153351692)
		{
			ParseData(data);
			stopwatch.Stop();
			ParseTime = stopwatch.Elapsed;
			return;
		}
		throw new NotImplementedException("JSON manifest parsing is not implemented yet");
	}

	private void ParseData(byte[] buffer)
	{
		FByteArchive fByteArchive = new FByteArchive("reader", buffer)
		{
			Position = 4L
		};
		int num = fByteArchive.Read<int>();
		int num2 = fByteArchive.Read<int>();
		int length = fByteArchive.Read<int>();
		fByteArchive.Position += 20L;
		EManifestStorageFlags eManifestStorageFlags = fByteArchive.Read<EManifestStorageFlags>();
		fByteArchive.Position += 4L;
		fByteArchive.Seek(num, SeekOrigin.Begin);
		byte[] array;
		switch (eManifestStorageFlags)
		{
		case EManifestStorageFlags.Compressed:
		{
			array = new byte[num2];
			using (MemoryStream stream = new MemoryStream(fByteArchive.ReadBytes(length)))
			{
				using ZlibStream zlibStream = new ZlibStream(stream, CompressionMode.Decompress);
				zlibStream.Read(array, 0, num2);
			}
			break;
		}
		case EManifestStorageFlags.Encrypted:
			throw new NotImplementedException("Encrypted Manifests are not supported yet");
		default:
			array = fByteArchive.ReadBytes(num2);
			break;
		}
		fByteArchive.Dispose();
		FByteArchive fByteArchive2 = new FByteArchive("manifest", array);
		int num3 = (int)fByteArchive2.Position;
		int num4 = fByteArchive2.Read<int>();
		fByteArchive2.Seek(num3 + num4, SeekOrigin.Begin);
		num3 = (int)fByteArchive2.Position;
		num4 = fByteArchive2.Read<int>();
		fByteArchive2.Seek(num3 + num4, SeekOrigin.Begin);
		fByteArchive2.Position += 4L;
		if ((int)fByteArchive2.Read<EManifestMetaVersion>() >= 0)
		{
			int num5 = fByteArchive2.Read<int>();
			string[] array2 = new string[num5];
			HashNameMap = new Dictionary<string, string>(num5);
			for (int i = 0; i < num5; i++)
			{
				array2[i] = fByteArchive2.ReadFString();
			}
			for (int j = 0; j < num5; j++)
			{
				int num6 = fByteArchive2.Read<int>();
				fByteArchive2.Seek((num6 < 0) ? (-num6 * 2) : num6, SeekOrigin.Current);
			}
			int num7 = (int)fByteArchive2.Position;
			for (int k = 0; k < num5; k++)
			{
				string key = BitConverter.ToString(array, num7 + k * 20, 20).Replace("-", "");
				HashNameMap[key] = array2[k];
			}
		}
	}
}
