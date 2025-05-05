using System;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine.Curves;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CUE4Parse.UE4.Objects.MovieScene;

public readonly struct FMovieSceneChannel<T> : IUStruct
{
	[JsonConverter(typeof(StringEnumConverter))]
	public readonly ERichCurveExtrapolation PreInfinityExtrap;

	[JsonConverter(typeof(StringEnumConverter))]
	public readonly ERichCurveExtrapolation PostInfinityExtrap;

	public readonly FFrameNumber[] Times;

	public readonly FMovieSceneValue<T>[] Values;

	public readonly T? DefaultValue;

	public readonly bool bHasDefaultValue;

	public readonly FFrameRate TickResolution;

	public readonly bool bShowCurve;

	public FMovieSceneChannel(FAssetArchive Ar)
	{
		if (FSequencerObjectVersion.Get(Ar) < FSequencerObjectVersion.Type.SerializeFloatChannelCompletely)
		{
			PreInfinityExtrap = ERichCurveExtrapolation.RCCE_None;
			PostInfinityExtrap = ERichCurveExtrapolation.RCCE_None;
			Times = Array.Empty<FFrameNumber>();
			Values = Array.Empty<FMovieSceneValue<T>>();
			DefaultValue = default(T);
			bHasDefaultValue = false;
			TickResolution = default(FFrameRate);
			bShowCurve = false;
			return;
		}
		PreInfinityExtrap = Ar.Read<ERichCurveExtrapolation>();
		PostInfinityExtrap = Ar.Read<ERichCurveExtrapolation>();
		int num = Unsafe.SizeOf<FFrameNumber>();
		int num2 = Ar.Read<int>();
		if (num2 == num)
		{
			Times = Ar.ReadArray<FFrameNumber>();
		}
		else
		{
			int num3 = Ar.Read<int>();
			if (num3 > 0)
			{
				int num4 = num2 - num;
				Times = new FFrameNumber[num3];
				for (int i = 0; i < num3; i++)
				{
					Ar.Position += num4;
					Times[i] = Ar.Read<FFrameNumber>();
				}
			}
			else
			{
				Times = Array.Empty<FFrameNumber>();
			}
		}
		num = Unsafe.SizeOf<FMovieSceneValue<T>>();
		num2 = Ar.Read<int>();
		if (num2 == num)
		{
			Values = Ar.ReadArray<FMovieSceneValue<T>>();
		}
		else
		{
			int num5 = Ar.Read<int>();
			if (num5 > 0)
			{
				int num6 = num2 - num;
				Values = new FMovieSceneValue<T>[num5];
				for (int j = 0; j < num5; j++)
				{
					Ar.Position += num6;
					Values[j] = Ar.Read<FMovieSceneValue<T>>();
				}
			}
			else
			{
				Values = Array.Empty<FMovieSceneValue<T>>();
			}
		}
		DefaultValue = Ar.Read<T>();
		bHasDefaultValue = Ar.ReadBoolean();
		TickResolution = Ar.Read<FFrameRate>();
		bShowCurve = FFortniteMainBranchObjectVersion.Get(Ar) >= FFortniteMainBranchObjectVersion.Type.SerializeFloatChannelShowCurve && Ar.ReadBoolean();
	}
}
