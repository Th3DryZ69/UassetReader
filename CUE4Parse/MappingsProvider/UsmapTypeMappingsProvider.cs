using CUE4Parse.MappingsProvider.Usmap;

namespace CUE4Parse.MappingsProvider;

public abstract class UsmapTypeMappingsProvider : AbstractTypeMappingsProvider
{
	public override TypeMappings? MappingsForGame { get; protected set; } = new TypeMappings();

	public override void Load(string path)
	{
		UsmapParser usmapParser = new UsmapParser(path);
		MappingsForGame = usmapParser.Mappings;
	}

	public override void Load(byte[] bytes)
	{
		UsmapParser usmapParser = new UsmapParser(bytes);
		MappingsForGame = usmapParser.Mappings;
	}
}
