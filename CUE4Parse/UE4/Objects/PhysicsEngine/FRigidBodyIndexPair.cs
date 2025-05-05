using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.PhysicsEngine;

public class FRigidBodyIndexPair
{
	public readonly int[] Indices = new int[2];

	public FRigidBodyIndexPair(FArchive Ar)
	{
		Indices[0] = Ar.Read<int>();
		Indices[1] = Ar.Read<int>();
	}
}
