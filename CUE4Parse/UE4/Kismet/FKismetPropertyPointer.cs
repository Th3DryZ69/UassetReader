using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

[JsonConverter(typeof(FKismetPropertyPointerConverter))]
public class FKismetPropertyPointer
{
	public FPackageIndex? Old;

	public FFieldPath? New;

	public bool bNew { get; } = true;

	public FKismetPropertyPointer(FKismetArchive Ar)
	{
		if (Ar.Game >= EGame.GAME_UE4_25)
		{
			New = new FFieldPath(Ar);
			return;
		}
		bNew = false;
		Old = new FPackageIndex(Ar);
	}
}
