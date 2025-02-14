using System;
using System.Collections.Generic;

namespace ShowBPM
{
    public class CallRateTracker
    {
        private readonly Queue<DateTime> callTimestamps = new Queue<DateTime>();
        private readonly object lockObject = new object();
        private DateTime lastCleanupTime;

        public void TrackCall()
        {
            lastCleanupTime = DateTime.Now;
            lock (lockObject)
            {
                callTimestamps.Enqueue(DateTime.Now);
            }
        }

        public int GetCallsPerSecond()
        {
            lock (lockObject)
            {
                // 清理超过一秒的调用记录
                while (callTimestamps.Count > 0 && (DateTime.Now - callTimestamps.Peek()).TotalSeconds > 1)
                {
                    callTimestamps.Dequeue();
                }

                // 如果距离最后一次清理已经超过一秒，则清空调用记录
                if ((DateTime.Now - lastCleanupTime).TotalSeconds > 1)
                {
                    callTimestamps.Clear();
                    lastCleanupTime = DateTime.Now;
                }

                return callTimestamps.Count;
            }
        }
    }
}