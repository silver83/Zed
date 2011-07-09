using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Z.Common.Logging;

namespace Z.Algorithms
{
    //role is to manage a queue, dequeue work items, and raise events
    public class AsyncWorkerCoordinator<T> : IDisposable
    {
        private static readonly string _runningStateError =
            string.Format("{0} is already in the running state",
                            typeof(AsyncWorkerCoordinator<>).Name);

        private AutoResetEvent _event = new AutoResetEvent(false);
        private int _stop = 0;
        private int _isRunning = 0;

        Func<T, bool> _callback;
        Queue<T> _processResults = new Queue<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback">Performs an action on the result of an asyncronous process. Should return false when the process completed, otherwise - false.</param>
        public AsyncWorkerCoordinator(Func<T, bool> callback)
        {
            _callback = callback;
        }

        private bool Disposed
        {
            get { return (Interlocked.CompareExchange(ref _disposed, 1, 1) == 1); }
        }
                
        public void Run(int timeoutMilliseconds)
        {
            //perform state checks
            if (Disposed)
                throw new ObjectDisposedException(typeof(AsyncWorkerCoordinator<T>).Name);

            if (Interlocked.CompareExchange(ref _isRunning, 1, 0) == 1)
                throw new InvalidOperationException(_runningStateError);

            RunInternal(timeoutMilliseconds);
            Interlocked.Exchange(ref _isRunning, 0);
        }

        //Will stop processing the current "Run" session
        public void Stop()
        {
            Interlocked.Exchange(ref _stop, 1);
            _event.Set();
        }

        /// <summary>
        /// The callback function for async calls
        /// This can be called before "run"
        /// </summary>
        /// <param name="processResult"></param>
        public void NotifyProcessCompleted(T processResult)
        {
            if (Disposed)
                throw new ObjectDisposedException(typeof(AsyncWorkerCoordinator<T>).Name);

            lock (_processResults)
            {
                _processResults.Enqueue(processResult);
            }

            _event.Set();
        }

        private void RunInternal(int timeoutMilliseconds)
        {
            bool signaled = true;
            
            //timeout target - if we reach this date, we timed out
            DateTime target = DateTime.Now + TimeSpan.FromMilliseconds(timeoutMilliseconds);

            lock (_disposeLock)
            {               
                while (true)
                {
                    //if we have more _processResults to work with - just keep processing.
                    if (_processResults.Count == 0)
                    {
                        double timeToWait = (target - DateTime.Now).TotalMilliseconds;

                        //if not - calculate time we have until timeout, and wait
                        signaled = _event.WaitOne((int)Math.Max(timeToWait, 0));
                        
                        //there was a timeout
                        if (!signaled)
                            throw new TimeoutException("The operation has timed out while waiting for ProcessCompleted notifications");
                    }
                    else
                    {
                        _event.Reset();
                    }
    
                    //check if someone called stop.
                    if (Interlocked.CompareExchange(ref _stop, 1, 1) == 1)
                        break;

                    try
                    {
                        T result;
                        lock (_processResults)
                        {
                            result = _processResults.Dequeue();
                        }                        
                        if (!_callback(result))
                            break; //callback decided its time to stop
                    }
                    catch (Exception e)
                    {
                        Log.Application.Error(e);
                    }
                }
            }
        }

        private int _disposed = 0;
        private object _disposeLock = new object();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_disposeLock)
            {
                _disposed = 1;
                if (disposing && _event != null)
                    _event.Close();

                _callback = null;
                _event = null;
            }
        }
    }
}
