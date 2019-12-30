using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class Mutes
  {
    [JsonProperty("mutes")]
    public List<Mute> Mute { get; set; }
  }
}