using TeamCityRetryTests.TeamCity.DomainEntities;

namespace TeamCityRetryTests.TeamCity.ActionTypes
{
  public interface IStatistics
  {
    Statistics GetFields(string fields);
    Properties GetByBuildId(string buildId);
  }
}