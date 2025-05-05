using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine.Curves;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Assets.Exports.Engine;

public class UCurveTable : UObject
{
	public Dictionary<FName, FStructFallback> RowMap { get; private set; }

	public ECurveTableMode CurveTableMode { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		int num = Ar.Read<int>();
		if (FFortniteMainBranchObjectVersion.Get(Ar) < FFortniteMainBranchObjectVersion.Type.ShrinkCurveTableSize)
		{
			CurveTableMode = ((num > 0) ? ECurveTableMode.RichCurves : ECurveTableMode.Empty);
		}
		else
		{
			CurveTableMode = Ar.Read<ECurveTableMode>();
		}
		RowMap = new Dictionary<FName, FStructFallback>(num);
		for (int i = 0; i < num; i++)
		{
			FName key = Ar.ReadFName();
			ECurveTableMode curveTableMode = CurveTableMode;
			RowMap[key] = new FStructFallback(Ar, curveTableMode switch
			{
				ECurveTableMode.SimpleCurves => "SimpleCurve", 
				ECurveTableMode.RichCurves => "RichCurve", 
				_ => "", 
			});
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("Rows");
		serializer.Serialize(writer, RowMap);
	}

	public FRealCurve? FindCurve(FName rowName, bool bWarnIfNotFound = true)
	{
		if (rowName.IsNone)
		{
			if (bWarnIfNotFound)
			{
				Log.Warning("UCurveTable::FindCurve : NAME_None is invalid row name for CurveTable '{0}'.", GetPathName());
			}
			return null;
		}
		if (!RowMap.TryGetValue(rowName, out FStructFallback value))
		{
			if (bWarnIfNotFound)
			{
				Log.Warning("UCurveTable::FindCurve : Row '{0}' not found in CurveTable '{1}'.", rowName.ToString(), GetPathName());
			}
			return null;
		}
		return CurveTableMode switch
		{
			ECurveTableMode.SimpleCurves => new FSimpleCurve(value), 
			ECurveTableMode.RichCurves => new FRichCurve(value), 
			_ => null, 
		};
	}

	public bool TryFindCurve(FName rowName, out FRealCurve outCurve, bool bWarnIfNotFound = true)
	{
		if (rowName.IsNone)
		{
			if (bWarnIfNotFound)
			{
				Log.Warning("UCurveTable::FindCurve : NAME_None is invalid row name for CurveTable '{0}'.", GetPathName());
			}
			outCurve = null;
			return false;
		}
		if (!RowMap.TryGetValue(rowName, out FStructFallback value))
		{
			if (bWarnIfNotFound)
			{
				Log.Warning("UCurveTable::FindCurve : Row '{0}' not found in CurveTable '{1}'.", rowName.ToString(), GetPathName());
			}
			outCurve = null;
			return false;
		}
		switch (CurveTableMode)
		{
		case ECurveTableMode.SimpleCurves:
			outCurve = new FSimpleCurve(value);
			return true;
		case ECurveTableMode.RichCurves:
			outCurve = new FRichCurve(value);
			return true;
		default:
			outCurve = null;
			return false;
		}
	}
}
