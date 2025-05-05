using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.i18N;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Localization;

[JsonConverter(typeof(FTextLocalizationMetaDataResourceConverter))]
public class FTextLocalizationMetaDataResource
{
	private readonly FGuid _locMetaMagic = new FGuid(2706173519u, 2203404392u, 3175500908u, 2085673584u);

	public readonly string NativeCulture;

	public readonly string NativeLocRes;

	public readonly string[]? CompiledCultures;

	public FTextLocalizationMetaDataResource(FArchive Ar)
	{
		ELocMetaVersion eLocMetaVersion = ELocMetaVersion.Initial;
		if (Ar.Read<FGuid>() == _locMetaMagic)
		{
			eLocMetaVersion = Ar.Read<ELocMetaVersion>();
		}
		else
		{
			Ar.Position = 0L;
			Log.Warning("LocMeta '" + Ar.Name + "' failed the magic number check!");
		}
		if ((int)eLocMetaVersion > 1)
		{
			throw new ParserException(Ar, $"LocMeta '{Ar.Name}' is too new to be loaded (File Version: {eLocMetaVersion:D}, Loader Version: {3:D})");
		}
		NativeCulture = Ar.ReadFString();
		NativeLocRes = Ar.ReadFString();
		if ((int)eLocMetaVersion >= 1)
		{
			CompiledCultures = Ar.ReadArray(Ar.ReadFString);
		}
		else
		{
			CompiledCultures = null;
		}
	}
}
