using System;
using System.Runtime.InteropServices;

namespace Uasset_Reader.Workspace.Swapping.Compression;

public static class Oodle
{
	private enum OodleFormat : uint
	{
		LZH,
		LZHLW,
		LZNIB,
		None,
		LZB16,
		LZBLW,
		LZA,
		LZNA,
		Kraken,
		Mermaid,
		BitKnit,
		Selkie,
		Akkorokamui
	}

	public enum OodleCompressionLevel : ulong
	{
		None,
		Fastest,
		Faster,
		Fast,
		Normal,
		Level1,
		Level2,
		Level3,
		Level4,
		Level5
	}

	public const string OODLE_5_DLL = "oo2core_5_win64.dll";

	[DllImport("oo2core_5_win64.dll")]
	private static extern int OodleLZ_Compress(OodleFormat format, byte[] decompressedBuffer, long decompressedSize, byte[] compressedBuffer, OodleCompressionLevel compressionLevel, uint a, uint b, uint c, uint threadModule);

	[DllImport("oo2core_5_win64.dll")]
	private static extern int OodleLZ_Decompress(byte[] buffer, long bufferSize, byte[] outputBuffer, long outputBufferSize, uint a, uint b, ulong c, uint d, uint e, uint f, uint g, uint h, uint i, uint threadModule);

	public static byte[] Compress(byte[] buffer, OodleCompressionLevel CompressionLevel)
	{
		uint num;
		try
		{
			num = (uint)OodleLZ_Compress(OodleFormat.Kraken, buffer, buffer.Length, new byte[buffer.Length + 274 * ((uint)(buffer.Length + 262143) / 262144u)], CompressionLevel, 0u, 0u, 0u, 0u);
		}
		catch (AccessViolationException)
		{
			num = 64u;
		}
		byte[] array = new byte[buffer.Length + (int)(274 * ((uint)(buffer.Length + 262143) / 262144u))];
		byte[] array2 = new byte[(uint)((int)num + OodleLZ_Compress(OodleFormat.Kraken, buffer, buffer.Length, array, CompressionLevel, 0u, 0u, 0u, 0u)) - (int)num];
		Buffer.BlockCopy(array, 0, array2, 0, OodleLZ_Compress(OodleFormat.Kraken, buffer, buffer.Length, array, CompressionLevel, 0u, 0u, 0u, 0u));
		return array2;
	}

	public static byte[] Decompress(byte[] data, int decompressedSize)
	{
		byte[] outputBuffer = new byte[decompressedSize];
		Decompress(data, (uint)data.Length, ref outputBuffer, (uint)decompressedSize);
		return outputBuffer;
	}

	private static uint Decompress(byte[] buffer, uint bufferSize, ref byte[] outputBuffer, uint outputBufferSize)
	{
		if (buffer.Length != 0 && bufferSize != 0 && outputBuffer.Length != 0 && outputBufferSize != 0)
		{
			return (uint)OodleLZ_Decompress(buffer, bufferSize, outputBuffer, outputBufferSize, 0u, 0u, 0uL, 0u, 0u, 0u, 0u, 0u, 0u, 0u);
		}
		return 0u;
	}
}
