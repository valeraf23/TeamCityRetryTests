using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class Artifacts
  {
    [JsonProperty("href")]
    public string Href { get; set; }
  }
}