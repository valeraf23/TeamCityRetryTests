using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class Links
  {
    public override string ToString()
    {
      return "links";
    }
    [JsonProperty("link")]
    public List<Link> Link { get; set; }
  }
}
