﻿using Newtonsoft.Json;

namespace TeamCityRetryTests.TeamCity.DomainEntities
{
  public class Revision
  {
    [JsonProperty("Version")]
    public string Version { get; set; }

    [JsonProperty("vcs-root-instance")]
    public VcsRoot VcsRootInstance { get; set; }

    [JsonProperty("vcsBranchName")]
    public string VcsBranchName { get; set; }

    public override string ToString()
    {
      return Version;
    }
  }
}
