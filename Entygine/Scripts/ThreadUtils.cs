using System.Threading;

namespace Entygine
{
    public static class ThreadUtils
    {
        private static int mainThreadID;

        internal static void SetMainThread()
        {
            if (mainThreadID != 0)
                throw new System.Exception("Main thread was already set.");

            mainThreadID = Thread.CurrentThread.ManagedThreadId;
        }

        public static void ThrowWorkerThread()
        {
            if (!IsMainThread)
                throw new System.Exception("Method can only be called from Main Thread.");
        }

        public static bool IsMainThread => Thread.CurrentThread.ManagedThreadId == mainThreadID;
    }
}
