using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.CMS.ModuleArea.Monitor.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kooboo.CMS.ModuleArea.Monitor.Services
{
    public class LogService
    {
        private static Queue<Log> _queue;
        private static ILogProvider _logProvider;
        static LogService()
        {
            _queue = new Queue<Log>();
            _logProvider = EngineContext.Current.Resolve<ILogProvider>();
        }

        public static void Push(Log log)
        {
            _queue.Enqueue(log);
            CheckTask();
        }

        private static Log Shift()
        {
            try
            {
                return _queue.Dequeue();
            }
            catch { }
            return null;
        }

        private static Task _logTask;
        private static bool TaskIsRunning()
        {
            return _logTask != null && _logTask.Status == TaskStatus.Running;
        }

        static Log log = null;
        private static void SaveLog()
        {
            while (true)
            {
                log = Shift();
                if (log == null)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    _logProvider.Add(log);
                    Thread.Sleep(200);
                }
            }
        }

        private static void CheckTask()
        {
            if (_logTask == null || !TaskIsRunning())
            {
                _logTask = new Task(SaveLog);
                _logTask.Start();
            }
        }
    }
}