using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports.Internationalization;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Core.i18N;

public abstract class FTextHistory : IUStruct
{
	public class None : FTextHistory
	{
		public readonly string? CultureInvariantString;

		public override string Text => CultureInvariantString ?? string.Empty;

		public None()
		{
		}

		public None(FAssetArchive Ar)
		{
			if (FEditorObjectVersion.Get(Ar) >= FEditorObjectVersion.Type.CultureInvariantTextSerializationKeyStability && Ar.ReadBoolean())
			{
				CultureInvariantString = Ar.ReadFString();
			}
		}
	}

	public class Base : FTextHistory
	{
		public readonly string Namespace;

		public readonly string Key;

		public readonly string SourceString;

		public readonly string LocalizedString;

		public override string Text => LocalizedString;

		public Base(FAssetArchive Ar)
		{
			Namespace = Ar.ReadFString() ?? string.Empty;
			Key = Ar.ReadFString() ?? string.Empty;
			SourceString = Ar.ReadFString();
			LocalizedString = Ar.Owner.Provider?.GetLocalizedString(Namespace, Key, SourceString) ?? string.Empty;
		}

		public Base(string namespacee, string key, string sourceString, string localizedString = "")
		{
			Namespace = namespacee;
			Key = key;
			SourceString = sourceString;
			LocalizedString = (string.IsNullOrEmpty(localizedString) ? sourceString : localizedString);
		}
	}

	public class NamedFormat : FTextHistory
	{
		public readonly FText SourceFmt;

		public readonly Dictionary<string, FFormatArgumentValue> Arguments;

		public override string Text => SourceFmt.Text;

		public NamedFormat(FAssetArchive Ar)
		{
			SourceFmt = new FText(Ar);
			Arguments = new Dictionary<string, FFormatArgumentValue>(Ar.Read<int>());
			for (int i = 0; i < Arguments.Count; i++)
			{
				Arguments[Ar.ReadFString()] = new FFormatArgumentValue(Ar);
			}
		}
	}

	public class OrderedFormat : FTextHistory
	{
		public readonly FText SourceFmt;

		public readonly FFormatArgumentValue[] Arguments;

		public override string Text => SourceFmt.Text;

		public OrderedFormat(FAssetArchive Ar)
		{
			SourceFmt = new FText(Ar);
			Arguments = Ar.ReadArray(() => new FFormatArgumentValue(Ar));
		}
	}

	public class ArgumentFormat : FTextHistory
	{
		public readonly FText SourceFmt;

		public readonly FFormatArgumentData[] Arguments;

		public override string Text => SourceFmt.Text;

		public ArgumentFormat(FAssetArchive Ar)
		{
			SourceFmt = new FText(Ar);
			Arguments = Ar.ReadArray(() => new FFormatArgumentData(Ar));
		}
	}

	public class FormatNumber : FTextHistory
	{
		public readonly string? CurrencyCode;

		public readonly FFormatArgumentValue SourceValue;

		public readonly FNumberFormattingOptions? FormatOptions;

		public readonly string TargetCulture;

		public override string Text => SourceValue.Value.ToString();

		public FormatNumber(FAssetArchive Ar, ETextHistoryType historyType)
		{
			if (historyType == ETextHistoryType.AsCurrency && Ar.Ver >= EUnrealEngineObjectUE4Version.ADDED_CURRENCY_CODE_TO_FTEXT)
			{
				CurrencyCode = Ar.ReadFString();
			}
			SourceValue = new FFormatArgumentValue(Ar);
			if (Ar.ReadBoolean())
			{
				FormatOptions = new FNumberFormattingOptions(Ar);
			}
			TargetCulture = Ar.ReadFString();
		}
	}

	public class AsDate : FTextHistory
	{
		public readonly FDateTime SourceDateTime;

		public readonly EDateTimeStyle DateStyle;

		public readonly string? TimeZone;

		public readonly string TargetCulture;

		public override string Text => SourceDateTime.ToString();

		public AsDate(FAssetArchive Ar)
		{
			SourceDateTime = Ar.Read<FDateTime>();
			DateStyle = Ar.Read<EDateTimeStyle>();
			if (Ar.Ver >= EUnrealEngineObjectUE4Version.FTEXT_HISTORY_DATE_TIMEZONE)
			{
				TimeZone = Ar.ReadFString();
			}
			TargetCulture = Ar.ReadFString();
		}
	}

	public class AsTime : FTextHistory
	{
		public readonly FDateTime SourceDateTime;

		public readonly EDateTimeStyle TimeStyle;

		public readonly string TimeZone;

		public readonly string TargetCulture;

		public override string Text => SourceDateTime.ToString();

		public AsTime(FAssetArchive Ar)
		{
			SourceDateTime = Ar.Read<FDateTime>();
			TimeStyle = Ar.Read<EDateTimeStyle>();
			TimeZone = Ar.ReadFString();
			TargetCulture = Ar.ReadFString();
		}
	}

	public class AsDateTime : FTextHistory
	{
		public readonly FDateTime SourceDateTime;

		public readonly EDateTimeStyle DateStyle;

		public readonly EDateTimeStyle TimeStyle;

		public readonly string TimeZone;

		public readonly string TargetCulture;

		public override string Text => SourceDateTime.ToString();

		public AsDateTime(FAssetArchive Ar)
		{
			SourceDateTime = Ar.Read<FDateTime>();
			DateStyle = Ar.Read<EDateTimeStyle>();
			TimeStyle = Ar.Read<EDateTimeStyle>();
			TimeZone = Ar.ReadFString();
			TargetCulture = Ar.ReadFString();
		}
	}

	public class Transform : FTextHistory
	{
		public readonly FText SourceText;

		public readonly ETransformType TransformType;

		public override string Text => SourceText.Text;

		public Transform(FAssetArchive Ar)
		{
			SourceText = new FText(Ar);
			TransformType = Ar.Read<ETransformType>();
		}
	}

	public class StringTableEntry : FTextHistory
	{
		public readonly FName TableId;

		public readonly string Key;

		public override string Text { get; }

		public StringTableEntry(FAssetArchive Ar)
		{
			TableId = Ar.ReadFName();
			Key = Ar.ReadFString();
			if (Ar.Owner.Provider.TryLoadObject(TableId.Text, out UStringTable export) && export.StringTable.KeysToMetaData.TryGetValue(Key, out string value))
			{
				Text = value;
			}
		}
	}

	public class TextGenerator : FTextHistory
	{
		public readonly FName GeneratorTypeID;

		public readonly byte[]? GeneratorContents;

		public override string Text => GeneratorTypeID.Text;

		public TextGenerator(FAssetArchive Ar)
		{
			GeneratorTypeID = Ar.ReadFName();
			_ = GeneratorTypeID.IsNone;
		}
	}

	[JsonIgnore]
	public abstract string Text { get; }
}
