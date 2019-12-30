using System.Collections.Generic;
using TeamCityRetryTests.TeamCity.DomainEntities;

namespace TeamCityRetryTests.TeamCity.ActionTypes
{
    public interface ITests
    {
        List<TestOccurrence> ByBuildId(string buildId);
        List<TestOccurrence> FailedTestsByBuildId(string buildId);
        List<TestOccurrence> ByBuildIdWithPagination(string buildId, int start, int count);
        Test TestResultByTestOccurrenceId(string testOccurrenceId);
        List<TestOccurrence> TestHistoryByTestId(string testId);
        List<TestOccurrence> TestHistoryByTestName(string fullTestName);
        List<TestOccurrence> CurrentlyFailingByProjectId(string projectId);
        List<TestOccurrence> CurrentlyFailingByProjectName(string projectName);
    }
}