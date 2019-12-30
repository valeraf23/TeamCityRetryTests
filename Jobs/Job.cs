using System;
using System.Threading;

namespace TeamCityRetryTests.Jobs
{
    public class Job
    {
        private readonly Action _action;
        private int _performed;

        public Job(Action action)
        {
            _action = action;
        }

        public void Perform()
        {
            if (Interlocked.Increment(ref _performed) > 1)
            {
                throw new InvalidOperationException("Action performed multiple times");
            }

            _action();
        }
    }
}