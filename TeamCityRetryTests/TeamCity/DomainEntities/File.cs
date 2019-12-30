using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class File
  {
    [JsonProperty("relative-file")]
    public string Relativefile { get; set; }
  }
}