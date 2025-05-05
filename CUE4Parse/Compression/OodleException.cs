using System;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.Compression;

public class OodleException : ParserException
{
	public OodleException(string? message = null, Exception? innerException = null)
		: base(message, innerException)
	{
	}

	public OodleException(FArchive reader, string? message = null, Exception? innerException = null)
		: base(reader, message, innerException)
	{
	}
}
