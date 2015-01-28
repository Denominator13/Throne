using System;
using System.Collections.Concurrent;

namespace Throne.Framework.Collections
{
    /// <summary>
    ///     Thread-safe object pool with generator function.
    /// </summary>
    /// <remarks>
    ///     Thread-safe meaning it wont starve the hardware while attempting to en/dequeue an object.
    ///     This class is not synchronized for serial access in a multi-threaded environment and may produce duplicates.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public sealed class ObjectPool<T> : ConcurrentQueue<T>
        where T : class
    {
        private readonly Func<T> _generator;

        public ObjectPool(Func<T> generator)
        {
            _generator = generator;
        }

        public void Drop(T item)
        {
            Enqueue(item);
        }

        public T Get()
        {
            T obj;
            return !TryDequeue(out obj) ? _generator() : obj;
        }
    }
}