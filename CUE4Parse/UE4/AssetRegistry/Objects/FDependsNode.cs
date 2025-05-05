using System.Collections;
using System.Collections.Generic;
using CUE4Parse.UE4.AssetRegistry.Readers;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

[JsonConverter(typeof(FDependsNodeConverter))]
public class FDependsNode
{
	private const int PackageFlagWidth = 3;

	private const int PackageFlagSetWidth = 5;

	private const int ManageFlagWidth = 1;

	private const int ManageFlagSetWidth = 1;

	public FAssetIdentifier Identifier;

	public List<FDependsNode> PackageDependencies;

	public List<FDependsNode> NameDependencies;

	public List<FDependsNode> ManageDependencies;

	public List<FDependsNode> Referencers;

	public BitArray? PackageFlags;

	public BitArray? ManageFlags;

	internal int _index;

	public FDependsNode(int index)
	{
		_index = index;
	}

	public void SerializeLoad(FAssetRegistryArchive Ar, FDependsNode[] preallocatedDependsNodeDataBuffer)
	{
		Identifier = new FAssetIdentifier(Ar);
		ReadDependencies(ref PackageDependencies, ref PackageFlags, 5);
		ReadDependenciesNoFlags(ref NameDependencies);
		ReadDependencies(ref ManageDependencies, ref ManageFlags, 1);
		ReadDependenciesNoFlags(ref Referencers);
		void ReadDependencies(ref List<FDependsNode> outDependencies, ref BitArray? outFlagBits, int flagSetWidth)
		{
			List<int> list = new List<int>();
			List<FDependsNode> pointerDependencies = new List<FDependsNode>();
			int[] array = Ar.ReadArray<int>();
			int num = array.Length;
			int num2 = flagSetWidth * num;
			int num3 = num2.DivideAndRoundUp(32);
			BitArray readBits = ((num3 != 0) ? new BitArray(Ar.ReadArray<int>(num3)) : new BitArray(0));
			int[] array2 = array;
			foreach (int num4 in array2)
			{
				if (num4 < 0 || preallocatedDependsNodeDataBuffer.Length <= num4)
				{
					throw new ParserException($"Index {num4} doesn't exist in 'PreallocatedDependsNodeDataBuffers'");
				}
				FDependsNode item = preallocatedDependsNodeDataBuffer[num4];
				pointerDependencies.Add(item);
			}
			for (int j = 0; j < num; j++)
			{
				list.Add(j);
			}
			list.Sort((int a, int b) => pointerDependencies[a]._index - pointerDependencies[b]._index);
			outDependencies = new List<FDependsNode>(num);
			foreach (int item2 in list)
			{
				outDependencies.Add(pointerDependencies[item2]);
			}
			outFlagBits = new BitArray(num2);
			for (int num5 = 0; num5 < num; num5++)
			{
				int num6 = list[num5];
				outFlagBits.SetRangeFromRange(num5 * flagSetWidth, flagSetWidth, readBits, num6 * flagSetWidth);
			}
		}
		void ReadDependenciesNoFlags(ref List<FDependsNode> outDependencies)
		{
			List<int> list = new List<int>();
			List<FDependsNode> pointerDependencies = new List<FDependsNode>();
			int[] array = Ar.ReadArray<int>();
			int num = array.Length;
			int[] array2 = array;
			foreach (int num2 in array2)
			{
				if (num2 < 0 || preallocatedDependsNodeDataBuffer.Length <= num2)
				{
					throw new ParserException($"Index {num2} doesn't exist in 'PreallocatedDependsNodeDataBuffers'");
				}
				FDependsNode item = preallocatedDependsNodeDataBuffer[num2];
				pointerDependencies.Add(item);
			}
			for (int j = 0; j < num; j++)
			{
				list.Add(j);
			}
			list.Sort((int a, int b) => pointerDependencies[a]._index - pointerDependencies[b]._index);
			outDependencies = new List<FDependsNode>(num);
			foreach (int item3 in list)
			{
				outDependencies.Add(pointerDependencies[item3]);
			}
		}
	}

	public void SerializeLoad_BeforeFlags(FAssetRegistryArchive Ar, FDependsNode[] preallocatedDependsNodeDataBuffer)
	{
		Identifier = new FAssetIdentifier(Ar);
		int num = Ar.Read<int>();
		int num2 = Ar.Read<int>();
		int num3 = Ar.Read<int>();
		int num4 = Ar.Read<int>();
		int num5 = ((Ar.Header.Version >= FAssetRegistryVersionType.AddedHardManage) ? Ar.Read<int>() : 0);
		int num6 = Ar.Read<int>();
		PackageDependencies = new List<FDependsNode>(num + num2);
		NameDependencies = new List<FDependsNode>(num3);
		ManageDependencies = new List<FDependsNode>(num4 + num5);
		Referencers = new List<FDependsNode>(num6);
		SerializeNodeArray(num, ref PackageDependencies);
		SerializeNodeArray(num2, ref PackageDependencies);
		SerializeNodeArray(num3, ref NameDependencies);
		SerializeNodeArray(num4, ref ManageDependencies);
		SerializeNodeArray(num5, ref ManageDependencies);
		SerializeNodeArray(num6, ref Referencers);
		void SerializeNodeArray(int num7, ref List<FDependsNode> outNodes)
		{
			for (int i = 0; i < num7; i++)
			{
				int num8 = Ar.Read<int>();
				if (num8 < 0 || num8 >= preallocatedDependsNodeDataBuffer.Length)
				{
					throw new ParserException($"Index {num8} doesn't exist in 'PreallocatedDependsNodeDataBuffers'");
				}
				FDependsNode item = preallocatedDependsNodeDataBuffer[num8];
				outNodes.Add(item);
			}
		}
	}
}
