using System;
using System.Runtime.InteropServices;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FShaderParameterBindings
{
	public struct FParameter
	{
		public ushort BufferIndex;

		public ushort BaseIndex;

		public ushort ByteOffset;

		public ushort ByteSize;
	}

	[StructLayout(LayoutKind.Sequential, Size = 4)]
	public struct FResourceParameter
	{
		public ushort ByteOffset;

		public byte BaseIndex;

		public EUniformBufferBaseType BaseType;

		public FResourceParameter(FMemoryImageArchive Ar)
		{
			BaseType = EUniformBufferBaseType.UBMT_INVALID;
			BaseIndex = (byte)Ar.Read<ushort>();
			ByteOffset = Ar.Read<ushort>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 8)]
	public struct FBindlessResourceParameter
	{
		public ushort ByteOffset;

		public ushort GlobalConstantOffset;

		public EUniformBufferBaseType BaseType;
	}

	[StructLayout(LayoutKind.Sequential, Size = 4)]
	public struct FParameterStructReference
	{
		public ushort BufferIndex;

		public ushort ByteOffset;
	}

	public FParameter[]? Parameters;

	public FResourceParameter[]? Textures;

	public FResourceParameter[]? SRVs;

	public FResourceParameter[]? UAVs;

	public FResourceParameter[]? Samplers;

	public FResourceParameter[]? GraphTextures;

	public FResourceParameter[]? GraphSRVs;

	public FResourceParameter[]? GraphUAVs;

	public FResourceParameter[]? ResourceParameters;

	public FBindlessResourceParameter[] BindlessResourceParameters;

	public FParameterStructReference[] GraphUniformBuffers;

	public FParameterStructReference[] ParameterReferences;

	public uint StructureLayoutHash;

	public ushort RootParameterBufferIndex = ushort.MaxValue;

	public FShaderParameterBindings(FMemoryImageArchive Ar)
	{
		Parameters = Ar.ReadArray<FParameter>();
		if (Ar.Game >= EGame.GAME_UE4_26)
		{
			ResourceParameters = Ar.ReadArray<FResourceParameter>();
		}
		else
		{
			Textures = Ar.ReadArray(() => new FResourceParameter(Ar));
			SRVs = Ar.ReadArray(() => new FResourceParameter(Ar));
			UAVs = Ar.ReadArray(() => new FResourceParameter(Ar));
			Samplers = Ar.ReadArray(() => new FResourceParameter(Ar));
			GraphTextures = Ar.ReadArray(() => new FResourceParameter(Ar));
			GraphSRVs = Ar.ReadArray(() => new FResourceParameter(Ar));
			GraphUAVs = Ar.ReadArray(() => new FResourceParameter(Ar));
		}
		BindlessResourceParameters = ((Ar.Game >= EGame.GAME_UE5_1) ? Ar.ReadArray<FBindlessResourceParameter>() : Array.Empty<FBindlessResourceParameter>());
		GraphUniformBuffers = ((Ar.Game >= EGame.GAME_UE4_26) ? Ar.ReadArray<FParameterStructReference>() : Array.Empty<FParameterStructReference>());
		ParameterReferences = Ar.ReadArray<FParameterStructReference>();
		StructureLayoutHash = Ar.Read<uint>();
		RootParameterBufferIndex = Ar.Read<ushort>();
		Ar.Position += 2L;
	}
}
