using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class CompatibleAgents
  {
    [JsonProperty("href")]
    public string Href { get; set; }

    [JsonProperty("agent")]
    public List<Agent> Agent { get; set; }
  }
}