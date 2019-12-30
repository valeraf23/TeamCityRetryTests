using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class RelatedIssues
  {
    [JsonProperty("href")]
    public string Href { get; set; }
  }
}