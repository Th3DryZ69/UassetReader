using System;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(BytePropertyConverter))]
public class ByteProperty : FPropertyTagType<byte>
{
	public ByteProperty(FArchive Ar, ReadType type)
	{
		base.Value = type switch
		{
			ReadType.ZERO => 0, 
			ReadType.NORMAL => Ar.Read<byte>(), 
			ReadType.MAP => (byte)Ar.Read<uint>(), 
			ReadType.ARRAY => Ar.Read<byte>(), 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
	}
}
