using TeamCityRetryTests.TeamCity.ActionTypes;

namespace TeamCityRetryTests.TeamCity
{
  public interface ITeamCityClient
  {
    IBuilds Builds { get; }
    IBuildQueue BuildQueue { get; }
    IBuildArtifacts Artifacts { get; }
    IStatistics Statistics { get; }
    ITests Tests { get; }
  }
}