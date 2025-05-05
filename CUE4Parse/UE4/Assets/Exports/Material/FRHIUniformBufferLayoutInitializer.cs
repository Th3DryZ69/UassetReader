using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FRHIUniformBufferLayoutInitializer
{
	public string Name;

	public FRHIUniformBufferResource[] Resources;

	public FRHIUniformBufferResource[] GraphResources;

	public FRHIUniformBufferResource[] GraphTextures;

	public FRHIUniformBufferResource[] GraphBuffers;

	public FRHIUniformBufferResource[] GraphUniformBuffers;

	public FRHIUniformBufferResource[] UniformBuffers;

	public uint Hash;

	public uint ConstantBufferSize;

	public ushort RenderTargetsOffset = ushort.MaxValue;

	public byte StaticSlot = byte.MaxValue;

	public EUniformBufferBindingFlags BindingFlags = EUniformBufferBindingFlags.Shader;

	public bool bHasNonGraphOutputs;

	public bool bNoEmulatedUniformBuffer;

	public FRHIUniformBufferLayoutInitializer(FMemoryImageArchive Ar)
	{
		if (Ar.Game >= EGame.GAME_UE5_0)
		{
			Name = Ar.ReadFString();
			Resources = Ar.ReadArray<FRHIUniformBufferResource>();
			GraphResources = Ar.ReadArray<FRHIUniformBufferResource>();
			GraphTextures = Ar.ReadArray<FRHIUniformBufferResource>();
			GraphBuffers = Ar.ReadArray<FRHIUniformBufferResource>();
			GraphUniformBuffers = Ar.ReadArray<FRHIUniformBufferResource>();
			UniformBuffers = Ar.ReadArray<FRHIUniformBufferResource>();
			Hash = Ar.Read<uint>();
			ConstantBufferSize = Ar.Read<uint>();
			RenderTargetsOffset = Ar.Read<ushort>();
			StaticSlot = Ar.Read<byte>();
			BindingFlags = Ar.Read<EUniformBufferBindingFlags>();
			bHasNonGraphOutputs = Ar.ReadFlag();
			bNoEmulatedUniformBuffer = Ar.ReadFlag();
			Ar.Position += 2L;
		}
		else if (Ar.Game >= EGame.GAME_UE4_26)
		{
			ConstantBufferSize = Ar.Read<uint>();
			StaticSlot = Ar.Read<byte>();
			Ar.Position++;
			RenderTargetsOffset = Ar.Read<ushort>();
			bHasNonGraphOutputs = Ar.ReadFlag();
			Ar.Position += 7L;
			Resources = Ar.ReadArray<FRHIUniformBufferResource>();
			GraphResources = Ar.ReadArray<FRHIUniformBufferResource>();
			GraphTextures = Ar.ReadArray<FRHIUniformBufferResource>();
			GraphBuffers = Ar.ReadArray<FRHIUniformBufferResource>();
			GraphUniformBuffers = Ar.ReadArray<FRHIUniformBufferResource>();
			UniformBuffers = Ar.ReadArray<FRHIUniformBufferResource>();
			Ar.Read<uint>();
			Ar.Position += 4L;
			Name = Ar.ReadFString();
			Hash = Ar.Read<uint>();
			Ar.Position += 4L;
		}
		else
		{
			ConstantBufferSize = Ar.Read<uint>();
			StaticSlot = Ar.Read<byte>();
			Ar.Position += 3L;
			Resources = Ar.ReadArray<FRHIUniformBufferResource>();
			Ar.Read<uint>();
			Ar.Position += 4L;
			Name = Ar.ReadFString();
			Hash = Ar.Read<uint>();
			Ar.Position += 4L;
		}
	}
}
