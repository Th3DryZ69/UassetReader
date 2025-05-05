using System;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FPlatformTypeLayoutParameters
{
	[Flags]
	public enum EFlags
	{
		Flag_Initialized = 1,
		Flag_Is32Bit = 2,
		Flag_AlignBases = 4,
		Flag_WithEditorOnly = 8,
		Flag_WithRaytracing = 0x10
	}

	public uint MaxFieldAlignment;

	public EFlags Flags;

	public FPlatformTypeLayoutParameters()
	{
		MaxFieldAlignment = uint.MaxValue;
	}

	public FPlatformTypeLayoutParameters(FArchive Ar)
	{
		MaxFieldAlignment = Ar.Read<uint>();
		Flags = Ar.Read<EFlags>();
	}
}
