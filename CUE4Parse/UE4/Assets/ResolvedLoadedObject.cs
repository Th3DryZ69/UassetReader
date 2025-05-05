using System;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets;

public class ResolvedLoadedObject : ResolvedObject
{
	private readonly UObject _object;

	public override FName Name => new FName(_object.Name);

	public override ResolvedObject? Outer
	{
		get
		{
			UObject outer = _object.Outer;
			if (outer == null)
			{
				return null;
			}
			return new ResolvedLoadedObject(outer);
		}
	}

	public override ResolvedObject? Class
	{
		get
		{
			UStruct uStruct = _object.Class;
			if (uStruct == null)
			{
				return null;
			}
			return new ResolvedLoadedObject(uStruct);
		}
	}

	public override ResolvedObject? Super => null;

	public override Lazy<UObject> Object => new Lazy<UObject>(() => _object);

	public ResolvedLoadedObject(UObject obj)
		: base(obj.Owner)
	{
		_object = obj;
	}
}
