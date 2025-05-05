namespace CUE4Parse.MappingsProvider.Usmap;

public enum EPropertyType : byte
{
	ByteProperty = 0,
	BoolProperty = 1,
	IntProperty = 2,
	FloatProperty = 3,
	ObjectProperty = 4,
	NameProperty = 5,
	DelegateProperty = 6,
	DoubleProperty = 7,
	ArrayProperty = 8,
	StructProperty = 9,
	StrProperty = 10,
	TextProperty = 11,
	InterfaceProperty = 12,
	MulticastDelegateProperty = 13,
	WeakObjectProperty = 14,
	LazyObjectProperty = 15,
	AssetObjectProperty = 16,
	SoftObjectProperty = 17,
	UInt64Property = 18,
	UInt32Property = 19,
	UInt16Property = 20,
	Int64Property = 21,
	Int16Property = 22,
	Int8Property = 23,
	MapProperty = 24,
	SetProperty = 25,
	EnumProperty = 26,
	FieldPathProperty = 27,
	Unknown = byte.MaxValue
}
