using System;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.IO.Objects;

public class FIoStatusException : ParserException
{
	public readonly FIoStatus Status;

	public FIoStatusException(FIoStatus status, Exception? innerException = null)
		: base(status.ToString(), innerException)
	{
		Status = status;
	}

	public FIoStatusException(EIoErrorCode errorCode, string errorMessage = "", Exception? innerException = null)
		: this(new FIoStatus(errorCode, errorMessage), innerException)
	{
	}

	public FIoStatusException(FArchive Ar, FIoStatus status, Exception? innerException = null)
		: base(Ar, status.ToString(), innerException)
	{
		Status = status;
	}

	public FIoStatusException(FArchive Ar, EIoErrorCode errorCode, string errorMessage = "", Exception? innerException = null)
		: this(Ar, new FIoStatus(errorCode, errorMessage), innerException)
	{
	}
}
