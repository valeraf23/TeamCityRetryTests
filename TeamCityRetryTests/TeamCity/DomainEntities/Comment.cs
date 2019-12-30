using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class Comment
  {
    [JsonProperty("timestamp")]
    public string Timestamp { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("user")]
    public User User { get; set; }
  }
}