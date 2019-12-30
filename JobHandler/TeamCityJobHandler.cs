﻿using System.Collections.Generic;
using System.Linq;
using TeamCityRetryTests.Jobs;

namespace TeamCityRetryTests.JobHandler
{
    public class TeamCityJobHandler : IReceiver<Job>
    {
        public void Handle(IEnumerable<Job> request)
        {
            var res = request.OfType<TeamCityJob>().FirstOrDefault();
            res?.Perform();
        }
    }
}