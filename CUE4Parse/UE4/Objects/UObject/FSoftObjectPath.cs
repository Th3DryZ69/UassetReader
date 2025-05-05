using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

[JsonConverter(typeof(FSoftObjectPathConverter))]
public readonly struct FSoftObjectPath : IUStruct
{
	public readonly FName AssetPathName;

	public readonly string SubPathString;

	public readonly IPackage? Owner;

	public FSoftObjectPath(FAssetArchive Ar)
	{
		if (Ar.Ver < EUnrealEngineObjectUE4Version.ADDED_SOFT_OBJECT_PATH)
		{
			string text = Ar.ReadFString();
			throw new ParserException(Ar, "Asset path \"" + text + "\" is in short form and is not supported, nor recommended");
		}
		AssetPathName = ((Ar.Ver >= EUnrealEngineObjectUE5Version.FSOFTOBJECTPATH_REMOVE_ASSET_PATH_FNAMES) ? new FName(new FTopLevelAssetPath(Ar).ToString()) : Ar.ReadFName());
		SubPathString = Ar.ReadFString();
		Owner = Ar.Owner;
	}

	public FSoftObjectPath(FName assetPathName, string subPathString, IPackage? owner = null)
	{
		AssetPathName = assetPathName;
		SubPathString = subPathString;
		Owner = owner;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public CUE4Parse.UE4.Assets.Exports.UObject Load()
	{
		return Load(Owner?.Provider ?? throw new ParserException("Package was loaded without a IFileProvider"));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryLoad(out CUE4Parse.UE4.Assets.Exports.UObject export)
	{
		IFileProvider fileProvider = Owner?.Provider;
		if (fileProvider == null)
		{
			export = null;
			return false;
		}
		return TryLoad(fileProvider, out export);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T Load<T>() where T : CUE4Parse.UE4.Assets.Exports.UObject
	{
		return Load<T>(Owner?.Provider ?? throw new ParserException("Package was loaded without a IFileProvider"));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryLoad<T>(out T export) where T : CUE4Parse.UE4.Assets.Exports.UObject
	{
		IFileProvider fileProvider = Owner?.Provider;
		if (fileProvider == null)
		{
			export = null;
			return false;
		}
		return TryLoad(fileProvider, out export);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<CUE4Parse.UE4.Assets.Exports.UObject> LoadAsync()
	{
		return await LoadAsync(Owner?.Provider ?? throw new ParserException("Package was loaded without a IFileProvider"));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<CUE4Parse.UE4.Assets.Exports.UObject?> TryLoadAsync()
	{
		IFileProvider fileProvider = Owner?.Provider;
		if (fileProvider == null)
		{
			return null;
		}
		return await TryLoadAsync(fileProvider).ConfigureAwait(continueOnCapturedContext: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<T> LoadAsync<T>() where T : CUE4Parse.UE4.Assets.Exports.UObject
	{
		return await LoadAsync<T>(Owner?.Provider ?? throw new ParserException("Package was loaded without a IFileProvider"));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<T?> TryLoadAsync<T>() where T : CUE4Parse.UE4.Assets.Exports.UObject
	{
		IFileProvider fileProvider = Owner?.Provider;
		if (fileProvider == null)
		{
			return null;
		}
		return await TryLoadAsync<T>(fileProvider).ConfigureAwait(continueOnCapturedContext: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T Load<T>(IFileProvider provider) where T : CUE4Parse.UE4.Assets.Exports.UObject
	{
		return (Load(provider) as T) ?? throw new ParserException("Loaded SoftObjectProperty but it was of wrong type");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryLoad<T>(IFileProvider provider, out T export) where T : CUE4Parse.UE4.Assets.Exports.UObject
	{
		if (!TryLoad(provider, out CUE4Parse.UE4.Assets.Exports.UObject export2) || !(export2 is T val))
		{
			export = null;
			return false;
		}
		export = val;
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<T> LoadAsync<T>(IFileProvider provider) where T : CUE4Parse.UE4.Assets.Exports.UObject
	{
		return ((await LoadAsync(provider)) as T) ?? throw new ParserException("Loaded SoftObjectProperty but it was of wrong type");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<T?> TryLoadAsync<T>(IFileProvider provider) where T : CUE4Parse.UE4.Assets.Exports.UObject
	{
		return (await TryLoadAsync(provider)) as T;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public CUE4Parse.UE4.Assets.Exports.UObject Load(IFileProvider provider)
	{
		return provider.LoadObject(AssetPathName.Text);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryLoad(IFileProvider provider, out CUE4Parse.UE4.Assets.Exports.UObject export)
	{
		return provider.TryLoadObject(AssetPathName.Text, out export);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<CUE4Parse.UE4.Assets.Exports.UObject> LoadAsync(IFileProvider provider)
	{
		return await provider.LoadObjectAsync(AssetPathName.Text);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<CUE4Parse.UE4.Assets.Exports.UObject?> TryLoadAsync(IFileProvider provider)
	{
		return await provider.TryLoadObjectAsync(AssetPathName.Text);
	}

	public override string ToString()
	{
		if (!string.IsNullOrEmpty(SubPathString))
		{
			return AssetPathName.Text + ":" + SubPathString;
		}
		if (!AssetPathName.IsNone)
		{
			return AssetPathName.Text;
		}
		return "";
	}
}
