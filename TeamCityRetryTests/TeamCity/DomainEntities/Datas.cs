﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class Datas
  {
    public override string ToString()
    {
      return "datas";
    }
    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("data")]
    public List<MetaData> Data { get; set; }
  }
}
