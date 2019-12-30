﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class InvestigationWrapper
  {
    [JsonProperty("investigation")]
    public List<Investigation> Investigation { get; set; }

    [JsonProperty("href")]
    public string Href { get; set; }
  }
}