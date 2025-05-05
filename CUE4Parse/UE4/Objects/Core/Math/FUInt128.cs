using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.Objects.Core.Math;

[StructLayout(LayoutKind.Sequential)]
public class FUInt128 : IUStruct
{
	public ulong Hi;

	public ulong Lo;

	public FUInt128()
		: this(0uL, 0uL)
	{
	}

	public FUInt128(ulong a)
	{
		Hi = 0uL;
		Lo = a;
	}

	public FUInt128(ulong a, ulong b)
	{
		Hi = a;
		Lo = b;
	}

	public FUInt128(uint A, uint B, uint C, uint D)
	{
		Hi = ((ulong)A << 32) | B;
		Lo = ((ulong)C << 32) | D;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetQuadPart(uint part, uint value)
	{
		switch (part)
		{
		case 3u:
			Hi &= 0xFFFFFFFFu | ((ulong)value << 32);
			break;
		case 2u:
			Hi &= (ulong)(-4294967296L | value);
			break;
		case 1u:
			Lo &= 0xFFFFFFFFu | ((ulong)value << 32);
			break;
		case 0u:
			Lo &= (ulong)(-4294967296L | value);
			break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint GetQuadPart(uint part)
	{
		return part switch
		{
			3u => (uint)(Hi >> 32), 
			2u => (uint)Hi, 
			1u => (uint)(Lo >> 32), 
			0u => (uint)Lo, 
			_ => 0u, 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint DivideInternal(uint dividend, uint divisor, ref uint remainder)
	{
		ulong num = ((ulong)remainder << 32) | dividend;
		remainder = (uint)(num % divisor);
		return (uint)(num / divisor);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsGreater(FUInt128 other)
	{
		if (Hi == other.Hi)
		{
			return Lo > other.Lo;
		}
		return Hi > other.Hi;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsLess(FUInt128 other)
	{
		if (Hi == other.Hi)
		{
			return Lo < other.Lo;
		}
		return Hi < other.Hi;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FUInt128 Divide(uint divisor, out uint remainder)
	{
		remainder = 0u;
		SetQuadPart(3u, DivideInternal(GetQuadPart(3u), divisor, ref remainder));
		SetQuadPart(2u, DivideInternal(GetQuadPart(2u), divisor, ref remainder));
		SetQuadPart(1u, DivideInternal(GetQuadPart(1u), divisor, ref remainder));
		SetQuadPart(0u, DivideInternal(GetQuadPart(0u), divisor, ref remainder));
		return this;
	}

	public override string ToString()
	{
		return $"Hi={Hi} Lo={Lo}";
	}
}
