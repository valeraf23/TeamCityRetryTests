using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class UserGroupWrapper
  {
    [JsonProperty("group")]
    public List<Group> Group { get; set; }
  }
}