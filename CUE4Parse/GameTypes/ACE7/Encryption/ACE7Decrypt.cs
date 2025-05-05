using System;
using System.IO;
using System.Reflection;
using System.Resources;
using CUE4Parse.UE4.Readers;
using CUE4Parse.Utils;

namespace CUE4Parse.GameTypes.ACE7.Encryption;

public class ACE7Decrypt
{
	private readonly byte[] fullKey;

	public ACE7Decrypt()
	{
		using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CUE4Parse.Resources.ACE7Key.bin");
		if (stream == null)
		{
			throw new MissingManifestResourceException("Couldn't find ACE7Key.bin in Embedded Resources");
		}
		fullKey = new byte[stream.Length];
		stream.Read(fullKey, 0, (int)stream.Length);
	}

	public FArchive DecryptUassetArchive(FArchive Ar, out ACE7XORKey key)
	{
		key = new ACE7XORKey(Ar.Name.SubstringBeforeLast('.').SubstringAfterLast('/'));
		byte[] array = new byte[Ar.Length];
		Ar.Read(array, 0, (int)Ar.Length);
		return new FByteArchive(Ar.Name, DecryptUasset(array, key), Ar.Versions);
	}

	public FArchive DecryptUexpArchive(FArchive Ar, ACE7XORKey key)
	{
		byte[] uexp = Ar.ReadBytes((int)Ar.Length);
		return new FByteArchive(Ar.Name, DecryptUexp(uexp, key), Ar.Versions);
	}

	public byte[] DecryptUasset(byte[] uasset, ACE7XORKey? key)
	{
		byte[] array = new byte[uasset.Length];
		BitConverter.GetBytes(2653586369u).CopyTo(array, 0);
		for (int i = 4; i < array.Length; i++)
		{
			array[i] = GetXORByte(uasset[i], ref key);
		}
		return array;
	}

	public byte[] DecryptUexp(byte[] uexp, ACE7XORKey? key)
	{
		byte[] array = new byte[uexp.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = GetXORByte(uexp[i], ref key);
		}
		BitConverter.GetBytes(2653586369u).CopyTo(array, array.Length - 4);
		return array;
	}

	private byte GetXORByte(byte tagb, ref ACE7XORKey? xorkey)
	{
		if (xorkey == null)
		{
			return tagb;
		}
		tagb = (byte)(tagb ^ fullKey[xorkey.pk1 * 1024 + xorkey.pk2] ^ 0x77);
		xorkey.pk1++;
		xorkey.pk2++;
		if (xorkey.pk1 >= 217)
		{
			xorkey.pk1 = 0;
		}
		if (xorkey.pk2 >= 1024)
		{
			xorkey.pk2 = 0;
		}
		return tagb;
	}
}
