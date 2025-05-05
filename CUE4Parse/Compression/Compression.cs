using System;
using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Readers;
using Ionic.Zlib;
using K4os.Compression.LZ4;

namespace CUE4Parse.Compression;

public static class Compression
{
	public const int LOADING_COMPRESSION_CHUNK_SIZE = 131072;

	public static byte[] Decompress(byte[] compressed, int uncompressedSize, CompressionMethod method, FArchive? reader = null)
	{
		return Decompress(compressed, 0, compressed.Length, uncompressedSize, method, reader);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte[] Decompress(byte[] compressed, int compressedOffset, int compressedCount, int uncompressedSize, CompressionMethod method, FArchive? reader = null)
	{
		byte[] array = new byte[uncompressedSize];
		Decompress(compressed, compressedOffset, compressedCount, array, 0, uncompressedSize, method);
		return array;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Decompress(byte[] compressed, byte[] dst, CompressionMethod method, FArchive? reader = null)
	{
		Decompress(compressed, 0, compressed.Length, dst, 0, dst.Length, method, reader);
	}

	public static void Decompress(byte[] compressed, int compressedOffset, int compressedSize, byte[] uncompressed, int uncompressedOffset, int uncompressedSize, CompressionMethod method, FArchive? reader = null)
	{
		using MemoryStream stream = new MemoryStream(compressed, compressedOffset, compressedSize, writable: false)
		{
			Position = 0L
		};
		switch (method)
		{
		case CompressionMethod.None:
			Buffer.BlockCopy(compressed, compressedOffset, uncompressed, uncompressedOffset, compressedSize);
			break;
		case CompressionMethod.Zlib:
		{
			ZlibStream zlibStream = new ZlibStream(stream, CompressionMode.Decompress);
			zlibStream.Read(uncompressed, uncompressedOffset, uncompressedSize);
			zlibStream.Dispose();
			break;
		}
		case CompressionMethod.Gzip:
		{
			GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress);
			gZipStream.Read(uncompressed, uncompressedOffset, uncompressedSize);
			gZipStream.Dispose();
			break;
		}
		case CompressionMethod.Oodle:
			Oodle.Decompress(compressed, compressedOffset, compressedSize, uncompressed, uncompressedOffset, uncompressedSize, reader);
			break;
		case CompressionMethod.LZ4:
		{
			byte[] array = new byte[uncompressedSize + uncompressedSize / 255 + 16];
			int num = LZ4Codec.Decode(compressed, compressedOffset, compressedSize, array, 0, array.Length);
			Buffer.BlockCopy(array, 0, uncompressed, uncompressedOffset, uncompressedSize);
			if (num != uncompressedSize)
			{
				throw new FileLoadException($"Failed to decompress LZ4 data (Expected: {uncompressedSize}, Result: {num})");
			}
			break;
		}
		default:
			if (reader != null)
			{
				throw new UnknownCompressionMethodException(reader, $"Compression method \"{method}\" is unknown");
			}
			throw new UnknownCompressionMethodException($"Compression method \"{method}\" is unknown");
		}
	}
}
