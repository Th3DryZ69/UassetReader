using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;

namespace Uasset_Reader.Workspace.Utilities;

public static class Miscellaneous
{
	public static bool ValidJson(this string Content)
	{
		try
		{
			JObject.Parse(Content);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static bool MatchObjects(ref List<string> NameMaps, out string Match, string ToMatch)
	{
		foreach (string NameMap in NameMaps)
		{
			if (!NameMap.StartsWith("/") && NameMap.StartsWith(ToMatch))
			{
				Match = NameMap;
				return true;
			}
		}
		Match = string.Empty;
		return true;
	}

	public static bool IsEncrypted(this string JSON)
	{
		if (Regex.IsMatch(JSON, "^[a-zA-Z0-9\\+/]*={0,2}$"))
		{
			return true;
		}
		return false;
	}

	public static void LoadImage(this Image Image, string url)
	{
		BitmapImage bitmapImage = new BitmapImage();
		bitmapImage.BeginInit();
		bitmapImage.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
		bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
		bitmapImage.EndInit();
		Image.Source = bitmapImage;
	}

	public static int CountChars(this string source, char toFind)
	{
		int num = 0;
		for (int i = 0; i < source.Length; i++)
		{
			if (source[i] == toFind)
			{
				num++;
			}
		}
		return num;
	}

	public static string ByteToHex(this byte[] Bytes)
	{
		return BitConverter.ToString(Bytes).Replace('-', ' ');
	}

	public static byte[] HexToByte(this string hexString)
	{
		hexString = hexString.Replace(" ", "");
		byte[] array = new byte[hexString.Length / 2];
		for (int i = 0; i < array.Length; i++)
		{
			string s = hexString.Substring(i * 2, 2);
			array[i] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		}
		return array;
	}

	public static byte[] ChunkOffsetLengths(byte[] Orignal, int Length)
	{
		byte[] array = new byte[5];
		Buffer.BlockCopy(BitConverter.GetBytes(Length), 0, array, 0, 4);
		Buffer.BlockCopy(new byte[1], 0, array, 4, 1);
		Array.Reverse(array);
		byte[] array2 = new byte[Orignal.Length];
		Buffer.BlockCopy(Orignal, 0, array2, 0, Orignal.Length);
		Buffer.BlockCopy(array, 0, array2, 5, 5);
		return array2;
	}

	public static byte[] Combine(params byte[][] arrays)
	{
		byte[] array = new byte[arrays.Sum((byte[] a) => a.Length)];
		int num = 0;
		foreach (byte[] array2 in arrays)
		{
			Buffer.BlockCopy(array2, 0, array, num, array2.Length);
			num += array2.Length;
		}
		return array;
	}

	public static byte[] MatchToByte(byte[] content, int ByeLength)
	{
		List<byte> list = new List<byte>(content);
		int num = ByeLength - content.Length;
		for (int i = 0; i < num; i++)
		{
			list.Add(0);
		}
		return list.ToArray();
	}

	public static byte[] CompressionBlock(byte[] Orignal, uint CompressedSize, uint UncompressedSize, bool IsCompressed)
	{
		byte[] array = new byte[Orignal.Length];
		Buffer.BlockCopy(Orignal, 0, array, 0, Orignal.Length);
		Buffer.BlockCopy(BitConverter.GetBytes(CompressedSize), 0, array, 5, 3);
		Buffer.BlockCopy(BitConverter.GetBytes(UncompressedSize), 0, array, 8, 3);
		if (IsCompressed)
		{
			Buffer.BlockCopy(BitConverter.GetBytes(1), 0, array, 11, 1);
		}
		else
		{
			Buffer.BlockCopy(BitConverter.GetBytes(0), 0, array, 11, 1);
		}
		return array;
	}

	public static int IndexOfSequence(this byte[] buffer, byte[] pattern, int Star)
	{
		int startIndex = 0;
		if (Star != 0)
		{
			startIndex = Star;
		}
		int num = Array.IndexOf(buffer, pattern[0], startIndex);
		while (num >= 0 && num <= buffer.Length - pattern.Length)
		{
			byte[] array = new byte[pattern.Length];
			Buffer.BlockCopy(buffer, num, array, 0, pattern.Length);
			if (array.SequenceEqual(pattern))
			{
				return num;
			}
			num = Array.IndexOf(buffer, pattern[0], num + 1);
		}
		return -1;
	}

	public static string FormatHexSwap(string Ucas, string Utoc, byte[] Search, byte[] Replace, byte[] CompressionBlock, byte[] RCompressionBlock, byte[] ChunkAndLengths, byte[] RChunkAndLengths)
	{
		string empty = string.Empty;
		empty += $"Search ({Ucas}):\n\n{Search.ByteToHex()}\n\nReplace:\n\n{Replace.ByteToHex()}";
		if (CompressionBlock != null)
		{
			empty += $"\n\nSearch ({Utoc}):\n\n{CompressionBlock.ByteToHex()}\n\nReplace:\n\n{RCompressionBlock.ByteToHex()}";
		}
		if (ChunkAndLengths != null)
		{
			empty += $"\n\nSearch ({Utoc}):\n\n{ChunkAndLengths.ByteToHex()}\n\nReplace:\n\n{RChunkAndLengths.ByteToHex()}";
		}
		return empty;
	}

	public static void UrlStart(this string url)
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = "cmd.exe",
			Arguments = "/C start " + url,
			WindowStyle = ProcessWindowStyle.Hidden,
			CreateNoWindow = true
		});
	}
}
