using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Objects.Engine;

public readonly struct FBspSurf : IUStruct
{
	public readonly FPackageIndex Material;

	public readonly uint PolyFlags;

	public readonly int pBase;

	public readonly int vNormal;

	public readonly int vTextureU;

	public readonly int vTextureV;

	public readonly int iBrushPoly;

	public readonly FPackageIndex Actor;

	public readonly FPlane Plane;

	public readonly float LightMapScale;

	public readonly int iLightmassIndex;

	public FBspSurf(FAssetArchive Ar)
	{
		Material = new FPackageIndex(Ar);
		PolyFlags = Ar.Read<uint>();
		pBase = Ar.Read<int>();
		vNormal = Ar.Read<int>();
		vTextureU = Ar.Read<int>();
		vTextureV = Ar.Read<int>();
		iBrushPoly = Ar.Read<int>();
		Actor = new FPackageIndex(Ar);
		Plane = Ar.Read<FPlane>();
		LightMapScale = Ar.Read<float>();
		iLightmassIndex = Ar.Read<int>();
	}
}
