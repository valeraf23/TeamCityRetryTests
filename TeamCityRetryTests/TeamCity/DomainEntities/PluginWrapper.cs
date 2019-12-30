using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class PluginWrapper
  {
    [JsonProperty("plugin")]
    public List<Plugin> Plugin { get; set; }
  }
}