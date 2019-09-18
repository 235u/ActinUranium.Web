using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ActinUranium.Web.Helpers
{
    [SuppressMessage(
        "Naming",
        "CA1710:Identifiers should have correct suffix",
        Justification = "Generalized data structure that might be extended")]
    internal class Lottery<T> : IEnumerable
    {
        private const string PoolEmptyMessage = "The element pool is empty.";
        private static readonly Random Random = new Random();

        private readonly List<T> _elementPool;

        public Lottery()
        {
            _elementPool = new List<T>();
        }

        public Lottery(IEnumerable<T> elements)
        {
            _elementPool = new List<T>(elements);
        }

        public int Count => _elementPool.Count;

        public IEnumerator GetEnumerator()
        {
            return _elementPool.GetEnumerator();
        }

        public void Add(T element)
        {
            _elementPool.Add(element);
        }

        public T Next()
        {
            if (_elementPool.Count == 0)
            {
                throw new InvalidOperationException(PoolEmptyMessage);
            }

            int index = Random.Next(maxValue: _elementPool.Count);
            return _elementPool[index];
        }

        public T Pull()
        {
            if (_elementPool.Count == 0)
            {
                throw new InvalidOperationException(PoolEmptyMessage);
            }

            int index = Random.Next(maxValue: _elementPool.Count);
            T element = _elementPool[index];
            _elementPool.RemoveAt(index);
            return element;
        }
    }
}
