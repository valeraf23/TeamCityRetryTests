using System.Collections.Generic;
using System.Linq;
using TeamCityRetryTests.JobHandler;
using TeamCityRetryTests.Jobs;

namespace TeamCityRetryTests
{
    public class Worker
    {
        private readonly Handler _handler = new Handler(
            new TeamCityJobHandler(),
            new ReportPortalJobHandler(),
            new DefaultJobHandler());

        private readonly List<Job> _jobs = new List<Job>();

        public Worker(IEnumerable<Job> jobs)
        {
            var enumerable = jobs as Job[] ?? jobs.ToArray();
            if (enumerable.Any())
            {
                _jobs.AddRange(enumerable);
            }
        }

        public void Perform() => _handler.Handle(_jobs);
    }
}