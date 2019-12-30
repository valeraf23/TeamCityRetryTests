using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class ChangeWrapper
  {
    [JsonProperty("change")]
    public List<Change> Change { get; set; }

    [JsonProperty("href")]
    public string Href { get; set; }
  }
}