using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Core.i18N;

public class FFormatArgumentValue : IUStruct
{
	public EFormatArgumentType Type;

	public object Value;

	public FFormatArgumentValue(FAssetArchive Ar)
	{
		Type = Ar.Read<EFormatArgumentType>();
		Value = Type switch
		{
			EFormatArgumentType.Text => new FText(Ar), 
			EFormatArgumentType.Int => (Ar.Game == EGame.GAME_HogwartsLegacy) ? Ar.Read<int>() : Ar.Read<long>(), 
			EFormatArgumentType.UInt => Ar.Read<ulong>(), 
			EFormatArgumentType.Double => Ar.Read<double>(), 
			EFormatArgumentType.Float => Ar.Read<float>(), 
			_ => throw new ParserException(Ar, $"{Type} argument not supported yet"), 
		};
	}
}
