using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EMCL.Utils;
using static EMCL.Modules.ModLogger;
using System.Threading;

namespace EMCL.Modules
{
    internal static class ModThread
    {
        public static List<Thread> threads = new List<Thread>();

        public static Thread RunThread(Action action, string name, ThreadPriority priority = ThreadPriority.Normal, bool addToPool = true)
        {
            Thread th = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (ThreadInterruptedException ex)
                {
                    Log(ex, $"{name}：线程执行失败");
                }
                catch (ThreadAbortException ex)
                {
                    ModLogger.Log($"[Thread]<{name}> 线程 {name} 被迫终止！");
                }
                catch (Exception ex)
                {
                    Log(ex, $"{name}：线程执行失败", LogLevel.Error);
                }
            })
            { Name = name, Priority = priority };
            th.Start();
            if (addToPool) { threads.Add(th); }
            return th;
        }
    }
}
