﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class Entries
  {
    public override string ToString()
    {
      return "entries";
    }
    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("entry")]
    public List<Entry> Entry { get; set; }
  }
}
