using System.Collections.Generic;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
    public class TestWrapper
    {
        public string Count { get; set; }

        public List<TestOccurrence> TestOccurrence { get; set; }
    }
}