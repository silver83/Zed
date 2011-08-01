using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Z.Common.Extensions
{
    public class MemoizedEnumerator<T> : IEnumerator<T>
    {
        private IEnumerator<T> _input;
        private List<T> _cache = new List<T>();

        public delegate void EnumerationCompletedHandler(IEnumerable<T> cache);
        public event EnumerationCompletedHandler OnEnumerationCompleted;

        public MemoizedEnumerator(IEnumerator<T> input)
        {
            _input = input;
        }

        public T Current
        {
            get
            {
                _cache.Add(_input.Current);
                return _input.Current;
            }
        }

        public bool MoveNext()
        {
            bool hasValue = _input.MoveNext();
            if (!hasValue)
                RaiseOnEnumerationCompleted();

            return hasValue;
        }

        private void RaiseOnEnumerationCompleted()
        {
            if (OnEnumerationCompleted != null)
                OnEnumerationCompleted(_cache);
        }

        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        #region IDisposable Members

        public void Dispose()
        {
            _input.Dispose();
            _input = null;
        }

        #endregion
    }

    public class MemoizedEnumerable<T> : IEnumerable<T>
    {
        private IEnumerable<T> _input;
        private IEnumerable<T> _cache;
        private MemoizedEnumerator<T> _memoizedEnumerator;

        public MemoizedEnumerable(IEnumerable<T> input)
        {
            _input = input;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_cache != null)
                return _cache.GetEnumerator();

            _memoizedEnumerator = new MemoizedEnumerator<T>(_input.GetEnumerator());
            _memoizedEnumerator.OnEnumerationCompleted += _memoizedEnumerator_OnEnumerationCompleted;
            return _memoizedEnumerator;
        }

        void _memoizedEnumerator_OnEnumerationCompleted(IEnumerable<T> cache)
        {
            _memoizedEnumerator.OnEnumerationCompleted -= _memoizedEnumerator_OnEnumerationCompleted;
            _cache = cache;
            _input = null;
            _memoizedEnumerator = null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public static class Memoize
    {
        public static IEnumerable<T> MemoizeAll<T>(this IEnumerable<T> input)
        {
            return new MemoizedEnumerable<T>(input);
        }
    }
}
