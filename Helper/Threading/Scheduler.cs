using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Helper.Threading
{
    public delegate void ThrowScheduleTaskException(ScheduleTaskExceptionEventArgs exception);
    public class Scheduler : IDisposable
    {
        private System.Threading.Timer _timer = null;
        private Dictionary<string, ScheduleTask> _tasks = new Dictionary<string, ScheduleTask>();
        public int Count
        {
            get
            {
                lock (_tasks)
                {
                    return _tasks.Count;
                }
            }
        }

        public event ThrowScheduleTaskException OnThrowScheduleTaskException;

        protected void RaiseThrowScheduleTaskException(ScheduleTaskExceptionEventArgs exception)
        {
            if (OnThrowScheduleTaskException != null)
            {
                OnThrowScheduleTaskException(exception);
            }
        }

        public Scheduler()
        {
            _timer = new Timer(timer_Elapsed, this, Timeout.Infinite, Timeout.Infinite);
        }



        void timer_Elapsed(object sender)
        {
            try
            {
                lock (_tasks)
                {
                    foreach (ScheduleTask task in _tasks.Values)
                    {
                        if (task.CanExecute())
                        {
                            task.CalcNewExecuteTime();
                            if (!task.CanConcurrent && task.IsRunning)
                            {
                                continue;
                            }
                            ThreadPool.QueueUserWorkItem(delegate(object o)
                            {
                                ScheduleTask t = o as ScheduleTask;
                                try
                                {
                                    if (!t.CanConcurrent)
                                    {
                                        if (!t.IsRunning)
                                        {
                                            t.IsRunning = true;
                                            try
                                            {
                                                t.Action(t, t.Data);
                                            }
                                            finally
                                            {
                                                t.IsRunning = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        t.Action(t, t.Data);
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    RaiseThrowScheduleTaskException(new ScheduleTaskExceptionEventArgs(ex, t));
                                }
                            }, task);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                RaiseThrowScheduleTaskException(new ScheduleTaskExceptionEventArgs(ex, null));
            }
            finally
            {
                ResetTimer();
            }
        }

        public void PushTask(ScheduleTask task)
        {
            PushTask(task, false);
        }

        public void PushTask(ScheduleTask task, bool executeImmediately)
        {
            task.Owner = this;
            if (!executeImmediately)
            {
                task.CalcNewExecuteTime();
            }
            lock (_tasks)
            {
                if (_tasks.ContainsKey(task.Name))
                {
                    throw new Exception("The schedule task '" + task.Name + "' exists!");
                }
                _tasks.Add(task.Name, task);
            }
            ResetTimer();
        }

        public void PushTask(string name, Action doAction, TimeSpan executeTime)
        {
            ScheduleTask task = new ScheduleTask(name, executeTime, delegate(ScheduleTask sender, object data)
            {

                try
                {
                    doAction();
                }
                finally
                {
                    sender.IntervalMilliseconds = (long)CalcInterval((TimeSpan)data);
                }
            }, (int)CalcInterval(executeTime));
            PushTask(task);
        }

        private static double CalcInterval(TimeSpan executeTime)
        {
            double interval;
            if (executeTime >= DateTime.Now.TimeOfDay)
            {
                interval = executeTime.Subtract(DateTime.Now.TimeOfDay).TotalMilliseconds;
            }
            else
            {
                interval = 86400000 - DateTime.Now.TimeOfDay.Subtract(executeTime).TotalMilliseconds;
            }
            return interval;
        }

        internal void ResetTimer()
        {
            long interval = -1;
            if (_tasks.Count > 0)
            {
                try
                {
                    interval = (_tasks.Values.Min<ScheduleTask, long>(x => x.ExecuteTime) - DateTime.UtcNow.Ticks) / 10000;
                    if (interval < 0L)
                    {
                        interval = 0L;
                    }
                }
                catch
                {
                }
            }
            lock (_timer)
            {
                _timer.Change(interval, Timeout.Infinite);
            }
        }

        public void RemoveTask(string taskName)
        {
            lock (_tasks)
            {
                _tasks.Remove(taskName);
            }
        }

        public bool Contains(string taskName)
        {
            lock (_tasks)
            {
                return _tasks.ContainsKey(taskName);
            }
        }

        public void ClearTask()
        {
            lock (_tasks)
            {
                _tasks.Clear();
            }
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
            if (_tasks != null)
            {
                _tasks.Clear();
                _tasks = null;
            }

        }
    }

    public class ScheduleTask
    {
        public string Name
        {
            get;
            private set;
        }

        internal Scheduler Owner
        {
            get;
            set;
        }

        internal bool IsRunning
        {
            get;
            set;
        }

        internal long ExecuteTime
        {
            get;
            set;
        }

        /// <summary>
        /// the default value is true.
        /// </summary>
        public bool CanConcurrent
        {
            get;
            private set;
        }

        public object Data
        {
            get;
            set;
        }
        public Action<ScheduleTask, object> Action
        {
            get;
            private set;
        }

        long _intervalTenMillionthseconds = 0;
        public long IntervalMilliseconds
        {
            get
            {
                return _intervalTenMillionthseconds / 10000;
            }
            set
            {
                _intervalTenMillionthseconds = value * 10000;
                if (Owner != null)
                {
                    CalcNewExecuteTime();
                    Owner.ResetTimer();
                }
            }
        }

        internal void CalcNewExecuteTime()
        {
            ExecuteTime = DateTime.UtcNow.Ticks + _intervalTenMillionthseconds;
        }

        internal bool CanExecute()
        {
            return DateTime.UtcNow.Ticks >= ExecuteTime;
        }

        public ScheduleTask(string name, Action<ScheduleTask, object> action, int intervalMilliSeconds)
            : this(name, null, action, intervalMilliSeconds, false)
        {
        }

        public ScheduleTask(string name, Action<ScheduleTask, object> action, int intervalMilliSeconds, bool canRepeat)
            : this(name, null, action, intervalMilliSeconds, canRepeat)
        {

        }

        public ScheduleTask(string name, object data, Action<ScheduleTask, object> action, int intervalMilliSeconds)
            : this(name, data, action, intervalMilliSeconds, false)
        {
        }

        public ScheduleTask(string name, object data, Action<ScheduleTask, object> action, int intervalMilliSeconds, bool canRepeat)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name is null!");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action is null!");
            }
            Name = name;
            Action = action;
            Data = data;
            ExecuteTime = 0;
            IntervalMilliseconds = intervalMilliSeconds;
            CanConcurrent = canRepeat;
        }
    }

    public class ScheduleTaskExceptionEventArgs : EventArgs
    {
        public ScheduleTask Task
        {
            get;
            private set;
        }

        public Exception Error
        {
            get;
            private set;
        }

        public ScheduleTaskExceptionEventArgs(Exception ex, ScheduleTask task)
        {
            Error = ex;
            Task = task;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Task.Name, Error);
        }
    }
}

