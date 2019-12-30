using System;
using System.Collections.Generic;
using TeamCityRetryTests.TeamCity.DomainEntities;
using TeamCityRetryTests.TeamCity.Locators;

namespace TeamCityRetryTests.TeamCity.ActionTypes
{
  public interface IBuilds
  {
    Build ById(string id);
    Builds GetFields(string fields);
    List<Build> AllBuildsOfStatusSinceDate(DateTime date, BuildStatus buildStatus);
    List<Build> AllRunningBuild();
    List<Build> AllSinceDate(DateTime date, long count = 100, List<string> param = null);
    List<Build> ByBuildLocator(BuildLocator locator);
    List<Build> ByBuildLocator(BuildLocator locator, List<String> param);
    List<Build> RetrieveEntireBuildChainFrom(string buildConfigId, bool includeInitial = true, List<string> param = null);
    List<Build> RetrieveEntireBuildChainTo(string buildConfigId, bool includeInitial = true, List<string> param = null);
    void DownloadLogs(string projectId, bool zipped, Action<string> downloadHandler);
  }
}
