
namespace Sdk.Common.Utilities
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskQueue
    {
        private readonly SemaphoreSlim _semaphore;

        public TaskQueue()
        {
            _semaphore = new SemaphoreSlim(1);
        }

        public TaskQueue(int semaphoreCount)
        {
            _semaphore = new SemaphoreSlim(semaphoreCount, semaphoreCount);
        }

        public async Task<T> Enqueue<T>(Func<Task<T>> task)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await task();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task Enqueue(Func<Task> task)
        {
            await _semaphore.WaitAsync();
            try
            {
                await task();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
