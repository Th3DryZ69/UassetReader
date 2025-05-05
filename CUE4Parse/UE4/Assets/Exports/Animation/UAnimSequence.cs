#define TRACE
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Assets.Exports.Animation.ACL;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.Engine.Animation;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class UAnimSequence : UAnimSequenceBase
{
	public int NumFrames;

	public FTrackToSkeletonMap[] TrackToSkeletonMapTable;

	public FRawAnimSequenceTrack[] RawAnimationData;

	public ResolvedObject? BoneCompressionSettings;

	public ResolvedObject? CurveCompressionSettings;

	public FTrackToSkeletonMap[] CompressedTrackToSkeletonMapTable;

	public FSmartName[] CompressedCurveNames;

	public byte[]? CompressedCurveByteStream;

	public FRawCurveTracks CompressedCurveData;

	public ICompressedAnimData CompressedDataStructure;

	public UAnimBoneCompressionCodec? BoneCompressionCodec;

	public UAnimCurveCompressionCodec? CurveCompressionCodec;

	public int CompressedRawDataSize;

	public EAdditiveAnimationType AdditiveAnimType;

	public EAdditiveBasePoseType RefPoseType;

	public ResolvedObject? RefPoseSeq;

	public int RefFrameIndex;

	public FName RetargetSource;

	public FTransform[]? RetargetSourceAssetReferencePose;

	public bool bUseRawDataOnly;

	public bool EnsuredCurveData;

	private static readonly int[] NumComponentsPerMask = new int[8] { 3, 1, 1, 2, 1, 2, 2, 3 };

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		NumFrames = GetOrDefault("NumFrames", 0);
		BoneCompressionSettings = GetOrDefault<ResolvedObject>("BoneCompressionSettings");
		CurveCompressionSettings = GetOrDefault<ResolvedObject>("CurveCompressionSettings");
		AdditiveAnimType = GetOrDefault("AdditiveAnimType", EAdditiveAnimationType.AAT_None);
		RefPoseType = GetOrDefault("RefPoseType", EAdditiveBasePoseType.ABPT_None);
		RefPoseSeq = GetOrDefault<ResolvedObject>("RefPoseSeq");
		RefFrameIndex = GetOrDefault("RefFrameIndex", 0);
		RetargetSource = GetOrDefault<FName>("RetargetSource");
		RetargetSourceAssetReferencePose = GetOrDefault<FTransform[]>("RetargetSourceAssetReferencePose");
		if (BoneCompressionSettings == null && Ar.Game == EGame.GAME_RogueCompany)
		{
			BoneCompressionSettings = new ResolvedLoadedObject(base.Owner.Provider.LoadObject("/Game/Animation/KSAnimBoneCompressionSettings.KSAnimBoneCompressionSettings"));
		}
		if (!new FStripDataFlags(Ar).IsEditorDataStripped())
		{
			RawAnimationData = Ar.ReadArray(() => new FRawAnimSequenceTrack(Ar));
			if (Ar.Ver >= EUnrealEngineObjectUE4Version.ANIMATION_ADD_TRACKCURVES && FUE5MainStreamObjectVersion.Get(Ar) < FUE5MainStreamObjectVersion.Type.RemovingSourceAnimationData)
			{
				FRawAnimSequenceTrack[] array = Ar.ReadArray(() => new FRawAnimSequenceTrack(Ar));
				if (array.Length != 0)
				{
					RawAnimationData = array;
				}
			}
		}
		if (FFrameworkObjectVersion.Get(Ar) < FFrameworkObjectVersion.Type.MoveCompressedAnimDataToTheDDC)
		{
			FUECompressedAnimData fUECompressedAnimData = (FUECompressedAnimData)(CompressedDataStructure = new FUECompressedAnimData());
			fUECompressedAnimData.CompressedByteStream = Ar.ReadBytes(Ar.Read<int>());
			if (Ar.Game == EGame.GAME_SeaOfThieves && fUECompressedAnimData.CompressedByteStream.Length == 1 && Ar.Length - Ar.Position > 0)
			{
				Ar.Position--;
				fUECompressedAnimData.CompressedByteStream = Ar.ReadBytes(Ar.Read<int>());
			}
			if (fUECompressedAnimData.KeyEncodingFormat == AnimationKeyFormat.AKF_PerTrackCompression && fUECompressedAnimData.CompressedScaleOffsets.OffsetData.Length != 0)
			{
				fUECompressedAnimData.CompressedByteStream = TransferPerTrackData(fUECompressedAnimData.CompressedByteStream);
			}
		}
		else if (Ar.ReadBoolean())
		{
			if (Ar.Game < EGame.GAME_UE4_23)
			{
				SerializeCompressedData(Ar);
			}
			else if (Ar.Game < EGame.GAME_UE4_25)
			{
				SerializeCompressedData2(Ar);
			}
			else
			{
				SerializeCompressedData3(Ar);
			}
			bUseRawDataOnly = Ar.ReadBoolean();
		}
		EnsuredCurveData = EnsureCurveData();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		FTrackToSkeletonMap[] compressedTrackToSkeletonMapTable = CompressedTrackToSkeletonMapTable;
		if (compressedTrackToSkeletonMapTable != null && compressedTrackToSkeletonMapTable.Length > 0)
		{
			writer.WritePropertyName("CompressedTrackToSkeletonMapTable");
			writer.WriteStartArray();
			compressedTrackToSkeletonMapTable = CompressedTrackToSkeletonMapTable;
			for (int i = 0; i < compressedTrackToSkeletonMapTable.Length; i++)
			{
				FTrackToSkeletonMap fTrackToSkeletonMap = compressedTrackToSkeletonMapTable[i];
				writer.WriteValue(fTrackToSkeletonMap.BoneTreeIndex);
			}
			writer.WriteEndArray();
		}
		FSmartName[] compressedCurveNames = CompressedCurveNames;
		if (compressedCurveNames != null && compressedCurveNames.Length > 0)
		{
			writer.WritePropertyName("CompressedCurveNames");
			serializer.Serialize(writer, CompressedCurveNames);
		}
		if (EnsuredCurveData)
		{
			writer.WritePropertyName("CompressedCurveData");
			serializer.Serialize(writer, CompressedCurveData);
		}
		if (CompressedDataStructure != null)
		{
			writer.WritePropertyName("CompressedDataStructure");
			serializer.Serialize(writer, CompressedDataStructure);
		}
		if (BoneCompressionCodec != null)
		{
			ResolvedLoadedObject value = new ResolvedLoadedObject(BoneCompressionCodec);
			writer.WritePropertyName("BoneCompressionCodec");
			serializer.Serialize(writer, value);
		}
		if (CurveCompressionCodec != null)
		{
			ResolvedLoadedObject value2 = new ResolvedLoadedObject(CurveCompressionCodec);
			writer.WritePropertyName("CurveCompressionCodec");
			serializer.Serialize(writer, value2);
		}
		if (CompressedRawDataSize > 0)
		{
			writer.WritePropertyName("CompressedRawDataSize");
			writer.WriteValue(CompressedRawDataSize);
		}
	}

	private void SerializeCompressedData(FAssetArchive Ar)
	{
		FUECompressedAnimData fUECompressedAnimData = (FUECompressedAnimData)(CompressedDataStructure = new FUECompressedAnimData());
		fUECompressedAnimData.KeyEncodingFormat = Ar.Read<AnimationKeyFormat>();
		fUECompressedAnimData.TranslationCompressionFormat = Ar.Read<AnimationCompressionFormat>();
		fUECompressedAnimData.RotationCompressionFormat = Ar.Read<AnimationCompressionFormat>();
		fUECompressedAnimData.ScaleCompressionFormat = Ar.Read<AnimationCompressionFormat>();
		fUECompressedAnimData.CompressedTrackOffsets = Ar.ReadArray<int>();
		fUECompressedAnimData.CompressedScaleOffsets = new FCompressedOffsetData(Ar);
		if (Ar.Game >= EGame.GAME_UE4_21 && Ar.ReadArray<FCompressedSegment>().Length != 0)
		{
			Log.Information("animation has CompressedSegments!");
		}
		CompressedTrackToSkeletonMapTable = Ar.ReadArray<FTrackToSkeletonMap>();
		if (Ar.Game < EGame.GAME_UE4_22)
		{
			CompressedCurveData = new FRawCurveTracks(new FStructFallback(Ar, "RawCurveTracks"));
		}
		else
		{
			CompressedCurveNames = Ar.ReadArray(() => new FSmartName(Ar));
		}
		if (Ar.Versions["AnimSequence.HasCompressedRawSize"])
		{
			CompressedRawDataSize = Ar.Read<int>();
		}
		if (Ar.Game >= EGame.GAME_UE4_22)
		{
			fUECompressedAnimData.CompressedNumberOfFrames = Ar.Read<int>();
		}
		int num = Ar.Read<int>();
		Ar.Position -= 4L;
		if (num >= 0 && num < Ar.Owner.NameMap.Length)
		{
			FName fName = Ar.ReadFName();
			if ("AKF_" + fName.Text != fUECompressedAnimData.KeyEncodingFormat.ToString() && !fName.Text.StartsWith("ACL"))
			{
				Ar.Position -= 8L;
			}
			fUECompressedAnimData.CompressedByteStream = Ar.ReadBytes(Ar.Read<int>());
			if (fName.Text.StartsWith("ACL"))
			{
				CompressedDataStructure = new UAnimBoneCompressionCodec_ACLSafe().AllocateAnimData();
				CompressedDataStructure.Bind(fUECompressedAnimData.CompressedByteStream);
			}
		}
		else
		{
			fUECompressedAnimData.CompressedByteStream = Ar.ReadBytes(Ar.Read<int>());
		}
		if (Ar.Game >= EGame.GAME_UE4_22)
		{
			string path = Ar.ReadFString();
			CurveCompressionCodec = CurveCompressionSettings?.Load<UAnimCurveCompressionSettings>()?.GetCodec(path);
			CompressedCurveByteStream = Ar.ReadBytes(Ar.Read<int>());
		}
		if (fUECompressedAnimData.KeyEncodingFormat == AnimationKeyFormat.AKF_PerTrackCompression && fUECompressedAnimData.CompressedScaleOffsets.OffsetData.Length != 0 && Ar.Game < EGame.GAME_UE4_23)
		{
			fUECompressedAnimData.CompressedByteStream = TransferPerTrackData(fUECompressedAnimData.CompressedByteStream);
		}
	}

	private void SerializeCompressedData2(FAssetArchive Ar)
	{
		CompressedRawDataSize = Ar.Read<int>();
		CompressedTrackToSkeletonMapTable = Ar.ReadArray<FTrackToSkeletonMap>();
		CompressedCurveNames = Ar.ReadArray(() => new FSmartName(Ar));
		FUECompressedAnimData fUECompressedAnimData = (FUECompressedAnimData)(CompressedDataStructure = new FUECompressedAnimData());
		CompressedDataStructure.SerializeCompressedData(Ar);
		byte[] bulkData = ReadSerializedByteStream(Ar);
		fUECompressedAnimData.Bind(bulkData);
		NumFrames = CompressedDataStructure.CompressedNumberOfFrames;
		string path = Ar.ReadFString();
		CurveCompressionCodec = CurveCompressionSettings?.Load<UAnimCurveCompressionSettings>()?.GetCodec(path);
		CompressedCurveByteStream = Ar.ReadBytes(Ar.Read<int>());
	}

	private void SerializeCompressedData3(FAssetArchive Ar)
	{
		CompressedRawDataSize = Ar.Read<int>();
		CompressedTrackToSkeletonMapTable = Ar.ReadArray<FTrackToSkeletonMap>();
		CompressedCurveNames = Ar.ReadArray(() => new FSmartName(Ar));
		byte[] bulkData = ReadSerializedByteStream(Ar);
		string text = Ar.ReadFString();
		string path = Ar.ReadFString();
		int length = Ar.Read<int>();
		CompressedCurveByteStream = Ar.ReadBytes(length);
		BoneCompressionCodec = BoneCompressionSettings?.Load<UAnimBoneCompressionSettings>()?.GetCodec(text);
		CurveCompressionCodec = CurveCompressionSettings?.Load<UAnimCurveCompressionSettings>()?.GetCodec(path);
		if (BoneCompressionCodec != null)
		{
			CompressedDataStructure = BoneCompressionCodec.AllocateAnimData();
			CompressedDataStructure.SerializeCompressedData(Ar);
			CompressedDataStructure.Bind(bulkData);
			NumFrames = CompressedDataStructure.CompressedNumberOfFrames;
		}
		else
		{
			Log.Warning("Unknown bone compression codec {0}", text);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static byte[] ReadSerializedByteStream(FAssetArchive Ar)
	{
		int length = Ar.Read<int>();
		if (Ar.ReadBoolean())
		{
			throw new NotImplementedException("Anim: bUseBulkDataForLoad not implemented");
		}
		return Ar.ReadBytes(length);
	}

	public bool IsValidAdditive()
	{
		if (AdditiveAnimType == EAdditiveAnimationType.AAT_None)
		{
			return false;
		}
		return RefPoseType switch
		{
			EAdditiveBasePoseType.ABPT_RefPose => true, 
			EAdditiveBasePoseType.ABPT_AnimScaled => RefPoseSeq != null && RefPoseSeq.Name.Text != base.Name, 
			EAdditiveBasePoseType.ABPT_AnimFrame => RefPoseSeq != null && RefPoseSeq.Name.Text != base.Name && RefFrameIndex >= 0, 
			EAdditiveBasePoseType.ABPT_LocalAnimFrame => RefFrameIndex >= 0, 
			_ => false, 
		};
	}

	public int GetNumTracks()
	{
		if (CompressedTrackToSkeletonMapTable.Length == 0)
		{
			return TrackToSkeletonMapTable.Length;
		}
		return CompressedTrackToSkeletonMapTable.Length;
	}

	public int GetTrackBoneIndex(int trackIndex)
	{
		if (CompressedTrackToSkeletonMapTable.Length == 0)
		{
			return TrackToSkeletonMapTable[trackIndex].BoneTreeIndex;
		}
		return CompressedTrackToSkeletonMapTable[trackIndex].BoneTreeIndex;
	}

	public int FindTrackForBoneIndex(int boneIndex)
	{
		FTrackToSkeletonMap[] array = ((CompressedTrackToSkeletonMapTable.Length != 0) ? CompressedTrackToSkeletonMapTable : TrackToSkeletonMapTable);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].BoneTreeIndex == boneIndex)
			{
				return i;
			}
		}
		return -1;
	}

	private byte[] TransferPerTrackData(byte[] src)
	{
		byte[] dst = new byte[src.Length];
		FUECompressedAnimData obj = (FUECompressedAnimData)CompressedDataStructure;
		int[] compressedTrackOffsets = obj.CompressedTrackOffsets;
		FCompressedOffsetData compressedScaleOffsets = obj.CompressedScaleOffsets;
		int num = compressedTrackOffsets.Length / 2;
		int srcOffset = 0;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				int num2 = 0;
				num2 = j switch
				{
					0 => compressedTrackOffsets[i * 2], 
					1 => compressedTrackOffsets[i * 2 + 1], 
					_ => compressedScaleOffsets.GetOffsetData(i, 0), 
				};
				if (num2 == -1)
				{
					continue;
				}
				int dstOffset = num2;
				uint num3 = BitConverter.ToUInt32(src, srcOffset);
				Copy(4);
				AnimationCompressionFormat animationCompressionFormat = (AnimationCompressionFormat)(num3 >> 28);
				int num4 = (int)((num3 >> 24) & 0xF);
				int num5 = (int)(num3 & 0xFFFFFF);
				bool flag = (num4 & 8) != 0;
				int num6 = NumComponentsPerMask[num4 & 7];
				if (animationCompressionFormat == AnimationCompressionFormat.ACF_IntervalFixed32NoW)
				{
					Copy(4 * num6 * 2);
				}
				switch (animationCompressionFormat)
				{
				case AnimationCompressionFormat.ACF_Float96NoW:
					Copy(4 * num6 * num5);
					break;
				case AnimationCompressionFormat.ACF_Fixed48NoW:
					Copy(2 * num6 * num5);
					break;
				case AnimationCompressionFormat.ACF_IntervalFixed32NoW:
				case AnimationCompressionFormat.ACF_Fixed32NoW:
				case AnimationCompressionFormat.ACF_Float32NoW:
					Copy(4 * num5);
					break;
				}
				if (flag)
				{
					int num7 = dstOffset;
					int num8 = num7.Align(4);
					if (num8 != num7)
					{
						Copy(num8 - num7);
					}
					Copy(((NumFrames < 256) ? 1 : 2) * num5);
				}
				int num9 = dstOffset;
				int num10 = num9.Align(4);
				Trace.Assert(num10 <= dst.Length);
				if (num10 != num9)
				{
					Copy(num10 - num9);
				}
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				void Copy(int size)
				{
					Buffer.BlockCopy(src, srcOffset, dst, dstOffset, size);
					srcOffset += size;
					dstOffset += size;
				}
			}
		}
		return dst;
	}

	private bool EnsureCurveData()
	{
		if (CompressedCurveData.FloatCurves == null && CurveCompressionCodec != null)
		{
			CompressedCurveData.FloatCurves = CurveCompressionCodec.ConvertCurves(this);
			return true;
		}
		return false;
	}
}
