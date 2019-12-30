using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class BuildTypeWrapper
  {
    [JsonProperty("buildType")]
    public List<BuildConfig> BuildType { get; set; }
  }
}