using Serilog;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterial
{
	public FMaterialShaderMap LoadedShaderMap;

	public void DeserializeInlineShaderMap(FMaterialResourceProxyReader Ar)
	{
		if (Ar.ReadBoolean())
		{
			if (Ar.ReadBoolean())
			{
				LoadedShaderMap = new FMaterialShaderMap();
				LoadedShaderMap.Deserialize(Ar);
			}
			else
			{
				Log.Warning("Loading a material resource '{0}' with an invalid ShaderMap!", Ar.Name);
			}
		}
	}
}
