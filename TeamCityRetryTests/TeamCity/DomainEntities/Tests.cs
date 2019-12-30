using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class Tests
  {
    [JsonProperty("test")]
    public List<Test> Test { get; set; }
  }
}