using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Objects.Core.i18N;

public class FFormatArgumentData : IUStruct
{
	public string ArgumentName;

	public FFormatArgumentValue ArgumentValue;

	public FFormatArgumentData(FAssetArchive Ar)
	{
		ArgumentName = Ar.ReadFString();
		ArgumentValue = new FFormatArgumentValue(Ar);
	}
}
