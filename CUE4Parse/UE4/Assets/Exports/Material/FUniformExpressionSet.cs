using System;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FUniformExpressionSet
{
	public FMaterialUniformPreshaderHeader[] UniformVectorPreshaders = Array.Empty<FMaterialUniformPreshaderHeader>();

	public FMaterialUniformPreshaderHeader[] UniformScalarPreshaders = Array.Empty<FMaterialUniformPreshaderHeader>();

	public FMaterialScalarParameterInfo[] UniformScalarParameters = Array.Empty<FMaterialScalarParameterInfo>();

	public FMaterialVectorParameterInfo[] UniformVectorParameters = Array.Empty<FMaterialVectorParameterInfo>();

	public FMaterialUniformPreshaderHeader[] UniformPreshaders = Array.Empty<FMaterialUniformPreshaderHeader>();

	public FMaterialUniformPreshaderField[]? UniformPreshaderFields;

	public FMaterialNumericParameterInfo[] UniformNumericParameters = Array.Empty<FMaterialNumericParameterInfo>();

	public readonly FMaterialTextureParameterInfo[][] UniformTextureParameters = new FMaterialTextureParameterInfo[6][];

	public FMaterialExternalTextureParameterInfo[] UniformExternalTextureParameters;

	public uint UniformPreshaderBufferSize;

	public FMaterialPreshaderData UniformPreshaderData;

	public byte[] DefaultValues;

	public FMaterialVirtualTextureStack[] VTStacks;

	public FGuid[] ParameterCollections;

	public FRHIUniformBufferLayoutInitializer UniformBufferLayoutInitializer;

	public FUniformExpressionSet(FMemoryImageArchive Ar)
	{
		if (Ar.Game >= EGame.GAME_UE5_0)
		{
			UniformPreshaders = Ar.ReadArray(() => new FMaterialUniformPreshaderHeader(Ar));
			UniformPreshaderFields = ((Ar.Game >= EGame.GAME_UE5_1) ? Ar.ReadArray<FMaterialUniformPreshaderField>() : Array.Empty<FMaterialUniformPreshaderField>());
			UniformNumericParameters = Ar.ReadArray(() => new FMaterialNumericParameterInfo(Ar));
			Ar.ReadArray<FMaterialTextureParameterInfo[]>(UniformTextureParameters, () => Ar.ReadArray(() => new FMaterialTextureParameterInfo(Ar)));
			UniformExternalTextureParameters = Ar.ReadArray(() => new FMaterialExternalTextureParameterInfo(Ar));
			UniformPreshaderBufferSize = Ar.Read<uint>();
			Ar.Position += 4L;
			UniformPreshaderData = new FMaterialPreshaderData(Ar);
			DefaultValues = Ar.ReadArray<byte>();
			VTStacks = Ar.ReadArray(() => new FMaterialVirtualTextureStack(Ar));
			ParameterCollections = Ar.ReadArray<FGuid>();
			UniformBufferLayoutInitializer = new FRHIUniformBufferLayoutInitializer(Ar);
			return;
		}
		UniformVectorPreshaders = Ar.ReadArray(() => new FMaterialUniformPreshaderHeader(Ar));
		UniformScalarPreshaders = Ar.ReadArray(() => new FMaterialUniformPreshaderHeader(Ar));
		UniformScalarParameters = Ar.ReadArray(() => new FMaterialScalarParameterInfo(Ar));
		UniformVectorParameters = Ar.ReadArray(() => new FMaterialVectorParameterInfo(Ar));
		UniformTextureParameters = new FMaterialTextureParameterInfo[5][];
		Ar.ReadArray<FMaterialTextureParameterInfo[]>(UniformTextureParameters, () => Ar.ReadArray(() => new FMaterialTextureParameterInfo(Ar)));
		UniformExternalTextureParameters = Ar.ReadArray(() => new FMaterialExternalTextureParameterInfo(Ar));
		UniformPreshaderData = new FMaterialPreshaderData(Ar);
		VTStacks = Ar.ReadArray(() => new FMaterialVirtualTextureStack(Ar));
		ParameterCollections = Ar.ReadArray<FGuid>();
		UniformBufferLayoutInitializer = new FRHIUniformBufferLayoutInitializer(Ar);
	}
}
