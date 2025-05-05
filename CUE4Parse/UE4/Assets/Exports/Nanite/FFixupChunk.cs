namespace CUE4Parse.UE4.Assets.Exports.Nanite;

public readonly struct FFixupChunk
{
	public readonly struct FHeader
	{
		public readonly ushort NumClusters;

		public readonly ushort NumHierachyFixups;

		public readonly ushort NumClusterFixups;

		public readonly ushort Pad;
	}

	public readonly FHeader Header;
}
