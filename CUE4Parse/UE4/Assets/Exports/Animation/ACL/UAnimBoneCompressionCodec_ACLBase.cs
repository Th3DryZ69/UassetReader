namespace CUE4Parse.UE4.Assets.Exports.Animation.ACL;

public abstract class UAnimBoneCompressionCodec_ACLBase : UAnimBoneCompressionCodec
{
	public override ICompressedAnimData AllocateAnimData()
	{
		return new FACLCompressedAnimData();
	}
}
