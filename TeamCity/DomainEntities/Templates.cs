using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class Templates
  {
    [JsonProperty("buildType")]
    public List<Template> BuildType { get; set; }
  }
}