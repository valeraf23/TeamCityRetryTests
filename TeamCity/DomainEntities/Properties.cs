using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class Properties
  {
    public Properties()
    {
      Property = new List<Property>();
    }

    public void Add(string name, string value)
    {
      Property.Add(new Property(name, value));
    }

    public override string ToString()
    {
      return "properties";
    }

    [JsonProperty("property")]
    public List<Property> Property { get; set; }

    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("href")]
    public string Href { get; set; }
  }
}