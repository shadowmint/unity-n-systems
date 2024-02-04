using System;
using System.Collections.Generic;
using System.Linq;

namespace N.Package.GameSystems.Utility.SceneTools
{
    public abstract class NSceneEventStream
    {
        private static readonly Lazy<List<IDisposable>> Data = new(() => new List<IDisposable>());

        public static void Add<T>(NSceneEventStream<T> stream)
        {
            if (!Data.Value.Contains(stream))
            {
                Data.Value.Add(stream);
            }
        }

        public static void ClearAllStreams()
        {
            var targets = Data.Value.ToArray();
            foreach (var disposable in targets)
            {
                disposable.Dispose();
            }

            Data.Value.Clear();
        }
    }

    public class NSceneEventStream<T> : IDisposable
    {
        private static readonly Lazy<Queue<T>> Data = new(() => new Queue<T>());

        public bool IsEmpty => Count == 0;

        public bool Any => Count > 0;

        public int Count => Data.Value.Count;

        public List<T> Consume()
        {
            var values = Data.Value.ToList();
            Data.Value.Clear();
            return values;
        }

        public List<T> Peek()
        {
            return Data.Value.ToList();
        }

        public void Add(T value)
        {
            NSceneEventStream.Add(this);
            Data.Value.Enqueue(value);
        }

        public void Clear()
        {
            Data.Value.Clear();
        }

        public List<T> Take(int count)
        {
            var rtn = new List<T>();
            if (count <= 0) return rtn;

            for (var i = 0; i < count && Any; i++)
            {
                var next = Data.Value.Dequeue();
                rtn.Add(next);
            }

            return rtn;
        }

        public List<T> TakeAll()
        {
            var count = Data.Value.Count;
            return Take(count);
        }

        public void AddRange(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                Add(value);
            }
        }

        /// <summary>
        /// Clear held data by all instances
        /// </summary>
        public void Dispose()
        {
            Clear();
        }
    }
}