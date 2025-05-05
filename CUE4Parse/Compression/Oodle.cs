using System.Runtime.InteropServices;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.Compression;

public static class Oodle
{
	public unsafe delegate long OodleDecompress(byte* bufferPtr, long bufferSize, byte* outputPtr, long outputSize, int a, int b, int c, long d, long e, long f, long g, long h, long i, int threadModule);

	private const string WARFRAME_CDN_HOST = "https://origin.warframe.com";

	private const string WARFRAME_INDEX_PATH = "/origin/E926E926/index.txt.lzma";

	public const string OODLE_DLL_NAME = "oo2core_9_win64.dll";

	public static OodleDecompress DecompressFunc;

	private static string WARFRAME_INDEX_URL => "https://origin.warframe.com/origin/E926E926/index.txt.lzma";

	unsafe static Oodle()
	{
		DecompressFunc = OodleLZ_Decompress;
	}

	public unsafe static void Decompress(byte[] compressed, int compressedOffset, int compressedSize, byte[] uncompressed, int uncompressedOffset, int uncompressedSize, FArchive? reader = null)
	{
		long num;
		fixed (byte* ptr = compressed)
		{
			fixed (byte* ptr2 = uncompressed)
			{
				num = DecompressFunc(ptr + compressedOffset, compressedSize, ptr2 + uncompressedOffset, uncompressedSize, 0, 0, 0, 0L, 0L, 0L, 0L, 0L, 0L, 3);
			}
		}
		if (num <= 0)
		{
			if (reader != null)
			{
				throw new OodleException(reader, $"Oodle decompression failed with result {num}");
			}
			throw new OodleException($"Oodle decompression failed with result {num}");
		}
	}

	[DllImport("oo2core_9_win64.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern long OodleLZ_Decompress(byte[] buffer, long bufferSize, byte[] output, long outputBufferSize, int a, int b, int c, long d, long e, long f, long g, long h, long i, int threadModule);

	[DllImport("oo2core_9_win64.dll", CallingConvention = CallingConvention.Cdecl)]
	public unsafe static extern long OodleLZ_Decompress(byte* buffer, long bufferSize, byte* output, long outputBufferSize, int a, int b, int c, long d, long e, long f, long g, long h, long i, int threadModule);
}
