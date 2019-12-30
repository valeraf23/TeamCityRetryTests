using System;

namespace TeamCityRetryTests.Jobs
{
    public class ReportPortalJob : Job
    {
        public ReportPortalJob(Action action) : base(action)
        {
        }
    }
}