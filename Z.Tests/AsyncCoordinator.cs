using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Z.Algorithms;
using System.Threading;
using System.Diagnostics;

namespace Z.Tests
{
    [TestFixture]
    public class AsyncCoordinator
    {
        [Test]
        public void AsyncCoordinator_Timeout_Exception()
        {
            int items = 5;

            //predefine the work you want done one the result
            Func<int, bool> callback = x =>
                {
                    //when in here - u cna set captued variables without worrying about multithreading - its called in the same thread as Run()
                    Console.WriteLine(x);
                    return Interlocked.Decrement(ref items) != 0;
                };

            bool thrown = false;
            try
            {
                using (AsyncWorkerCoordinator<int> wrapper = new AsyncWorkerCoordinator<int>(callback))
                {
                    //make a bunch of calls with callbacks
                    for (int i = 0; i < items - 1; i++)
                        ThreadPool.QueueUserWorkItem(x => { wrapper.NotifyProcessCompleted((int)x); }, i);


                    wrapper.Run(500);
                }
            }
            catch (TimeoutException)
            {
                thrown = true;
            }
            Assert.IsTrue(thrown);
        }


        [Test]
        public void AsyncCoordinator_DoubleRun_Exception()
        {
            //predefine the work you want done one the result
            Func<string, bool> callback = x =>
            {
                //never called
                return true;
            };

            bool thrown = false;
            try
            {
                using (AsyncWorkerCoordinator<string> wrapper = new AsyncWorkerCoordinator<string>(callback))
                {
                    ThreadPool.QueueUserWorkItem(x => { try { wrapper.Run(1000); } catch { } });
                    //make a bunch of calls with callbacks
                    Thread.Sleep(1000);
                    wrapper.Run(1000);
                }
            }
            catch (InvalidOperationException)
            {
                thrown = true;
            }
            Assert.IsTrue(thrown);
        }

        [Test]
        public void AsyncCoordinator_NormalRun_NoException()
        {
            int items = 1000;
            Func<int, bool> callback = x =>
            {
                //when in here - u can set captured variables without worrying about multithreading - its called in the same thread as Run()
                Console.WriteLine(x);
                return Interlocked.Decrement(ref items) != 0;
            };

            bool thrown = false;
            try
            {
                using (AsyncWorkerCoordinator<int> wrapper = new AsyncWorkerCoordinator<int>(callback))
                {
                    //make a bunch of calls with callbacks
                    for (int i = 0; i < items; i++)
                        ThreadPool.QueueUserWorkItem(x => { wrapper.NotifyProcessCompleted((int)x); }, i);

                    wrapper.Run(2000);
                }
            }
            catch (TimeoutException)
            {
                thrown = true;
            }
            Assert.IsFalse(thrown);
        }
    }
}
