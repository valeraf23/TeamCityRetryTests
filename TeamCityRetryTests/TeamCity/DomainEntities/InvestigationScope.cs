using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class InvestigationScope
  {
    [JsonProperty("buildTypes")]
    public BuildTypeWrapper BuildTypes { get; set; }

    [JsonProperty("project")]
    public Project Project { get; set; }
  }
}