using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Objects.PhysicsEngine;

public class UPhysicsAsset : CUE4Parse.UE4.Assets.Exports.UObject
{
	public Dictionary<FRigidBodyIndexPair, bool>? CollisionDisableTable;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		int num = Ar.Read<int>();
		CollisionDisableTable = new Dictionary<FRigidBodyIndexPair, bool>(num);
		for (int i = 0; i < num; i++)
		{
			FRigidBodyIndexPair key = new FRigidBodyIndexPair(Ar);
			CollisionDisableTable[key] = Ar.ReadBoolean();
		}
	}
}
