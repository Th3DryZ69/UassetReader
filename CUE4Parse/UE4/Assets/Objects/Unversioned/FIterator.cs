using System;
using System.Collections;
using System.Collections.Generic;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Assets.Objects.Unversioned;

public class FIterator : IEnumerator<(int Val, bool IsNonZero)>, IEnumerator, IDisposable
{
	private int _schemaIt;

	private readonly BitArray _zeroMask;

	private int _zeroMaskIndex;

	private readonly IEnumerator<FFragment> _fragmentIt;

	private int _remainingFragmentValues;

	public bool IsNonZero
	{
		get
		{
			if (_fragmentIt.Current.HasAnyZeroes)
			{
				return !_zeroMask.GetOrFalse(_zeroMaskIndex);
			}
			return true;
		}
	}

	public (int Val, bool IsNonZero) Current => (Val: _schemaIt, IsNonZero: IsNonZero);

	object IEnumerator.Current => Current;

	public FIterator(FUnversionedHeader header)
	{
		_zeroMask = header.ZeroMask;
		_fragmentIt = header.Fragments.GetEnumerator();
		if (header.HasValues)
		{
			Skip();
		}
	}

	public bool MoveNext()
	{
		_schemaIt++;
		_remainingFragmentValues--;
		if (_fragmentIt.Current.HasAnyZeroes)
		{
			_zeroMaskIndex++;
		}
		if (_remainingFragmentValues == 0)
		{
			if (_fragmentIt.Current.IsLast)
			{
				return false;
			}
			_fragmentIt.MoveNext();
			Skip();
		}
		return true;
	}

	private void Skip()
	{
		_schemaIt += _fragmentIt.Current.SkipNum;
		while (_fragmentIt.Current.ValueNum == 0)
		{
			if (_fragmentIt.Current.IsLast)
			{
				throw new ParserException("Cannot receive last fragment in Skip()");
			}
			_fragmentIt.MoveNext();
			_schemaIt += _fragmentIt.Current.SkipNum;
		}
		_remainingFragmentValues = _fragmentIt.Current.ValueNum;
	}

	public void Reset()
	{
		_schemaIt = (_zeroMaskIndex = (_remainingFragmentValues = 0));
		_fragmentIt.Reset();
		Skip();
	}

	public void Dispose()
	{
		_fragmentIt.Dispose();
	}
}
