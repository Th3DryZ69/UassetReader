namespace CUE4Parse.UE4.Assets.Exports.Animation;

public abstract class UAnimCurveCompressionCodec : UObject
{
	public virtual UAnimCurveCompressionCodec? GetCodec(string path)
	{
		return this;
	}

	public abstract FFloatCurve[] ConvertCurves(UAnimSequence animSeq);
}
