using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class BuildTriggers
  {
    public override string ToString()
    {
      return "triggers";
    }
    [JsonProperty("trigger")]
    public List<BuildTrigger> Trigger { get; set; }
  }
}