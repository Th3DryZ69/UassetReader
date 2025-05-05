using System;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

[Flags]
public enum ELightMapFlags
{
	LMF_None = 0,
	LMF_Streamed = 1,
	LMF_LQLightmap = 2
}
