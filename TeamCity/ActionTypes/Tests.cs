using System.Collections.Generic;
using TeamCityRetryTests.TeamCity.Connection;
using TeamCityRetryTests.TeamCity.DomainEntities;

namespace TeamCityRetryTests.TeamCity.ActionTypes
{
    internal class Tests : ITests
    {
        private readonly ITeamCityCaller _caller;

        internal Tests(ITeamCityCaller caller)
        {
            _caller = caller;
        }

        public Test TestResultByTestOccurrenceId(string testOccurrenceId)
        {
            var test = _caller.GetFormat<Test>("/testOccurrences/{0}", testOccurrenceId);
            return test;
        }

        public List<TestOccurrence> ByBuildId(string buildId)
        {
            var tests = _caller.GetFormat<TestWrapper>("/testOccurrences?locator=build:id:{0}", buildId);
            return tests.TestOccurrence;
        }

        public List<TestOccurrence> FailedTestsByBuildId(string buildId)
        {
            var tests = _caller.GetFormat<TestWrapper>("/testOccurrences?locator=build:(id:{0}),status:FAILURE",
                buildId);
            return tests.TestOccurrence;
        }

        public List<TestOccurrence> ByBuildIdWithPagination(string buildId, int start, int count)
        {
            var tests = _caller.GetFormat<TestWrapper>("/testOccurrences?locator=build:(id:{0}),start:{1},count:{2}",
                buildId, start, count);
            return tests.TestOccurrence;
        }

        public List<TestOccurrence> TestHistoryByTestId(string testId)
        {
            var tests = _caller.GetFormat<TestWrapper>("/testOccurrences?locator=test:id:{0}", testId);
            return tests.TestOccurrence;
        }

        public List<TestOccurrence> TestHistoryByTestName(string fullTestName)
        {
            var tests = _caller.GetFormat<TestWrapper>("/testOccurrences?locator=build:name:{0}", fullTestName);
            return tests.TestOccurrence;
        }

        public List<TestOccurrence> CurrentlyFailingByProjectId(string projectId)
        {
            var tests = _caller.GetFormat<TestWrapper>(
                "/testOccurrences?locator=currentlyFailing:true,affectedProject:id:{0}", projectId);
            return tests.TestOccurrence;
        }

        public List<TestOccurrence> CurrentlyFailingByProjectName(string projectName)
        {
            var tests = _caller.GetFormat<TestWrapper>(
                "/testOccurrences?locator=currentlyFailing:true,affectedProject:name:{0}", projectName);
            return tests.TestOccurrence;
        }

    }
}