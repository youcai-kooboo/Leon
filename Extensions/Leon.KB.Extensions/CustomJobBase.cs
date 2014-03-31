using System;
using System.Collections.Generic;
using System.Linq;
using Kooboo.Job;

namespace Leon.KB.Extensions
{
    public abstract class CustomJobBase : IJob
    {
        public static Dictionary<string, JobExecutor> Jobs = new Dictionary<string, JobExecutor>();

        public JobExecutor AttachJob(int interval, object executionState)
        {
            if (!Jobs.ContainsKey(this.ToString()))
            {
                lock (_lockHelper)
                {
                    if (!Jobs.ContainsKey(this.ToString()))
                    {
                        var jobExecutor = new JobExecutor((IJob)Activator.CreateInstance(this.GetType()), interval, executionState);
                        Jobs[this.ToString()] = jobExecutor;
                        jobExecutor.Start();
                        return jobExecutor;
                    }
                }
            }
            return Jobs[this.ToString()];
        }

        private string _jobName;
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_jobName))
            {
                _jobName = this.GetType().Name;
            }
            return _jobName;
        }

        readonly object _lockHelper = new object();
        public void Start()
        {
            lock (_lockHelper)
            {
                var job = Jobs.FirstOrDefault(it => it.Key == this.ToString()).Value;
                if (job != null)
                {
                    job.Start();
                }
            }
        }
        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            lock (Jobs)
            {
                var job = Jobs.FirstOrDefault(it => it.Key == this.ToString()).Value;
                if (job != null)
                {
                    job.Stop();
                }
            }
        }

        public virtual void Error(Exception e)
        {
            Kooboo.HealthMonitoring.Log.LogException(e);
        }

        public abstract void Execute(object executionState);
    }
}
