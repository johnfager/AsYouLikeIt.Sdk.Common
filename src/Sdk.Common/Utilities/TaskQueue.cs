
namespace JFM.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskQueue
    {
        private SemaphoreSlim semaphore;

        public TaskQueue()
        {
            semaphore = new SemaphoreSlim(1);
        }

        public TaskQueue(int semaphoreCount)
        {
            semaphore = new SemaphoreSlim(semaphoreCount, semaphoreCount);
        }

        public async Task<T> Enqueue<T>(Func<Task<T>> task)
        {
            await semaphore.WaitAsync();
            try
            {
                return await task();
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task Enqueue(Func<Task> task)
        {
            await semaphore.WaitAsync();
            try
            {
                await task();
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
