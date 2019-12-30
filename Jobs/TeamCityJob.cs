using System;

namespace TeamCityRetryTests.Jobs
{
    public class TeamCityJob:Job
    {
        public TeamCityJob(Action action) : base(action)
        {
        }
    }
}