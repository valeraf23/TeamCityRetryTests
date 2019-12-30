using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class RoleWrapper
  {
    [JsonProperty("role")]
    public List<Role> Role { get; set; }
  }
}