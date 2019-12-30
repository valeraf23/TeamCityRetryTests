using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class BuildSteps
  {
    public override string ToString()
    {
      return "steps";
    }
    [JsonProperty("step")]
    public List<BuildStep> Step { get; set; }
  }
}