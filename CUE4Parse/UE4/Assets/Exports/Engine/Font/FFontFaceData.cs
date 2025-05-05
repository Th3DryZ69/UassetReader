using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Engine.Font;

public class FFontFaceData
{
	public byte[] Data;

	public FFontFaceData(FArchive Ar)
	{
		Data = Ar.ReadArray<byte>();
	}
}
