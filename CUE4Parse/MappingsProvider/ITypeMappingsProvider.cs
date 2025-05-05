namespace CUE4Parse.MappingsProvider;

public interface ITypeMappingsProvider
{
	TypeMappings? MappingsForGame { get; }

	void Load(string path);

	void Load(byte[] bytes);

	void Reload();
}
