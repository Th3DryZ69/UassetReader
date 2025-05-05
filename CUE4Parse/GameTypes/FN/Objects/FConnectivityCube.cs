using System.Collections;
using CUE4Parse.UE4;
using CUE4Parse.UE4.Readers;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.GameTypes.FN.Objects;

[JsonConverter(typeof(FConnectivityCubeConverter))]
public class FConnectivityCube : IUStruct
{
	public readonly BitArray[] Faces = new BitArray[6];

	public FConnectivityCube(FArchive Ar)
	{
		for (int i = 0; i < Faces.Length; i++)
		{
			int num = Ar.Read<int>();
			int length = num.DivideAndRoundUp(32);
			int[] values = Ar.ReadArray<int>(length);
			Faces[i] = new BitArray(values)
			{
				Length = num
			};
		}
	}
}
