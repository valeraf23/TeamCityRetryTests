using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class MetaData
  {
    public override string ToString()
    {
      return "metaData";
    }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("entries")]
    public Entries Entries { get; set; }
  }
}
