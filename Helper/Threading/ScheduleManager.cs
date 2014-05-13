using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper.Threading
{
    public static class ScheduleManager
    {
        static readonly Scheduler _scheduler = new Scheduler();

        static ScheduleManager()
        {
            _scheduler.OnThrowScheduleTaskException += RaiseThrowScheduleTaskException;
        }

        public static event ThrowScheduleTaskException OnThrowScheduleTaskException;

        private static void RaiseThrowScheduleTaskException(ScheduleTaskExceptionEventArgs exception)
        {
            if (OnThrowScheduleTaskException != null)
            {
                OnThrowScheduleTaskException(exception);
            }
        }

        public static void PushTask(ScheduleTask task)
        {
            _scheduler.PushTask(task);
        }

        public static void PushTask(ScheduleTask task, bool executeImmediately)
        {
            _scheduler.PushTask(task, executeImmediately);
        }

        public static void PushTask(string name,Action doAction, TimeSpan executeTime)
        {
            _scheduler.PushTask(name, doAction, executeTime);
        }

        public static void RemoveTask(string taskName)
        {
            _scheduler.RemoveTask(taskName);
        }

        public static bool Contains(string taskName)
        {
            return _scheduler.Contains(taskName);
        }

        public static void ClearTask()
        {
            _scheduler.ClearTask();
        }
    }
}
