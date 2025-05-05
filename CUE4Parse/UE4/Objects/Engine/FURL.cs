using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Engine;

public class FURL
{
	public string Protocol;

	public string Host;

	public int Port;

	public bool Valid;

	public string Map;

	public string[] Op;

	public string Portal;

	public FURL(FArchive Ar)
	{
		Protocol = Ar.ReadFString();
		Host = Ar.ReadFString();
		Map = Ar.ReadFString();
		Portal = Ar.ReadFString();
		Op = Ar.ReadArray(Ar.ReadFString);
		Port = Ar.Read<int>();
		Valid = Ar.ReadBoolean();
	}
}
