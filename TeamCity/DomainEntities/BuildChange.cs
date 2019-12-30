using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class BuildChange
  {
    [JsonProperty("nextBuild")]
    public Build NextBuild { get; set; }

    [JsonProperty("prevBuild")]
    public Build PrevBuild { get; set; }
  }
}