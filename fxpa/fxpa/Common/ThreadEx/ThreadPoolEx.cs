// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace fxpa
{
    public class ThreadPoolEx
    {
        /// <summary>
        /// Running threads.
        /// </summary>
        public volatile int MaximumSimultaniouslyRunningThreadsAllowed = 50;
        
        /// <summary>
        /// Total threads (running, sleeping, suspended, etc.)
        /// </summary>
        public volatile int MaximumTotalThreadsAllowed = 500;

        class TargetInfo
        {
            internal TargetInfo(Delegate d, object[] args)
            {
                Target = d;
                Args = args;
            }

            internal readonly Delegate Target;
            internal readonly object[] Args;
        }

        List<Thread> _threads = new List<Thread>();

        List<TargetInfo> _infos = new List<TargetInfo>();

        public int QueuedItemsCount
        {
            get { lock (_infos) { return _infos.Count; } }
        }

        delegate void InvokeDelegate(TargetInfo info);

        /// <summary>
        /// 
        /// </summary>
        public ThreadPoolEx()
        {
        }

        public void Queue(Delegate d, params object[] args)
        {
            if (d == null)
            {
                return;
            }

            lock (_infos)
            {
                TargetInfo info = new TargetInfo(d, args);
                _infos.Add(info);
            }

            ProcessQueue();
        }

        /// <summary>
        /// LOCK _threads before calling.
        /// We need to count only those actively working.
        /// </summary>
        /// <returns></returns>
        int GetRunningThreadsCount()
        {
            int result = 0;
            foreach(Thread t in _threads)
            {
                if (t.ThreadState == System.Threading.ThreadState.Running)
                {
                    result++;
                }
            }

            return result;
        }

        void ProcessQueue()
        {
            lock (_infos)
            {
                if (_infos.Count == 0)
                {
                    return;
                }
            }

            lock (_threads)
            {
                // Keep these inside the lock section.
                if (GetRunningThreadsCount() >= MaximumSimultaniouslyRunningThreadsAllowed)
                {
                    //TracerHelper.Trace("Too many running threads, suspeding.");
                    Console.WriteLine("Too many running threads, suspeding.");
                    return;
                }
                //Console.WriteLine(" _info " + ((TargetInfo)_infos[0]).Target.Target + ((TargetInfo)_infos[0]).Target.Method );
                //_threads.Clear();
                //foreach (Thread thd in _threads)
                //{
                //    Console.WriteLine(thd.ToString());
                //}
                Thread thread = new Thread(new ThreadStart(Execute));
                _threads.Add(thread);

                if (Debugger.IsAttached)
                {// Since we have debugger attached, also show the caller.
                    thread.Name = "FireAndForget slow trace maybe from "; // +TracerHelper.GetFullCallingMethodName(3);
                }
                else
                {
                    thread.Name = "FireAndForget";
                }

                thread.Start();
            }
        }

        void Execute()
        {
            while (true)
            {
                TargetInfo targetInfo;
                lock (_infos)
                {
                    if (_infos.Count == 0)
                    {
                        break;
                    }

                    targetInfo = _infos[0];
                    _infos.RemoveAt(0);
                }

                targetInfo.Target.DynamicInvoke(targetInfo.Args);
            }

            lock (_threads)
            {
                _threads.Remove(Thread.CurrentThread);
            }
        }
    }
}
