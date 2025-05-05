using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

[JsonConverter(typeof(FPackageIndexConverter))]
public class FPackageIndex
{
	public readonly int Index;

	public readonly IPackage? Owner;

	public ResolvedObject? ResolvedObject => Owner?.ResolvePackageIndex(this);

	public bool IsNull => Index == 0;

	public bool IsExport => Index > 0;

	public bool IsImport => Index < 0;

	public string Name => ResolvedObject?.Name.Text ?? "None";

	public FPackageIndex(FAssetArchive Ar, int index)
	{
		Index = index;
		Owner = Ar.Owner;
	}

	public FPackageIndex(FAssetArchive Ar)
	{
		Index = Ar.Read<int>();
		Owner = Ar.Owner;
	}

	public FPackageIndex(FKismetArchive Ar)
	{
		Index = Ar.Read<int>();
		Owner = Ar.Owner;
		Ar.Index += 4;
	}

	public FPackageIndex()
	{
		Index = 0;
		Owner = null;
	}

	public override string ToString()
	{
		return ResolvedObject?.ToString() ?? Index.ToString();
	}

	protected internal void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		if (TryLoad(out UProperty export))
		{
			serializer.Serialize(writer, export);
		}
		else
		{
			serializer.Serialize(writer, this);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T? Load<T>() where T : CUE4Parse.UE4.Assets.Exports.UObject
	{
		return Owner?.FindObject(this)?.Value as T;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryLoad<T>(out T export) where T : CUE4Parse.UE4.Assets.Exports.UObject
	{
		if (!TryLoad(out CUE4Parse.UE4.Assets.Exports.UObject export2) || !(export2 is T val))
		{
			export = null;
			return false;
		}
		export = val;
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<T> LoadAsync<T>() where T : CUE4Parse.UE4.Assets.Exports.UObject
	{
		return ((await LoadAsync()) as T) ?? throw new ParserException("Loaded " + ToString() + " but it was of wrong type");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<T?> TryLoadAsync<T>() where T : CUE4Parse.UE4.Assets.Exports.UObject
	{
		return (await TryLoadAsync()) as T;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public CUE4Parse.UE4.Assets.Exports.UObject? Load()
	{
		return ResolvedObject?.Load();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryLoad(out CUE4Parse.UE4.Assets.Exports.UObject? export)
	{
		if (ResolvedObject != null)
		{
			return ResolvedObject.TryLoad(out export);
		}
		export = null;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<CUE4Parse.UE4.Assets.Exports.UObject> LoadAsync()
	{
		if (ResolvedObject != null)
		{
			return await ResolvedObject.LoadAsync();
		}
		throw new ParserException(ToString() + " could not be loaded");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<CUE4Parse.UE4.Assets.Exports.UObject?> TryLoadAsync()
	{
		if (ResolvedObject != null)
		{
			CUE4Parse.UE4.Assets.Exports.UObject uObject = await ResolvedObject.TryLoadAsync();
			if (uObject != null)
			{
				return uObject;
			}
		}
		return null;
	}
}
