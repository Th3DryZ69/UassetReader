namespace CUE4Parse.UE4.Assets.Exports.Animation.ACL;

public class UAnimBoneCompressionCodec_ACLDatabase : UAnimBoneCompressionCodec_ACLBase
{
	public override ICompressedAnimData AllocateAnimData()
	{
		return new FACLDatabaseCompressedAnimData
		{
			Codec = this
		};
	}
}
