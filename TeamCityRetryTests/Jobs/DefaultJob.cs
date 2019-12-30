using System;

namespace TeamCityRetryTests.Jobs
{
    public class DefaultJob : Job
    {
        public DefaultJob(Action action) : base(action)
        {
        }
    }
}