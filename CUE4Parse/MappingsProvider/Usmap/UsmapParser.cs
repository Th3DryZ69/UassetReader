using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using CUE4Parse.Compression;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Serialization;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.MappingsProvider.Usmap;

public class UsmapParser
{
	private const ushort FileMagic = 12484;

	public readonly TypeMappings? Mappings;

	public readonly EUsmapCompressionMethod CompressionMethod;

	public readonly EUsmapVersion Version;

	public readonly FPackageFileVersion PackageVersion;

	public readonly FCustomVersionContainer CustomVersions;

	public readonly uint NetCL;

	public UsmapParser(string path, string name = "An unnamed usmap")
		: this(File.OpenRead(path), name)
	{
	}

	public UsmapParser(Stream data, string name = "An unnamed usmap")
		: this(new FStreamArchive(name, data))
	{
	}

	public UsmapParser(byte[] data, string name = "An unnamed usmap")
		: this(new FByteArchive(name, data))
	{
	}

	public UsmapParser(FArchive archive)
	{
		if (archive.Read<ushort>() != 12484)
		{
			throw new ParserException("Usmap has invalid magic");
		}
		Version = archive.Read<EUsmapVersion>();
		if ((int)Version > 1)
		{
			throw new ParserException($"Usmap has invalid version ({Version})");
		}
		FUsmapReader fUsmapReader = new FUsmapReader(archive, Version);
		if ((int)fUsmapReader.Version >= 1 && fUsmapReader.ReadBoolean())
		{
			PackageVersion = fUsmapReader.Read<FPackageFileVersion>();
			CustomVersions = new FCustomVersionContainer(fUsmapReader);
			NetCL = fUsmapReader.Read<uint>();
		}
		else
		{
			PackageVersion = default(FPackageFileVersion);
			CustomVersions = new FCustomVersionContainer();
			NetCL = 0u;
		}
		CompressionMethod = fUsmapReader.Read<EUsmapCompressionMethod>();
		uint num = fUsmapReader.Read<uint>();
		uint num2 = fUsmapReader.Read<uint>();
		byte[] array = new byte[num2];
		switch (CompressionMethod)
		{
		case EUsmapCompressionMethod.None:
			if (num != num2)
			{
				throw new ParserException("No compression: Compression size must be equal to decompression size");
			}
			fUsmapReader.Read(array, 0, (int)num);
			break;
		case EUsmapCompressionMethod.Oodle:
			Oodle.Decompress(fUsmapReader.ReadBytes((int)num), 0, (int)num, array, 0, (int)num2);
			break;
		case EUsmapCompressionMethod.Brotli:
		{
			using (BrotliDecoder brotliDecoder = default(BrotliDecoder))
			{
				brotliDecoder.Decompress(fUsmapReader.ReadBytes((int)num), array, out var _, out var _);
			}
			break;
		}
		default:
			throw new ParserException($"Invalid compression method {CompressionMethod}");
		}
		fUsmapReader = new FUsmapReader(new FByteArchive(fUsmapReader.Name, array), fUsmapReader.Version);
		uint num3 = fUsmapReader.Read<uint>();
		List<string> list = new List<string>((int)num3);
		for (int i = 0; i < num3; i++)
		{
			byte nameLength = fUsmapReader.Read<byte>();
			list.Add(fUsmapReader.ReadStringUnsafe(nameLength));
		}
		uint num4 = fUsmapReader.Read<uint>();
		Dictionary<string, Dictionary<int, string>> dictionary = new Dictionary<string, Dictionary<int, string>>((int)num4);
		for (int j = 0; j < num4; j++)
		{
			string key = fUsmapReader.ReadName(list);
			byte b = fUsmapReader.Read<byte>();
			Dictionary<int, string> dictionary2 = new Dictionary<int, string>(b);
			for (int k = 0; k < b; k++)
			{
				string value = fUsmapReader.ReadName(list);
				dictionary2[k] = value;
			}
			dictionary.Add(key, dictionary2);
		}
		uint num5 = fUsmapReader.Read<uint>();
		Dictionary<string, Struct> dictionary3 = new Dictionary<string, Struct>();
		TypeMappings typeMappings = new TypeMappings(dictionary3, dictionary);
		for (int l = 0; l < num5; l++)
		{
			Struct obj = UsmapProperties.ParseStruct(typeMappings, fUsmapReader, list);
			dictionary3[obj.Name] = obj;
		}
		Mappings = typeMappings;
	}
}
