using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class Parameters
  {
    public override string ToString()
    {
      return "parameters";
    }
    [JsonProperty("property")]
    public List<Property> Property { get; set; }
  }
}