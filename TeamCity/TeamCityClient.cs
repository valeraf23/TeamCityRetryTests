using TeamCityRetryTests.TeamCity.ActionTypes;
using TeamCityRetryTests.TeamCity.Connection;

namespace TeamCityRetryTests.TeamCity
{
  public class TeamCityClient : ITeamCityClient
  {
    private readonly ITeamCityCaller _caller;
    private IBuilds _builds;
    private IBuildQueue _buildQueue;
    private IBuildArtifacts _artifacts;
    private IStatistics _statistics;
    private ITests _tests;

    public TeamCityClient(ITeamCityCaller teamCityCaller)
    {
      _caller = teamCityCaller;
    }

    
    public IBuilds Builds => _builds ?? (_builds = new Builds(_caller));

    public IBuildQueue BuildQueue => _buildQueue ?? (_buildQueue = new BuildQueue(_caller));

    public IBuildArtifacts Artifacts => _artifacts ?? (_artifacts = new BuildArtifacts(_caller));

    public IStatistics Statistics => _statistics ?? (_statistics = new Statistics(_caller));

    public ITests Tests => _tests ?? (_tests = new Tests(_caller));
  }
}