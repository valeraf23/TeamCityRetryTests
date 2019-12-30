using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class FileWrapper
  {
    [JsonProperty("file")]
    public List<File> File { get; set; }
  }
}