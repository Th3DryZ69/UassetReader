using System.Runtime.CompilerServices;

namespace CUE4Parse.UE4.IO.Objects;

public class FIoStatus
{
	public readonly EIoErrorCode ErrorCode;

	public readonly string ErrorMessage;

	public FIoStatus(EIoErrorCode errorCode, string errorMessage)
	{
		ErrorCode = errorCode;
		ErrorMessage = errorMessage;
	}

	public override string ToString()
	{
		return $"{ErrorMessage} ({ErrorCode})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FIoStatusException ToException()
	{
		return new FIoStatusException(this);
	}
}
