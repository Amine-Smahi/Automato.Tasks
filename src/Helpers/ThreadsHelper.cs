using System.ComponentModel;
using System.Threading;

namespace Automato.Tasks.Helpers
{
    public static class ThreadsHelper
    {
        public static void MonitorWait(object syncObject)
        {
            Monitor.Wait(syncObject);
        }

        public static void Sleep(int waitingTime)
        {
            Thread.Sleep(waitingTime);
        }

        public static void PulseMonitor(AsyncCompletedEventArgs eventArgs)
        {
            lock (eventArgs.UserState)
            {
                Monitor.Pulse(eventArgs.UserState);
            }
        }
    }
}
