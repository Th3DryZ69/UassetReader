using System.Text;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public abstract class UAnimBoneCompressionCodec : UObject
{
	public virtual UAnimBoneCompressionCodec? GetCodec(string ddcHandle)
	{
		if (!(GetCodecDDCHandle() == ddcHandle))
		{
			return null;
		}
		return this;
	}

	public string GetCodecDDCHandle()
	{
		StringBuilder stringBuilder = new StringBuilder(128);
		stringBuilder.Append(base.Name);
		UObject outer = Outer;
		while (outer != null && !(outer is UAnimBoneCompressionSettings))
		{
			stringBuilder.Append('.');
			stringBuilder.Append(outer.Name);
			outer = outer.Outer;
		}
		return stringBuilder.ToString();
	}

	public abstract ICompressedAnimData AllocateAnimData();
}
