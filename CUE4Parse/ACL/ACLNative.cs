using System.Runtime.InteropServices;

namespace CUE4Parse.ACL;

public static class ACLNative
{
	public const string LIB_NAME = "CUE4Parse-Natives";

	[DllImport("CUE4Parse-Natives")]
	public static extern nint nAllocate(int size, int alignment = 16);

	[DllImport("CUE4Parse-Natives")]
	public static extern void nDeallocate(nint ptr, int size);
}
