using System.Collections.Generic;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Shaders;

[JsonConverter(typeof(FShaderCodeArchiveConverter))]
public class FShaderCodeArchive
{
	public readonly byte[][] ShaderCode;

	public readonly FRHIShaderLibrary SerializedShaders;

	public readonly Dictionary<FSHAHash, FShaderCodeEntry> PrevCookedShaders;

	public FShaderCodeArchive(FArchive Ar)
	{
		uint num = Ar.Read<uint>();
		bool flag = false;
		if (Ar.Game >= EGame.GAME_UE5_0 && num == 1)
		{
			flag = true;
		}
		switch (num)
		{
		case 2u:
		{
			FSerializedShaderArchive fSerializedShaderArchive = new FSerializedShaderArchive(Ar);
			ShaderCode = new byte[fSerializedShaderArchive.ShaderEntries.Length][];
			for (int i = 0; i < fSerializedShaderArchive.ShaderEntries.Length; i++)
			{
				ShaderCode[i] = Ar.ReadBytes((int)fSerializedShaderArchive.ShaderEntries[i].Size);
			}
			SerializedShaders = fSerializedShaderArchive;
			break;
		}
		case 1u:
			if (flag)
			{
				FIoStoreShaderCodeArchive serializedShaders = new FIoStoreShaderCodeArchive(Ar);
				SerializedShaders = serializedShaders;
			}
			break;
		}
	}
}
