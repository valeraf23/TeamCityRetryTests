using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class UserWrapper
  {
    [JsonProperty("user")]
    public List<User> User { get; set; }
  }
}