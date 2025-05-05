using System.Runtime.CompilerServices;

namespace CUE4Parse.ACL;

public struct TracksHeader
{
	public uint Tag;

	public ushort Version;

	public byte AlgorithmType;

	public byte TrackType;

	public uint NumTracks;

	public uint NumSamples;

	public float SampleRate;

	public uint MiscPacked;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool GetHasScale()
	{
		return (MiscPacked & 1) != 0;
	}
}
