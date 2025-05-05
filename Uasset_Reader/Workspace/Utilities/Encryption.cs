using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Uasset_Reader.Workspace.Utilities;

public static class Encryption
{
	public static string Compress(this string uncompressedString)
	{
		byte[] inArray;
		using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
		{
			using MemoryStream memoryStream2 = new MemoryStream();
			using (DeflateStream destination = new DeflateStream(memoryStream2, CompressionLevel.Optimal, leaveOpen: true))
			{
				memoryStream.CopyTo(destination);
			}
			inArray = memoryStream2.ToArray();
		}
		return Encrypt(Convert.ToBase64String(inArray));
	}

	public static string Decompress(this string compressedString)
	{
		byte[] bytes = null;
		using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(Decrypt(compressedString))))
		{
			using DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress);
			using MemoryStream memoryStream = new MemoryStream();
			deflateStream.CopyTo(memoryStream);
			bytes = memoryStream.ToArray();
		}
		return Encoding.UTF8.GetString(bytes);
	}

	private static string Encrypt(string plainText)
	{
		string text = "\ue0e4\ue8e4\uf8e4\ue0e4\ue8e4\uf8e4\uf8e4\uf8e4ᣤᣤ";
		string text2 = "졊젙졅졎젡졇졂졗";
		string s = "@1B2c3D5e5F5b7H8";
		int num = (text[0] ^ 0x61E8) + 3829;
		int num2 = num << 5;
		int num3 = num & 0xFFFF;
		int num4 = (num2 | (num3 >> 11)) & 0xFFFF;
		string text3 = text.Substring(0, 0);
		string text4 = ((char)(num4 & 0xFFFF)).ToString();
		string text5 = text;
		text = text3 + text4 + text5.Substring(1);
		int num5 = text2[0] - 51191;
		string text6 = text2.Substring(0, 0);
		string text7 = ((char)(num5 & 0xFFFF)).ToString();
		string text8 = text2;
		text2 = text6 + text7 + text8.Substring(1);
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		byte[] bytes2 = new Rfc2898DeriveBytes(text, Encoding.ASCII.GetBytes(text2)).GetBytes(32);
		ICryptoTransform cryptoTransform = new RijndaelManaged
		{
			Mode = CipherMode.CBC,
			Padding = PaddingMode.Zeros
		}.CreateEncryptor(bytes2, Encoding.ASCII.GetBytes(s));
		byte[] inArray;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			ICryptoTransform transform = cryptoTransform;
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
			{
				byte[] buffer = bytes;
				cryptoStream.Write(buffer, 0, bytes.Length);
				cryptoStream.FlushFinalBlock();
				inArray = memoryStream.ToArray();
				cryptoStream.Close();
			}
			memoryStream.Close();
		}
		return Convert.ToBase64String(inArray);
	}

	private static string Decrypt(string encryptedText)
	{
		string text = "\ue0e4\ue8e4\uf8e4\ue0e4\ue8e4\uf8e4\uf8e4\uf8e4ᣤᣤ";
		string text2 = "졊젙졅졎젡졇졂졗";
		string s = "@1B2c3D5e5F5b7H8";
		int num = (text[0] ^ 0x61E8) + 3829;
		int num2 = num << 5;
		int num3 = num & 0xFFFF;
		int num4 = (num2 | (num3 >> 11)) & 0xFFFF;
		string text3 = text.Substring(0, 0);
		string text4 = ((char)(num4 & 0xFFFF)).ToString();
		string text5 = text;
		text = text3 + text4 + text5.Substring(1);
		int num5 = text2[0] - 51191;
		string text6 = text2.Substring(0, 0);
		string text7 = ((char)(num5 & 0xFFFF)).ToString();
		string text8 = text2;
		text2 = text6 + text7 + text8.Substring(1);
		byte[] array = Convert.FromBase64String(encryptedText);
		byte[] bytes = new Rfc2898DeriveBytes(text, Encoding.ASCII.GetBytes(text2)).GetBytes(32);
		ICryptoTransform cryptoTransform = new RijndaelManaged
		{
			Mode = CipherMode.CBC,
			Padding = PaddingMode.None
		}.CreateDecryptor(bytes, Encoding.ASCII.GetBytes(s));
		MemoryStream memoryStream = new MemoryStream(array);
		ICryptoTransform transform = cryptoTransform;
		CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
		byte[] array2 = new byte[array.Length];
		byte[] buffer = array2;
		int count = cryptoStream.Read(buffer, 0, array2.Length);
		memoryStream.Close();
		cryptoStream.Close();
		Encoding uTF = Encoding.UTF8;
		byte[] bytes2 = array2;
		return uTF.GetString(bytes2, 0, count).TrimEnd("\0".ToCharArray());
	}
}
