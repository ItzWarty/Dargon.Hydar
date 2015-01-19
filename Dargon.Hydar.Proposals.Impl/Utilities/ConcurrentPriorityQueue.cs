using ItzWarty.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ItzWarty;

namespace Dargon.Hydar.Proposals.Utilities {
   public class ConcurrentPriorityQueue<T> : IPriorityQueue<T> where T : IComparable<T> {
      private readonly ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
      private readonly IPriorityQueue<T> inner = new PriorityQueue<T>();

      public TResult ReadOperation<TResult>(Func<TResult> func) {
         bool success = false;
         try {
            success = readerWriterLock.TryEnterReadLock(Timeout.Infinite);
            return func();
         } finally {
            if (success) {
               readerWriterLock.ExitReadLock();
            }
         }
      }

      public TResult WriteOperation<TResult>(Func<TResult> func) {
         bool success = false;
         try {
            success = readerWriterLock.TryEnterWriteLock(Timeout.Infinite);
            return func();
         } finally {
            if (success) {
               readerWriterLock.ExitWriteLock();
            }
         }
      }

      public int Count { get { return ReadOperation(() => inner.Count); } }
      public bool Empty { get { return ReadOperation(() => inner.Empty); } }

      public void Add(T item) {
         WriteOperation(() => {
            return inner.With(x => x.Add(item));
         });
      }

      public void Clear() {
         WriteOperation(() => {
            return inner.With(x => x.Clear());
         });
      }

      public bool Contains(T item) {
         return ReadOperation(() => inner.Contains(item));
      }

      public void CopyTo(T[] array, int arrayIndex) {
         ReadOperation(() => inner.With(x => x.CopyTo(array, arrayIndex)));
      }

      public T Dequeue() {
         return WriteOperation(() => inner.Dequeue());
      }

      public void Enqueue(T item) {
         WriteOperation(() => inner.With(x => x.Enqueue(item)));
      }

      public IEnumerator<T> GetEnumerator() {
         return ReadOperation(() => inner.GetEnumerator());
      }

      public T Peek() {
         return ReadOperation(() => inner.Peek());
      }

      public bool Remove(T value) {
         return WriteOperation(() => inner.Remove(value));
      }

      public T[] ToArray() {
         return ReadOperation(() => inner.ToArray());
      }

      public void TrimExcess() {
         WriteOperation(() => inner.With(x => x.TrimExcess()));
      }

      IEnumerator IEnumerable.GetEnumerator() {
         return GetEnumerator();
      }
   }
}
