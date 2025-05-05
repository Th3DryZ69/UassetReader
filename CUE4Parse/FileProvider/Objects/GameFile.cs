using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CUE4Parse.Compression;
using CUE4Parse.UE4.Readers;
using CUE4Parse.Utils;
using Serilog;

namespace CUE4Parse.FileProvider.Objects;

public abstract class GameFile
{
	public static readonly string[] Ue4PackageExtensions = new string[2] { "uasset", "umap" };

	public static readonly string[] Ue4KnownExtensions = new string[5] { "uasset", "umap", "uexp", "ubulk", "uptnl" };

	public abstract bool IsEncrypted { get; }

	public abstract CompressionMethod CompressionMethod { get; }

	public string Path { get; protected internal set; }

	public long Size { get; protected set; }

	public string PathWithoutExtension => Path.SubstringBeforeLast('.');

	public string Name => Path.SubstringAfterLast('/');

	public string NameWithoutExtension => Name.SubstringBeforeLast('.');

	public string Extension => Path.SubstringAfterLast('.');

	public bool IsUE4Package => Ue4PackageExtensions.Contains<string>(Extension, StringComparer.OrdinalIgnoreCase);

	protected GameFile()
	{
	}

	protected GameFile(string path, long size)
	{
		Path = path;
		Size = size;
	}

	public abstract byte[] Read();

	public abstract FArchive CreateReader();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual bool TryRead(out byte[] data)
	{
		try
		{
			data = Read();
			return true;
		}
		catch
		{
			data = null;
			return false;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual bool TryCreateReader(out FArchive reader)
	{
		try
		{
			reader = CreateReader();
			return true;
		}
		catch (Exception exception)
		{
			Log.Warning(exception, "Couldn't create GameFile reader");
			reader = null;
			return false;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual async Task<byte[]> ReadAsync()
	{
		return await Task.Run((Func<byte[]>)Read);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual async Task<FArchive> CreateReaderAsync()
	{
		return await Task.Run((Func<FArchive>)CreateReader);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual async Task<byte[]?> TryReadAsync()
	{
		return await Task.Run(delegate
		{
			TryRead(out byte[] data);
			return data;
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual async Task<FArchive?> TryCreateReaderAsync()
	{
		return await Task.Run(delegate
		{
			TryCreateReader(out FArchive reader);
			return reader;
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override string ToString()
	{
		return Path;
	}
}
