﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class Revisions
  {
    [JsonProperty("count")]
    public int Count { get; set; }
    [JsonProperty("revision")]
    public List<Revision> Revision { get; set; }
  }
}
