using System;
using System.Runtime.InteropServices;

namespace CUE4Parse.ACL;

public class CompressedTracks
{
	private readonly int _bufferLength;

	public nint Handle { get; private set; }

	public CompressedTracks(byte[] buffer)
	{
		_bufferLength = buffer.Length;
		Handle = ACLNative.nAllocate(_bufferLength);
		Marshal.Copy(buffer, 0, Handle, buffer.Length);
		string text = IsValid(checkHash: false);
		if (text != null)
		{
			ACLNative.nDeallocate(Handle, _bufferLength);
			Handle = IntPtr.Zero;
			throw new ACLException(text);
		}
	}

	public CompressedTracks(nint existing)
	{
		_bufferLength = -1;
		Handle = existing;
	}

	~CompressedTracks()
	{
		if (_bufferLength >= 0 && Handle != IntPtr.Zero)
		{
			ACLNative.nDeallocate(Handle, _bufferLength);
			Handle = IntPtr.Zero;
		}
	}

	public string? IsValid(bool checkHash)
	{
		string text = Marshal.PtrToStringAnsi(nCompressedTracks_IsValid(Handle, checkHash));
		if (text.Length <= 0)
		{
			return null;
		}
		return text;
	}

	public TracksHeader GetTracksHeader()
	{
		return Marshal.PtrToStructure<TracksHeader>(Handle + Marshal.SizeOf<RawBufferHeader>());
	}

	[DllImport("CUE4Parse-Natives")]
	private static extern nint nCompressedTracks_IsValid(nint handle, bool checkHash);
}
