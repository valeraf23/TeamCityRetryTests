using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class ProblemTarget
  {
    [JsonProperty("anyProblem")]
    public bool AnyProblem { get; set; }

    [JsonProperty("tests")]
    public Tests Tests { get; set; }

    [JsonProperty("problems")]
    public Problems Problems { get; set; }

  }
}