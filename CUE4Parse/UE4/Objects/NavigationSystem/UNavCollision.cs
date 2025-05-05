using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.NavigationSystem;

public class UNavCollision : CUE4Parse.UE4.Assets.Exports.UObject
{
	public class Consts
	{
		public const int Initial = 1;

		public const int AreaClass = 2;

		public const int ConvexTransforms = 3;

		public const int ShapeGeoExport = 4;

		public const int Latest = 4;

		public const uint Magic = 2721575479u;
	}

	public FFormatContainer? CookedFormatData;

	public FPackageIndex AreaClass;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		long position = Ar.Position;
		int num;
		if (Ar.Read<uint>() != 2721575479u)
		{
			num = 1;
			Ar.Position = position;
		}
		else
		{
			num = Ar.Read<int>();
		}
		Ar.Read<FGuid>();
		if (Ar.ReadBoolean())
		{
			CookedFormatData = new FFormatContainer(Ar);
		}
		if (num >= 2)
		{
			AreaClass = new FPackageIndex(Ar);
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		if (CookedFormatData != null)
		{
			writer.WritePropertyName("CookedFormatData");
			serializer.Serialize(writer, CookedFormatData);
		}
		if (!AreaClass.IsNull)
		{
			writer.WritePropertyName("AreaClass");
			serializer.Serialize(writer, AreaClass);
		}
	}
}
