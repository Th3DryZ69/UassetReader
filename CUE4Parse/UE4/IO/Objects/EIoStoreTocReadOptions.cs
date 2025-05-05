using System;

namespace CUE4Parse.UE4.IO.Objects;

[Flags]
public enum EIoStoreTocReadOptions
{
	Default = 0,
	ReadDirectoryIndex = 1,
	ReadTocMeta = 2,
	ReadAll = 3
}
