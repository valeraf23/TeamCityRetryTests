using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class ProjectWrapper
  {
    [JsonProperty("project")]
    public List<Project> Project { get; set; }
  }
}