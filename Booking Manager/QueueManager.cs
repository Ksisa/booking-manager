using System.Collections.Concurrent;

namespace Booking_Manager
{
    public class QueueManager<T> where T : notnull
    {
        private ConcurrentDictionary<T, Queue<Task>> _Queues;

        public QueueManager()
        {
            this._Queues = new();
        }

        public void AddToQueue(T id, Task task) 
        {
            if (!this._Queues.ContainsKey(id))
            {
                this._Queues.TryAdd(id, new Queue<Task>());
                this._Queues[id].Enqueue(task);
                Task.Run(() => this.RunQueue(id));
            }
            else
            {
                this._Queues[id].Enqueue(task);
            }
        }

        private void RunQueue(T id)
        {
            while(this._Queues.TryGetValue(id, out Queue<Task>? queue) && queue.Count > 0)
            {
                Task task = queue.Dequeue();
                task.Start();
                task.Wait();
                task.Dispose();
            }

            this._Queues.Remove(id, out _) ;
        }

        public void WaitForQueueToFinish(T id)
        {
            while (this._Queues.TryGetValue(id, out Queue<Task>? queue) && queue.Count > 0)
            {
                Task task = queue.Peek();
                task.Wait();
            }
        }
    }
}
