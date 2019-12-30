using TeamCityRetryTests.TeamCity.Connection;
using TeamCityRetryTests.TeamCity.DomainEntities;

namespace TeamCityRetryTests.TeamCity.ActionTypes
{
  public class Statistics : IStatistics
  {
    private readonly ITeamCityCaller _mCaller;
    private string _mFields;

    internal Statistics(ITeamCityCaller caller)
    {
      _mCaller = caller;
    }

    public Statistics GetFields(string fields)
    {
      var newInstance = (Statistics) MemberwiseClone();
      newInstance._mFields = fields;
      return newInstance;
    }

    public Properties GetByBuildId(string buildId)
    {
      return _mCaller.GetFormat<Properties>(ActionHelper.CreateFieldUrl("/builds/id:{0}/statistics", _mFields), buildId);
    }
  }
}